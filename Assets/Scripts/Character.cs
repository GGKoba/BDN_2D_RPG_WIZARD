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
    private float speed;

    // Référence sur l'animator
    private Animator animator;

    // Référence sur le rigibody
    private Rigidbody2D rigibody;

    // Direction du personnage
    protected Vector2 direction;

    // Indique si le personnage est en déplacement ou non
    public bool isMoving
    {
        get { return direction.x != 0 || direction.y != 0; }
        set {}
    }



    /// <summary>
    /// Start
    /// </summary>
    protected virtual void Start()
    {
        // Référence sur l'animator du personnage
        animator = GetComponent<Animator>();

        // Référence sur le rigibody du personnage
        rigibody = GetComponent<Rigidbody2D>();
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
    /// Mouvement du personnage
    /// </summary>
    public void Move()
    {
        // Mouvement du personnage
        // transform.Translate(direction * speed * Time.deltaTime);
        rigibody.velocity = direction.normalized * speed;
    }

    /// <summary>
    /// Utilise le layer d'animation adapté
    /// </summary>
    public void HandleLayers()
    {
        // Vérifie si le personnage bouge ou pas. S'il bouge, alors il faut jouer l'animation
        if (isMoving)
        {
            // Utilise le layer "WALK"
            ActivateLayer("WalkLayer");

            // Renseigne les paramètres de l'animation : le personnage s'oriente dans la bonne direction
            animator.SetFloat("x", direction.x);
            animator.SetFloat("y", direction.y);
        }
        else
        {
            // Utilise le layer "IDLE" s'il n'y a plus de mouvement
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


}
