using System.Collections;
using UnityEngine;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques au joueur
/// </summary>
public class Player : Character
{
    // Mana du joueur
    [SerializeField]
    private Stat mana = default;

    // Tableau des positions pour lancer les sorts
    [SerializeField]
    private Transform[] exitPoints = default;

    // Tableau des paires de blocs pour bloquer la ligne de mire
    [SerializeField]
    private Block[] blocks = default;

    // Mana initiale du joueur (readonly)
    private readonly float initMana = 50;
    
    // Index de la position d'attaque (2 = down)
    private int exitIndex = 2;

    // Bibliothèque des sorts
    private SpellBook spellBook;

    // Cible du joueur
    public Transform MyTarget { get; set; }

    // Positions mini/maxi 
    private Vector3 minPosition, maxPosition;


    /// <summary>
    /// Start : Surcharge la fonction Start du script Character
    /// </summary>
    protected override void Start()
    {
        // Initialise les barres
        mana.Initialize(initMana, initMana);

        // Référence sur la bibliothèque des sorts
        spellBook = gameObject.GetComponent<SpellBook>();

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

        // [DEBUG] : Test Vie/Mana
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }


        // Déplacement en Haut
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            exitIndex = 0;
            MyDirection += Vector2.up;
        }

        // Déplacement en Bas
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            exitIndex = 2;
            MyDirection += Vector2.down;
        }

        // Déplacement à Gauche
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            exitIndex = 3;
            MyDirection += Vector2.left;
        }

        // Déplacement à Droite
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            exitIndex = 1;
            MyDirection += Vector2.right;
        }


        // Stoppe l'attaque s'il y a un déplacement
        if (IsMoving)
        {
            StopAttack();
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
    /// <param name="spellIndex">Index du sort</param>
    private IEnumerator Attack(int spellIndex)
    {
        // La cible de l'attaque est la cible sélectionnée
        Transform attackTarget = MyTarget;

        // Récupére un sort avec ses propriétes depuis la bibliothèque des sorts 
        Spell mySpell = spellBook.CastSpell(spellIndex);

        // Indique que l'on attaque
        isAttacking = true;

        // Lance l'animation d'attaque
        animator.SetBool("attack", isAttacking);

        // Simule le temps d'incantation
        yield return new WaitForSeconds(mySpell.SpellCastTime);

        // Vérifie que la cible de l'attaque est toujours attaquable 
        if (attackTarget != null && InLineOfSight())
        {
            // Instantie le sort
            SpellManager spell = Instantiate(mySpell.SpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellManager>();

            // Affecte la cible et les dégâts du sort
            spell.Initialize(attackTarget, mySpell.SpellDamage);
        }

        // Termine l'attaque
        StopAttack();
    }

    /// <summary>
    /// Incante un sort
    /// </summary>
    /// <param name="spellIndex">Index du sort</param>
    public void CastSpell(int spellIndex)
    {
        // Actualise les blocs
        Block();

        // Vérifie si l'on peut attaquer
        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
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
        spellBook.StopCasting();

        // Indique que l'on n'attaque pas
        isAttacking = false;

        // Arrête l'animation d'attaque
        animator.SetBool("attack", isAttacking);

        // Vérifie qu'il existe une référence à la routine d'attaque
        if (attackRoutine != null)
        {
            // Arrête la routine d'attaque
            StopCoroutine(attackRoutine);

            // Reset la routine d'attaque
            attackRoutine = null;
        }
    }
}