using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les personnages héritent
/// </summary>
public abstract class Character : MonoBehaviour
{
    // Vitesse de déplacement
    [SerializeField]
    private float speed = default;

    // Préfab des sorts
    [SerializeField]
    private GameObject[] spellPrefab = default;

    // Référence sur le rigidbody
    private Rigidbody2D myRigidbody;

    // Référence sur l'animator
    protected Animator animator;

    // Référence sur la routine d'attaque
    protected Coroutine attackRoutine;

    // Direction du personnage
    protected Vector2 direction;

    // Indique si le personnage attaque ou non
    protected bool isAttacking = false;

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
        // Référence sur l'animator du personnage
        animator = GetComponent<Animator>();

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
            animator.SetFloat("x", direction.x);
            animator.SetFloat("y", direction.y);

            // Stoppe l'attaque s'il y a un déplacent
            StopAttack();

        }
        else if (isAttacking)
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
    /// Active un Layer d'animation (Idle/Walk)
    /// </summary>
    public void ActivateLayer(string layerName)
    {
        // Boucle sur les layers d'animations
        for (int i = 0; i < animator.layerCount; i++)
		{
            // Reset le layer courant
            animator.SetLayerWeight(i, 0);
		}

        // Active le layer correspond au nom passé en paramètre
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }

    /// <summary>
    /// Fin de l'attaque
    /// </summary>
    public void StopAttack()
    {
        // Indique que l'on n'attaque pas
        isAttacking = false;

        // Arrête l'animation d'attaque
        animator.SetBool("attack", isAttacking);

        // Vérifie qu'il existe une référence à la routine d'attaque
        if (attackRoutine != null)
        {
            // Arrête la routine d'attaque
            StopCoroutine(attackRoutine);
        }
    }

    /// <summary>
    /// Incante un sort
    /// </summary>
    public void CastSpell()
    {
        // Instantie le sort
        Instantiate(spellPrefab[0], transform.position, Quaternion.identity);
    }
}