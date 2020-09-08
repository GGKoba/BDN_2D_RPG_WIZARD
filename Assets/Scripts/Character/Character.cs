using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les personnages héritent
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    // Référence sur le rigidbody
    [SerializeField]
    private Rigidbody2D rigidbodyCharacter;

    // Vitesse de déplacement
    [SerializeField]
    private float speed = default;

    // Propriété d'accès à la vitesse du personnage
    public float MySpeed { get => speed; set => speed = value; }

    // Type du personnage
    [SerializeField]
    private string type = default;

    // Propriété d'accès à la vitesse du personnage
    public string MyType { get => type; }

    // Vie initiale du personnage (readonly)
    [SerializeField]
    protected float initHealth = default;

    [SerializeField]
    // Référence sur la vie du personnage
    protected Stat health = default;

    // Propriété d'accès à la vie du personnage
    public Stat MyHealth { get => health; set => health = value; }

    [SerializeField]
    // Référence sur le niveau du personnage
    private int level;

    // Propriété d'accès au niveau du personnage
    public int MyLevel { get => level; set => level = value; }

    // Référence sur la hitBox du personnage
    [SerializeField]
    protected Transform hitBox = default;

    // Direction du personnage
    private Vector2 direction;

    // Propriété d'accès à la direction du personnage
    public Vector2 MyDirection { get => direction; set => direction = value; }

    // Propriété d'accès sur l'animator
    public Animator MyAnimator { get; set; }

    // Propriété d'accès à la cible de du personnage
    public Transform MyTarget { get; set; }

    // Référence sur la routine d'action
    protected Coroutine actionRoutine;

    // Propriété d'accès sur l'indicateur d'attaque du personnage
    public bool IsAttacking { get; set; }

    // Propriété d'accès sur l'indicateur de déplacement du personnage
    public bool IsMoving { get => direction.x != 0 || direction.y != 0; }

    // Propriété d'accès sur la vie du personnage
    public bool IsAlive { get => health.MyCurrentValue > 0; }



    /// <summary>
    /// Start
    /// </summary>
    protected virtual void Start()
    {
        // Référence sur l'animator du personnage
        MyAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update : virtual pour être écrasée pour les autres classes
    /// </summary>
    protected virtual void Update()
    {
        HandleLayers();
    }

    /// <summary>
    /// FixedUpdate : Update utilisé pour le Rigibody
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Déplacement du personnage : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual void Move()
    {
        // Si le personnage est en vie
        if (IsAlive)
        {
            // Déplace le personnage
            rigidbodyCharacter.velocity = direction.normalized * speed;
        }
    }

    /// <summary>
    /// Utilise le layer d'animation adapté : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual void HandleLayers()
    {
        // Si le personnage est en vie
        if (IsAlive)
        {
            // Vérifie si le personnage bouge ou pas. S'il bouge, alors il faut jouer l'animation
            if (IsMoving)
            {
                // Utilise le layer "WALK"
                ActivateLayer("WalkLayer");

                // Renseigne les paramètres de l'animation : le personnage s'oriente dans la bonne direction
                MyAnimator.SetFloat("x", direction.x);
                MyAnimator.SetFloat("y", direction.y);
            }
            else if (IsAttacking)
            {
                // Utilise le layer "ATTACK"
                ActivateLayer("AttackLayer");
            }
            else
            {
                // Utilise le layer "IDLE" s'il n'y a plus de mouvement ou d'attaque
                ActivateLayer("IdleLayer");
            }
        }
        else
        {
            // Utilise le layer "DEATH" si le personnage n'a plus de vie
            ActivateLayer("DeathLayer");
        }

    }

    /// <summary>
    /// Active un layer d'animation (Idle/Walk/Attack) : virtual pour être écrasée pour les autres classes
    /// </summary>
    /// <param name="layerName">Nom du layer à activer</param>
    public virtual void ActivateLayer(string layerName)
    {
        // Boucle sur les layers d'animations
        for (int i = 0; i < MyAnimator.layerCount; i++)
		{
            // Réinitialise le layer courant
            MyAnimator.SetLayerWeight(i, 0);
		}

        // Active le layer correspond au nom passé en paramètre
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    /// <summary>
    /// Dégâts liée à une attaque
    /// </summary>
    /// <param name="damage">Montant des dégâts</param>
    /// <param name="source">Source de l'attaque</param>
    public virtual void TakeDamage(float damage, Transform source)
    {
        // Réduction de la vie du personnage
        health.MyCurrentValue -= damage;

        // Affiche un texte
        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), CombatTextType.Damage, false);

        // Si le personnage n'a plus de vie
        if (health.MyCurrentValue <= 0)
        {
            // Réinitialise la direction
            direction = Vector2.zero;

            // Stoppe le déplacement
            rigidbodyCharacter.velocity = direction;

            // Déclenche l'évènement de disparition du personnage
            GameManager.MyInstance.OnKillConfirmed(this);

            // Activation du trigger "die"
            MyAnimator.SetTrigger("die");
        }
    }

    /// <summary>
    /// Accès à la vie du personnage
    /// </summary>
    /// <param name="amount">Points de vie à ajouter au personnage</param>
    public void GetHealth(int amount)
    {
        // Actualise la vie du personnage
        health.MyCurrentValue += amount;

        // Affiche un texte
        CombatTextManager.MyInstance.CreateText(transform.position, amount.ToString(), CombatTextType.Heal, true);
    }
}