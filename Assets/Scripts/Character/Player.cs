using System.Collections;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques au joueur
/// </summary>
public class Player : Character
{
    // Instance de classe (singleton)
    private static Player instance;

    // Propriété d'accès à l'instance
    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type Player (doit être unique)
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    // Mana du joueur
    [SerializeField]
    private Stat mana = default;

    // XP du joueur
    [SerializeField]
    private Stat xp = default;

    // Texte du niveau du joueur
    [SerializeField]
    private Text levelText = default;

    // Tableau des positions pour lancer les sorts
    [SerializeField]
    private Transform[] exitPoints = default;

    // Tableau des paires de blocs pour bloquer la ligne de mire
    [SerializeField]
    private Block[] blocks = default;

    // Référence sur l'emplacement de l'equipement sur le personnage
    [SerializeField]
    private GearSocket[] gearSockets = default;


    // Mana initiale du joueur (readonly)
    private readonly float initMana = 50;
    
    // Index de la position d'attaque (2 = down)
    private int exitIndex = 2;

    // Référence sur l'interaction
    private IInteractable interactable;

    // Propriété d'accès sur la référence sur l'interaction
    public IInteractable MyInteractable { get => interactable; set => interactable = value; }

    // Positions mini/maxi 
    private Vector3 minPosition, maxPosition;

    // Propriété d'accès à l'argent du joueur
    public int MyGold { get; set; }


    /// <summary>
    /// Start : Surcharge la fonction Start du script Character
    /// </summary>
    protected override void Start()
    {
        // Initialise l'argent du joueur
        MyGold = 25;

        // Initialise la barre de mana
        mana.Initialize(initMana, initMana);

        // Initialise la barre d'XP
        xp.Initialize(0, GetXPMax());

        // Actualise le texte du niveau du joueur
        RefreshPlayerLevelText();

        // Appelle Start sur la classe mère (abstraite)
        base.Start();
    }

    /// <summary>
    /// Update : Surcharge la fonction Update du script Character
    /// </summary>
    protected override void Update()
    {
        // Vérifie les interactions
        GetInput();

        // Position du joueur
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x), Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y), transform.position.z);

        // Appelle Update sur la classe mère (abstraite)
        base.Update();
    }

    /// <summary>
    /// Interactions du joueur
    /// </summary>
    private void GetInput()
    {
        MyDirection = Vector2.zero;

        // [DEBUG]
        // [I] : Vie/Mana --
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;

            CombatTextManager.MyInstance.CreateText(transform.position, "10", CombatTextType.Damage, false);
        }
        // [O] : Vie/Mana ++
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;

            CombatTextManager.MyInstance.CreateText(transform.position, "10", CombatTextType.Heal, true);
        }
        // [X] : Experience ++
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(12);
        }

        // Déplacement en Haut
        if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["UP"]) || Input.GetKey(KeyCode.UpArrow))
        {
            exitIndex = 0;
            MyDirection += Vector2.up;
        }

        // Déplacement en Bas
        if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["DOWN"]) || Input.GetKey(KeyCode.DownArrow))
        {
            exitIndex = 2;
            MyDirection += Vector2.down;
        }

        // Déplacement à Gauche
        if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["LEFT"]) || Input.GetKey(KeyCode.LeftArrow))
        {
            exitIndex = 3;
            MyDirection += Vector2.left;
        }

        // Déplacement à Droite
        if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["RIGHT"]) || Input.GetKey(KeyCode.RightArrow))
        {
            exitIndex = 1;
            MyDirection += Vector2.right;
        }

        // Stoppe l'attaque s'il y a un déplacement
        if (IsMoving)
        {
            StopAttack();
        }

        // Boutons d'actions
        foreach (string action in KeyBindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.MyInstance.ActionBinds[action]))
            {
                // Déclenchement de l'action du bouton
                UIManager.MyInstance.ClickActionButton(action);
            }
        }

    }

    /// <summary>
    /// Définit les limites du joueur
    /// </summary>
    /// <param name="min">Position du joueur minimum (en bas à gauche)</param>
    /// <param name="max">Position du joueur maximum (en haut à droite)</param>
    public void SetLimits(Vector3 min, Vector3 max)
    {
        minPosition = min;
        maxPosition = max;
    }

    /// <summary>
    /// Routine d'attaque
    /// </summary>
    /// <param name="spellName">Nom du sort</param>
    private IEnumerator Attack(string spellName)
    {
        // La cible de l'attaque est la cible sélectionnée
        Transform attackTarget = MyTarget;

        // Récupére un sort avec ses propriétes depuis la bibliothèque des sorts 
        Spell mySpell = SpellBook.MyInstance.CastSpell(spellName);

        // Indique que l'on attaque
        IsAttacking = true;

        // Lance l'animation d'attaque
        MyAnimator.SetBool("attack", IsAttacking);

        // Pour chaque emplacement de l'equipement sur le personnage
        foreach (GearSocket socket in gearSockets)
        {
            // Lance l'animation d'attaque
            socket.MyAnimator.SetBool("attack", IsAttacking);
        }

        // Simule le temps d'incantation
        yield return new WaitForSeconds(mySpell.MyCastTime);

        // Vérifie que la cible de l'attaque est toujours attaquable 
        if (attackTarget != null && InLineOfSight())
        {
            // Instantie le sort
            SpellScript spell = Instantiate(mySpell.MyPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            // Affecte la cible et les dégâts du sort
            spell.Initialize(attackTarget, mySpell.MyDamage, transform);
        }

        // Termine l'attaque
        StopAttack();
    }

    /// <summary>
    /// Incante un sort
    /// </summary>
    /// <param name="spellName">Nom du sort</param>
    public void CastSpell(string spellName)
    {
        // Actualise les blocs
        Block();

        // Vérifie si l'on peut attaquer
        if (MyTarget != null && !IsAttacking && !IsMoving && InLineOfSight() && MyTarget.GetComponentInParent<Character>().IsAlive)
        {
            // Démarre la routine d'attaque
            attackRoutine = StartCoroutine(Attack(spellName));
        }
    }

    /// <summary>
    /// Verifie si la cible est dans la ligne de mire
    /// </summary>
    private bool InLineOfSight()
    {
        // Si le joueur a une cible
        if (MyTarget != null)
        {
            // Calcule la direction de la cible
            Vector2 targetDirection = (MyTarget.position - transform.position).normalized;

            // Lance un raycast dans la direction de la cible  [Debug.DrawRay(transform.position, targetDirection, Color.red);]
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), LayerMask.GetMask("Block"));

            // S'il n'y a pas de collision, on peut lancer le sort
            if (hit.collider == null)
            {
                return true;
            }
        }

        // En cas de collision, on ne peut pas lancer le sort
        return false;
    }

    /// <summary>
    /// Change la paire de blocs en fonction de la direction du joueur
    /// </summary>
    private void Block()
    {
        foreach (Block bloc in blocks)
	    {
            // Désactive toutes les paires blocs
            bloc.Deactivate();
	    }

        // Active la paire de bloc correspondante à la direction du joueur
        blocks[exitIndex].Activate();
    }

    /// <summary>
    /// Fin de l'attaque
    /// </summary>
    public void StopAttack()
    {
        // Stoppe l'incantation du sort
        SpellBook.MyInstance.StopCasting();

        // Indique que l'on n'attaque pas
        IsAttacking = false;

        // Arrête l'animation d'attaque
        MyAnimator.SetBool("attack", IsAttacking);

        // Pour chaque emplacement de l'equipement sur le personnage
        foreach (GearSocket socket in gearSockets)
        {
            // Arrête l'animation d'attaque
            socket.MyAnimator.SetBool("attack", IsAttacking);
        }

        // Vérifie qu'il existe une référence à la routine d'attaque
        if (attackRoutine != null)
        {
            // Arrête la routine d'attaque
            StopCoroutine(attackRoutine);

            // Réinitialise la routine d'attaque
            attackRoutine = null;
        }
    }

    /// <summary>
    /// Update : Surcharge la fonction HandleLayers du script Character
    /// </summary>
    public override void HandleLayers()
    {
        // Appelle HandleLayers sur la classe mère (abstraite)
        base.HandleLayers();

        // Si le joueur est en mouvement
        if (IsMoving)
        {
            // Pour chaque emplacement de l'equipement sur le personnage
            foreach (GearSocket socket in gearSockets)
            {
                // Actualise les paramètres de l'animation
                socket.SetDirection(MyDirection.x, MyDirection.y);
            }
        }
    }

    /// <summary>
    /// Active un Layer d'animation (Idle/Walk/Attack) : Surcharge la fonction ActivateLayer du script Character
    /// </summary>
    /// <param name="layerName">Nom du layer à activer</param>
    public override void ActivateLayer(string layerName)
    {
        // Appelle ActivateLayer sur la classe mère (abstraite)
        base.ActivateLayer(layerName);

        // Pour chaque emplacement de l'equipement sur le personnage
        foreach (GearSocket socket in gearSockets)
        {
            // Active le layer d'animation
            socket.ActivateLayer(layerName);
        }
    }

    /// <summary>
    /// Interaction du personnage : Surcharge la fonction Interact du script NPC
    /// </summary>
    public void Interact()
    {
        // S'il y a une interaction du joueur
        if (MyInteractable != null)
        {
            // Début de l'interaction  
            MyInteractable.Interact();
        }
    }

    /// <summary>
    /// Gain d'expérience
    /// </summary>
    /// <param name="amountXp">Expérience gagnée</param>
    public void GainXP(int amountXp)
    {
        // Ajoute l'expérience au personnage
        xp.MyCurrentValue += amountXp;

        // Affiche un message
        CombatTextManager.MyInstance.CreateText(transform.position, amountXp.ToString(), CombatTextType.Experience, false);

        // Si la barre d'expérience est remplie
        if (xp.MyCurrentValue >= xp.MyMaxValue)
        {
            // Démarre la routine de gain de niveau
            StartCoroutine(LevelUp());
        }
    }

    /// <summary>
    /// Gain de niveau
    /// </summary>
    private IEnumerator LevelUp()
    {
        // Tant que la barre d'experience n'est pas remplie
        while (!xp.IsFull)
        {
            yield return null;
        }

        // Incrémente le niveau du joueur
        MyLevel++;

        //Actualise l'affichage du niveau
        RefreshPlayerLevelText();

        // Actualise l'expérience à obtenir pour le niveau
        xp.MyMaxValue = GetXPMax();

        // Actualise la valeur courant e du niveau
        xp.MyCurrentValue = xp.MyOverflow;

        // Réinitialise le remplissage de la barre
        xp.ResetBar();
    }

    /// <summary>
    /// Retourne le montant d'expérience à obtenir pour le niveau
    /// </summary>
    private float GetXPMax()
    {
        return Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f));
    }

    /// <summary>
    /// Actualise le texte du niveau du joueur
    /// </summary>
    private void RefreshPlayerLevelText()
    {
        levelText.text = MyLevel.ToString();
    }

    /// <summary>
    /// Détection de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le joueur entre en contact avec un ennemi
        if (collision.CompareTag("Enemy") || collision.CompareTag("Interactable"))
        {
            // Interaction avec l'ennemi
            MyInteractable = collision.GetComponent<IInteractable>();
        }
    }

    /// <summary>
    /// Détection de fin de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        // Si le joueur n'est plus en contact avec un ennemi
        if (collision.CompareTag("Enemy") || collision.CompareTag("Interactable"))
        {
            // S'il y a une interaction du joueur
            if (MyInteractable != null)
            {
                // Fin de l'interaction avec l'ennemi
                MyInteractable.StopInteract();

                // Réinitialise l'interaction
                MyInteractable = null;
            }
        }
    }
}