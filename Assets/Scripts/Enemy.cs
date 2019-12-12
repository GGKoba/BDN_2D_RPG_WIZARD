using UnityEngine;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux ennemis
/// </summary>
public class Enemy : NPC
{
    // Canvas de la barre de vie
    [SerializeField]
    private CanvasGroup healthGroup = default;

    // Etat courant de l'ennemi
    private IState currentState;

    // Propriété d'accès au temps d'attaque de l'ennemi
    public float MyAttackTime { get; set; }

    // Propriété d'accès à la portée d'attaque de l'ennemi
    public float MyAttackRange { get; set; }

    // Propriété d'accès à la portée d'aggro de l'ennemi
    public float MyAggroRange { get; set; }

    // Portée d'aggro de l'ennemi
    [SerializeField]
    private float initialAggroRange;

    // Cible à portée ?
    public bool InRange { get => Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange; }



    /// <summary>
    /// Awake
    /// </summary>
    protected void Awake()
    {
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
        // Si le personnage est en vie
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
    /// Désélection d'un ennemi : Surcharge la fonction Select du script NPC
    /// </summary>
    public override void DeSelect()
    {
        // Masque la barre de vie
        healthGroup.alpha = 0;

        // Appelle DeSelect sur la classe mère
        base.DeSelect();    
    }

    /// <summary>
    /// Sélection d'un ennemi : Surcharge la fonction Select du script NPC
    /// </summary>
    public override Transform Select()
    {
        // Affiche la barre de vie
        healthGroup.alpha = 1;

        // Appelle Select sur la classe mère
        return base.Select();
    }

    /// <summary>
    /// Dégâts liée à une attaque : Surcharge la fonction TakeDamage du script Character
    /// </summary>
    /// <param name="damage">Montant des dégâts</param>
    /// <param name="source">Source de l'attaque</param>
    public override void TakeDamage(float damage, Transform source)
    {
        // Définit la cible et actualise la portée d'aggro
        SetTarget(source);

        // Appelle TakeDamage sur la classe mère
        base.TakeDamage(damage, source);

        // Déclenche l'évènement de changement de la vie
        OnHealthChanged(health.MyCurrentValue);
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
        // S'il n'y a pas de cible
        if (MyTarget == null)
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
}