using System.Collections.Generic;
using UnityEngine;



// Gestion du changement de la valeur de la vie d'un PNJ
public delegate void HealthChanged(float health);

// Gestion de la disparition du personnage
public delegate void CharacterRemoved();


/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux ennemis
/// </summary>
public class Enemy : Character, IInteractable
{
    // Evènement de changement de valeur de la vie
    public event HealthChanged HealthChangedEvent;

    // Evènement de disparition du personnage
    public event CharacterRemoved CharacterRemovedEvent;

    // Canvas de la barre de vie
    [SerializeField]
    private CanvasGroup healthGroup = default;

    // Etat courant de l'ennemi
    private IState currentState;

    // Propriété d'accès à la position de départ de l'ennemi
    public Vector3 MyStartPosition { get; set; }

    // Propriété d'accès au temps d'attaque de l'ennemi
    public float MyAttackTime { get; set; }

    // Propriété d'accès à la portée d'attaque de l'ennemi
    public float MyAttackRange { get; set; }

    // Propriété d'accès à la portée d'aggro de l'ennemi
    public float MyAggroRange { get; set; }

    // Portée d'aggro de l'ennemi
    [SerializeField]
    private float initialAggroRange = default;

    // Propriété d'accès à la portée de l'ennemi
    public bool InRange { get => Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange; }

    // Table des butins de l'ennemi
    [SerializeField]
    private LootTable lootTable = default;

    // Image de la cible
    [SerializeField]
    private Sprite portrait = default;

    // Propriété d'accès à l'image de la cible
    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }


    /// <summary>
    /// Awake
    /// </summary>
    protected void Awake()
    {
        // Initialise les barres
        health.Initialize(initHealth, initHealth);

        // Initialisation de la position de départ de l'ennemi
        MyStartPosition = transform.position;

        // Initialisation de la portée d'attaque de l'ennemi
        MyAttackRange = 1.39f;

        // Initialisation de la portée d'aggro de l'ennemi
        MyAggroRange = initialAggroRange;

        // Passage à l'état d'attente
        ChangeState(new IdleState());
    }
    
    /// <summary>
    /// Start : Surcharge la fonction Start du script NPC
    /// </summary>
    protected override void Start()
    {
        // Référence sur CanvasGroup de la barre de cast du joueur
        healthGroup.alpha = 0;

        // Appelle Start sur la classe mère
        base.Start();
    }

    /// <summary>
    /// Update : Surcharge la fonction Update du script NPC
    /// </summary>
    protected override void Update()
    {
        // Si l'ennemi est en vie
        if (IsAlive)
        {
            // S'il n'y a pas d'attaque
            if (!IsAttacking)
            {
                // Mise à jour du temps depuis la dernière attaque
                MyAttackTime += Time.deltaTime;
            }

            // Appelle Update sur l'état courant
            currentState.Update();
        }

        // Appelle Update sur la classe mère
        base.Update();
    }

    /// <summary>
    /// Désélection d'un ennemi
    /// </summary>
    public void DeSelect()
    {
        // Masque la barre de vie
        healthGroup.alpha = 0;

        // Désabonnement de l'évènement de changement de vie
        HealthChangedEvent -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);

        // Désabonnement de l'évènement de disparition du personnage
        CharacterRemovedEvent -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
    }

    /// <summary>
    /// Sélection d'un ennemi
    /// </summary>
    public Transform Select()
    {
        // Affiche la barre de vie
        healthGroup.alpha = 1;

        // Appelle Select sur la classe mère
        return hitBox;
    }

    /// <summary>
    /// Dégâts liée à une attaque : Surcharge la fonction TakeDamage du script Character
    /// </summary>
    /// <param name="damage">Montant des dégâts</param>
    /// <param name="source">Source de l'attaque</param>
    public override void TakeDamage(float damage, Transform source)
    {
        // Si l'état n'est pas en évasion
        if (!(currentState is EvadeState))
        {
            // Si le personnage est en vie
            if (IsAlive)
            {
                // Définit la cible et actualise la portée d'aggro
                SetTarget(source);

                // Appelle TakeDamage sur la classe mère
                base.TakeDamage(damage, source);

                // Déclenche l'évènement de changement de la vie
                OnHealthChanged(health.MyCurrentValue);

                // Si le personnage n'est plus en vie
                if (!IsAlive)
                {
                    // Retire l'ennemi de la liste des attaquants
                    Player.MyInstance.MyAttackers.Remove(this);

                    // Gagne l'expérience calculée
                    Player.MyInstance.GainXP(XPManager.CalculateXP(this));
                }
            }
        }
    }

    /// <summary>
    /// Change l'état de l'ennemi
    /// </summary>
    /// <param name="state">Etat de l'ennemi</param>
    public void ChangeState(IState state)
    {
        // S'il y a un état;
        if (currentState != null)
        {
            // Sortie de l'ancien état
            currentState.Exit();
        }

        // Mise à jour de l'état courant
        currentState = state;

        // Entrée dans le nouvel état
        currentState.Enter(this);
    }

    /// <summary>
    /// Définit la cible
    /// </summary>
    /// <param name="target">Cible</param>
    public void SetTarget(Transform sourceTarget)
    {
        // S'il n'y a pas de cible et que l'état n'est pas en évasion
        if (MyTarget == null && !(currentState is EvadeState))
        {
            // Calcul de la distance entre l'ennemi et la cible
            float distance = Vector2.Distance(transform.position, sourceTarget.position);

            // Réinitialise la portée d'aggro de l'ennemi
            MyAggroRange = initialAggroRange;

            // Ajoute la distance entre l'ennemi et la cible à la portée d'aggro de l'ennemi
            MyAggroRange += distance;

            // La source de l'attaque devient la cible
            MyTarget = sourceTarget;
        }
    }

    /// <summary>
    /// Réinitialise les données de l'ennemi
    /// </summary>
    public void Reset()
    {
        // Réinitialise la cible
        MyTarget = null;

        // Réinitialise la portée d'aggro de l'ennemi
        MyAggroRange = initialAggroRange;

        // Réinitialise la vie de l'ennemi
        MyHealth.MyCurrentValue = MyHealth.MyMaxValue;

        // Déclenche l'évènement de changement de la vie
        OnHealthChanged(health.MyCurrentValue);
    }

    /// <summary>
    /// Interaction avec le personnage
    /// </summary>
    public void Interact()
    {
        // Si l'ennemi n'est pas en vie
        if (!IsAlive)
        {
            // Liste des butins
            List<Drop> drops = new List<Drop>();

            // Pour toutes les entités en interaction
            foreach (IInteractable interactable in Player.MyInstance.MyInteractables)
            {
                // Si c'est un ennemi et qu'il n'est plus en vie
                if (interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    // Ajoute la liste des butins de l'ennemi
                    drops.AddRange((interactable as Enemy).lootTable.GetLoots());
                }
            }

            // Actualise les pages des butins
            LootWindow.MyInstance.CreatePages(drops);
        }
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage
    /// </summary>
    public void StopInteract()
    {
        // Ferme la fenêtre des butins
        LootWindow.MyInstance.Close();
    }

    /// <summary>
    /// Appelle l'évènement de changement de valeur de la vie
    /// </summary>
    /// <param name="health">Vie actuelle</param>
    public void OnHealthChanged(float health)
    {
        // S'il y a un abonnement à cet évènement
        if (HealthChangedEvent != null)
        {
            // Déclenchement de l'évènement de changement de la valeur de la vie
            HealthChangedEvent.Invoke(health);
        }
    }

    /// <summary>
    /// Appelle l'évènement de disparition du personnage
    /// </summary>
    public void OnCharacterRemoved()
    {
        // S'il y a un abonnement à cet évènement
        if (CharacterRemovedEvent != null)
        {
            // Déclenchement de l'évènement de disparition du personnage
            CharacterRemovedEvent.Invoke();
        }

        // Destruction du personnage
        Destroy(gameObject);
    }
}