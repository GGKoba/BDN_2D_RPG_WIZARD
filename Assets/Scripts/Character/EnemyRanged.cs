

using UnityEngine;
/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux ennemis à distance
/// </summary>
public class EnemyRanged : Enemy
{
    // Prefab des "fleches"
    [SerializeField]
    private GameObject arrowPrefab = default;

    // Tableau des positions pour tirer
    [SerializeField]
    private Transform[] exitPoints = default;

    // Champ de vision
    private float fieldOfView = 120;

    // Mise à jour de la direction
    private bool updateDirection = false;


    /// <summary>
    /// Update : Surcharge la fonction Update du script Character
    /// </summary>
    protected override void Update()
    {
        // Suivi de la direction de la cible
        LookAtTarget();

        // Appelle Update sur la classe mère
        base.Update();
    }

    /// <summary>
    /// LateUpdate : Est appellée après un UPDATE
    /// </summary>
    private void LateUpdate()
    {
        // Suivi de la direction de la cible
        UpdateDirection();
    }

    /// <summary>
    /// Attaque
    /// </summary>
    /// <param name="exitIndex">Index de la position d'attaque</param>
    public void Shoot(int exitIndex)
    {
        // Instantie la flèche
        SpellScript arrow = Instantiate(arrowPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

        // Affecte la cible et les dégâts de la flèche
        arrow.Initialize(MyTarget.MyHitBox, damage, this);
    }

    /// <summary>
    /// Regarde en direction de la cible
    /// </summary>
    private void LookAtTarget()
    {
        // si j'ai une cible
        if (MyTarget != null)
        {
            // Direction de la cible
            Vector2 directionToTarget = (MyTarget.transform.position - transform.position).normalized;

            // Ma direction
            Vector2 myDirection = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

            // Angle de vision de la cible
            float angleToTarget = Vector2.Angle(myDirection, directionToTarget);

            if (angleToTarget > fieldOfView / 2)
            {
                // Ajuste la direction
                MyAnimator.SetFloat("x", directionToTarget.x);
                MyAnimator.SetFloat("y", directionToTarget.y);

                // Actualise la direction
                updateDirection = true;
            }
        }
    }

    /// <summary>
    /// Actualise la direction
    /// </summary>
    private void UpdateDirection()
    {
        // Si on doit actualise rla direction
        if (updateDirection)
        {
            // Initialise une direction
            Vector2 direction = Vector2.zero;

            // Sprite utilisé
            string spriteName = MySpriteRenderer.sprite.name;
            
            // Verification du sprite
            if (spriteName.Contains("up"))
            {
                direction = Vector2.up;
            }
            else if (spriteName.Contains("down"))
            {
                direction = Vector2.down;
            }
            else if (spriteName.Contains("left"))
            {
                direction = Vector2.left;
            }
            else if (spriteName.Contains("right"))
            {
                direction = Vector2.right;
            }

            // Ajuste la direction
            MyAnimator.SetFloat("x", direction.x);
            MyAnimator.SetFloat("y", direction.y);

            // Réinitialise le flag
            updateDirection = false;
        }
    }
}