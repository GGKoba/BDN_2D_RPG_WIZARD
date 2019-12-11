using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les personnages héritent
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour
{
    // Référence sur le rigidbody
    private Rigidbody2D myRigidbody;

    // Vitesse de déplacement
    [SerializeField]
    private float speed = default;

    // Propriété d'accès à la vitesse du personnage
    public float MySpeed { get => speed; set => speed = value; }

    // Vie initiale du personnage (readonly)
    [SerializeField]
    private float initHealth = default;

    [SerializeField]
    // Référence sur la vie du personnage
    protected Stat health = default;

    // Propriété d'accès à la vie du personnage
    public Stat MyHealth { get => health; }

    // Référence sur la hitBox du personnage
    [SerializeField]
    protected Transform hitBox = default;

    // Direction du personnage
    private Vector2 direction;

    // Propriété d'accès à la direction du personnage
    public Vector2 MyDirection { get => direction; set => direction = value; }

    // Référence sur l'animator
    public Animator MyAnimator { get; set; }

    // Référence sur la routine d'attaque
    protected Coroutine attackRoutine;
    
    // Indique si le personnage attaque ou non
    public bool IsAttacking { get; set; }

    // Indique si le personnage est en déplacement ou non
    public bool IsMoving
    {
        get { return direction.x != 0 || direction.y != 0; }
    }


    /// <summary>
    /// Start
    /// </summary>
    protected virtual void Start()
    {
        // Initialise les barres
        health.Initialize(initHealth, initHealth);

        // Référence sur l'animator du personnage
        MyAnimator = GetComponent<Animator>();

        // Référence sur le rigidbody du personnage
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update : virtual pour être écrasée pour les sous-classes
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
    /// Déplacement du personnage
    /// </summary>
    public void Move()
    {
        // Déplace le personnage
        myRigidbody.velocity = direction.normalized * speed;
    }

    /// <summary>
    /// Utilise le layer d'animation adapté
    /// </summary>
    public void HandleLayers()
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

    /// <summary>
    /// Active un Layer d'animation (Idle/Walk/Attack)
    /// </summary>
    /// <param name="layerName">Nom du layer à activer</param>
    public void ActivateLayer(string layerName)
    {
        // Boucle sur les layers d'animations
        for (int i = 0; i < MyAnimator.layerCount; i++)
		{
            // Reset le layer courant
            MyAnimator.SetLayerWeight(i, 0);
		}

        // Active le layer correspond au nom passé en paramètre
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    /// <summary>
    /// Dégâts liée à une attaque
    /// </summary>
    /// <param name="damage">Montant des dégâts</param>
    public virtual void TakeDamage(float damage)
    {
        // Réduction de la vie du personnage
        health.MyCurrentValue -= damage;
          
        // Si le personnage n'a plus de vie
        if (health.MyCurrentValue <= 0)
        {
            // Activation du trigger "die"
            MyAnimator.SetTrigger("die");
        }
    }
}