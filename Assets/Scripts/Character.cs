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
    
    // Direction du personnage
    protected Vector2 direction;


    /// <summary>
    /// Start
    /// </summary>
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }


    /// <summary>
    /// Update : virtual pour être écrasée pour les sous-classes
    /// </summary>
    protected virtual void Update()
    {
        Move();
    }

    /// <summary>
    /// Mouvement du personnage
    /// </summary>
    public void Move()
    {
        // Mouvement du personnage
        transform.Translate(direction * speed * Time.deltaTime);

        // Vérifie si le personnage bouge ou pas. S'il bouge, alors il faut jouer l'animation
        if (direction.x != 0 || direction.y != 0)
        {
            // Animation du mouvement du personnage
            AnimateMovement();
        }
        else
        {
            // Utilise le layer "IDLE" s'il n'y a plus de mouvement
            animator.SetLayerWeight(1, 0);
        }
    }

    /// <summary>
    /// Animation du mouvement
    /// </summary>
    private void AnimateMovement()
    {
        // Utilise le layer "WALK"
        animator.SetLayerWeight(1, 1);

        // Renseigne les paramètres de l'animation : le personnage s'oriente dans la bonne direction
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }

}
