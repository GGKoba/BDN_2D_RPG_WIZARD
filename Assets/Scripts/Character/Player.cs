﻿using System.Collections;
using System.Collections.Generic;
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

    // Propriété d'accès sur la mana du joueur
    public Stat MyMana { get => mana; set => mana = value; }

    // Expérience du joueur
    [SerializeField]
    private Stat xp = default;

    // Propriété d'accès sur l'expérience du joueur
    public Stat MyXp { get => xp; set => xp = value; }

    // Texte du niveau du joueur
    [SerializeField]
    private Text levelText = default;

    // Référence sur l'animator de levelUp
    [SerializeField]
    private Animator ding = default;

    // Indicateur sur la minimap
    [SerializeField]
    private Transform minimapIcon = default;

    // Tableau des positions pour lancer les sorts
    [SerializeField]
    private Transform[] exitPoints = default;

    // Tableau des paires de blocs pour bloquer la ligne de mire
    [SerializeField]
    private Block[] blocks = default;

    // Référence sur l'emplacement de l'equipement sur le personnage
    [SerializeField]
    private GearSocket[] gearSockets = default;

    // Tableau des objets de l'équipement
    [SerializeField]
    private SpriteRenderer[] gearRenderers = default;

    // Mana initiale du joueur (readonly)
    private readonly float initMana = 50;

    // Index de la position d'attaque (2 = down)
    private int exitIndex = 2;

    // Référence sur l'interaction
    private List<IInteractable> interactables = new List<IInteractable>();

    // Propriété d'accès sur la référence sur l'interaction
    public List<IInteractable> MyInteractables { get => interactables; set => interactables = value; }

    // Positions mini/maxi 
    private Vector3 minPosition, maxPosition;

    #region PathFinding
    // Script de Pathfinding
    [SerializeField]
    private AStar astar = default;

    // Objectif
    private Vector3 goal;

    // Destination
    private Vector3 destination;

    // Noeud courant
    private Vector3 currentNode;
    #endregion

    // Propriété d'accès sur la routine
    public Coroutine MyRoutine { get; set; }

    // Propriété d'accès à l'argent du joueur
    public int MyGold { get; set; }

    // Liste des attaquants
    private List<Enemy> attackers = new List<Enemy>();

    // Propriété d'accès à la liste des attaquants
    public List<Enemy> MyAttackers { get => attackers; set => attackers = value; }

    // Position de départ du joueur
    private Vector2 initPosition;


    /// <summary>
    /// Update : Surcharge la fonction Update du script Character
    /// </summary>
    protected override void Update()
    {
        // Vérifie les interactions
        GetInput();

        // Initialise de déplacement au clic
        ClickToMove();

        // Position du joueur
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x), Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y), transform.position.z);

        // Appelle Update sur la classe mère (abstraite)
        base.Update();
    }

    /// <summary>
    /// Initialise les informations du joueur
    /// </summary>
    public void SetPlayerDefaultValues()
    {
        // Initialise l'argent du joueur
        MyGold = 25;

        // Initialise la barre de vie
        health.Initialize(initHealth, initHealth);

        // Initialise la barre de mana
        mana.Initialize(initMana, initMana);

        // Initialise la barre d'XP
        xp.Initialize(0, GetXPMax());

        // Actualise le texte du niveau du joueur
        RefreshPlayerLevelText();

        // Initialise la position de départ du joueur
        initPosition = transform.parent.position;
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
            minimapIcon.eulerAngles = new Vector3(0, 0, 0);
        }

        // Déplacement en Bas
        if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["DOWN"]) || Input.GetKey(KeyCode.DownArrow))
        {
            exitIndex = 2;
            MyDirection += Vector2.down;
            minimapIcon.eulerAngles = new Vector3(0, 0, 180);
        }

        // Déplacement à Gauche
        if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["LEFT"]) || Input.GetKey(KeyCode.LeftArrow))
        {
            exitIndex = 3;
            MyDirection += Vector2.left;
            if (MyDirection.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 90);
            }
        }

        // Déplacement à Droite
        if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["RIGHT"]) || Input.GetKey(KeyCode.RightArrow))
        {
            exitIndex = 1;
            MyDirection += Vector2.right;
            if (MyDirection.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 270);
            }
        }

        // Stoppe l'attaque s'il y a un déplacement
        if (IsMoving)
        {
            // Stoppe les routines
            StopActionRoutine();
            StopRoutine();
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
    /// Incante un sort
    /// </summary>
    /// <param name="castable">Element incantable</param>
    public void CastSpell(ICastable castable)
    {
        // Actualise les blocs
        Block();

        // Vérifie si l'on peut attaquer
        if (MyTarget != null && !IsAttacking && !IsMoving && InLineOfSight() && MyTarget.GetComponentInParent<Character>().IsAlive && InRange(castable as Spell, MyTarget.transform.position))
        {
            // Démarre la routine
            MyRoutine = StartCoroutine(CastActionRoutine(castable));
        }
    }

    /// <summary>
    /// Récolte des items
    /// </summary>
    /// <param name="castable">Element incantable</param>
    /// <param name="items">Liste des butins</param>
    public void Gather(ICastable castable, List<Drop> items)
    {
        // Vérifie si l'on peut récolter
        if (!IsAttacking)
        {
            // Démarre la routine
            MyRoutine = StartCoroutine(GatherActionRoutine(castable, items));
        }
    }

    /// <summary>
    /// Routine d'attaque
    /// </summary>
    /// <param name="castable">Element incantable</param>
    private IEnumerator CastActionRoutine(ICastable castable)
    {
        // La cible de l'attaque est la cible sélectionnée
        Transform currentTarget = MyTarget.MyHitBox;

        // Routine d'action
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        // Vérifie que la cible de l'attaque est toujours attaquable 
        if (currentTarget != null && InLineOfSight())
        {
            // Récupère les informations de l'élément incantable
            Spell spell = SpellBook.MyInstance.GetSpell(castable.MyTitle);

            // Instantie le sort
            SpellScript spellScript = Instantiate(spell.MyPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            // Initialiase le sort (cible, dégats, source, débuff)
            spellScript.Initialize(currentTarget, spell.MyDamage, this, spell.MyDebuff);
        }
    }

    /// <summary>
    /// Routine de récolte
    /// </summary>
    /// <param name="castable">Element incantable</param>
    /// <param name="items">Liste des butins</param>
    private IEnumerator GatherActionRoutine(ICastable castable, List<Drop> items)
    {
        // Routine d'action
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        // Création de la liste des pages de butin
        LootWindow.MyInstance.CreatePages(items);
    }

    /// <summary>
    /// Routine de fabrication
    /// </summary>
    /// <param name="castable">Element incantable</param>
    public IEnumerator CraftActionRoutine(ICastable castable)
    {
        // Routine d'action
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        // Ajoute l'item dans l'inventaire
        Craft.MyInstance.AddItemsToInventory();
    }

    /// <summary>
    /// Routine d'action
    /// </summary>
    /// <param name="castable">Element incantable</param>
    private IEnumerator ActionRoutine(ICastable castable)
    {
        // Récupére les informations liées à l'action incantable
        SpellBook.MyInstance.Cast(castable);

        // Lance l'attaque
        SetIsAttacking(true);

        // Simule le temps d'incantation
        yield return new WaitForSeconds(castable.MyCastTime);

        // Termine la routine d'action
        StopActionRoutine();
    }

    /// <summary>
    /// Fin de la routine d'action
    /// </summary>
    public void StopActionRoutine()
    {
        // Stoppe l'incantation du sort
        SpellBook.MyInstance.StopCasting();

        // Stoppe l'attaque
        SetIsAttacking(false);

        // Vérifie qu'il existe une référence à la routine d'action
        if (actionRoutine != null)
        {
            // Arrête la routine d'action
            StopCoroutine(actionRoutine);
        }
    }

    /// <summary>
    /// Actualise la propriété d'attaque
    /// </summary>
    /// <param name="isAttacking"></param>
    private void SetIsAttacking(bool isAttacking)
    {
        IsAttacking = isAttacking;

        SetAttackAnimations();
    }

    /// <summary>
    /// Actualise les animations d'attaque
    /// </summary>
    private void SetAttackAnimations()
    {
        MyAnimator.SetBool("attack", IsAttacking);

        // Pour chaque emplacement de l'equipement sur le personnage
        foreach (GearSocket socket in gearSockets)
        {
            socket.MyAnimator.SetBool("attack", IsAttacking);
        }
    }

    /// <summary>
    /// Stoppe la routine
    /// </summary>
    private void StopRoutine()
    {
        // Vérifie qu'il existe une référence à la routine
        if (MyRoutine != null)
        {
            // Arrête la routine
            StopCoroutine(MyRoutine);
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
            Vector2 targetDirection = (MyTarget.transform.position - transform.position).normalized;

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

        //Déclenche l'animation de levelUp
        ding.SetTrigger("Ding");

        //Actualise l'affichage du niveau
        RefreshPlayerLevelText();

        // Actualise l'expérience à obtenir pour le niveau
        xp.MyMaxValue = GetXPMax();

        // Actualise la valeur courant e du niveau
        xp.MyCurrentValue = xp.MyOverflow;

        // Réinitialise le remplissage de la barre
        xp.ResetBar();

        // Si la barre d'expérience est remplie
        if (xp.MyCurrentValue >= xp.MyMaxValue)
        {
            // Démarre la routine de gain de niveau
            StartCoroutine(LevelUp());
        }
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
    public void RefreshPlayerLevelText()
    {
        levelText.text = MyLevel.ToString();
    }

    /// <summary>
    /// Trouve le chemin jusqu'à l'objectif
    /// </summary>
    /// <param name="aGoal"></param>
    public void GetPath(Vector3 aGoal)
    {
        // Récupère le chemin
        MyPath = astar.Algorithm(transform.position, aGoal);

        // Définit le noeud courant (point de départ)
        currentNode = MyPath.Pop();

        // Récupère le 1er point de destination
        destination = MyPath.Pop();

        // Définit le point d'arrivée
        goal = aGoal;
    }

    /// <summary>
    /// Déplacement par clic
    /// </summary>
    private void ClickToMove()
    {
        // S'il y a un chemin
        if (MyPath != null)
        {
            // Déplacement vers la destination
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, MySpeed * Time.deltaTime);

            // Position de la cellule de destination
            Vector3Int dest = astar.MyTilemap.WorldToCell(destination);

            // Position de la cellule de courante
            Vector3Int nodeCurrent = astar.MyTilemap.WorldToCell(currentNode);

            // Distance entre la destination et le joueur
            float distance = Vector2.Distance(destination, transform.parent.position);

            // Direction vers le bas
            if (nodeCurrent.y > dest.y)
            {
                MyDirection = Vector2.down;
            }
            // Direction vers le haut
            else if (nodeCurrent.y < dest.y)
            {
                MyDirection = Vector2.up;
            }

            if (nodeCurrent.y == dest.y)
            {
                // Direction vers la gauche
                if (nodeCurrent.x > dest.x)
                {
                    MyDirection = Vector2.left;
                }
                // Direction vers la droite
                else if (nodeCurrent.x < dest.x)
                {
                    MyDirection = Vector2.right;
                }
            }

            // Si le joueur est sur la destination
            if (distance <= 0f)
            {
                // S'il y a un chemin à faire
                if (MyPath.Count > 0)
                {
                    // Mise à jour du noeud courant
                    currentNode = destination;

                    // Mise à jour de la destination
                    destination = MyPath.Pop();
                }
                else
                {
                    // Réinitialise le chemin
                    MyPath = null;
                }
            }
        }
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
            // Référence sur l'entité en interaction
            IInteractable interactable = collision.GetComponent<IInteractable>();

            // Si c'est une nouvelle entité en interaction
            if (!MyInteractables.Contains(interactable))
            {
                // Ajoute dans la liste l'entité en interaction
                MyInteractables.Add(collision.GetComponent<IInteractable>());
            }
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
            if (MyInteractables.Count > 0)
            {
                // Référence sur l'entité en interaction
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                // S'il y a une interaction
                if (interactable != null)
                {
                    // Fin de l'interaction avec l'ennemi
                    interactable.StopInteract();
                }

                // Supprime de la liste l'entité en interaction
                MyInteractables.Remove(interactable);
            }
        }
    }

    /// <summary>
    /// Ajoute une cible dans la liste des attaquants
    /// </summary>
    /// <param name="enemy">Cible à ajouter</param>
    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }

    /// <summary>
    /// Routine de repop
    /// </summary>
    public IEnumerator Respawn()
    {
        // Désactive le joueur
        MySpriteRenderer.enabled = false;

        // Délai d'attente
        yield return new WaitForSeconds(5f);

        // Initialise la barre de vie
        health.Initialize(initHealth, initHealth);

        // Initialise la barre de mana
        mana.Initialize(initMana, initMana);

        // Replace le joueur
        transform.parent.position = initPosition;

        // Active le joueur
        MySpriteRenderer.enabled = true;

        // Déclenche l'animation de repop
        MyAnimator.SetTrigger("respawn");

        // Affiche l'équipement du joueur
        ShowGear();
    }

    /// <summary>
    /// Masque l'équipement du joueur
    /// </summary>
    public void HideGear()
    {
        // Pour chaque équipement
        foreach (SpriteRenderer item in gearRenderers)
        {
            // Masque l'item
            item.enabled = false;
        }
    }

    /// <summary>
    /// Affiche l'équipement du joueur
    /// </summary>
    public void ShowGear()
    {
        // Pour chaque équipement
        foreach (SpriteRenderer item in gearRenderers)
        {
            // Masque l'item
            item.enabled = true;
        }
    }

    /// <summary>
    /// Définit si la cible est à portée du sort
    /// </summary>
    /// <param name="spell">Sort à incanter</param>
    /// <param name="targetPosition">Position de la cible</param>
    /// <returns></returns>
    private bool InRange(Spell spell, Vector2 targetPosition)
    {
        if (Vector2.Distance(targetPosition, transform.position) <= spell.MyRange)
        {
            // Retourne OK
            return true;
        }

        // Message "Hors de portée"
        MessageFeedManager.MyInstance.WriteMessage("Hors de portée !!", Color.red);

        // Retourne KO
        return false;
    }
}