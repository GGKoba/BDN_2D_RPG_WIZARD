using UnityEngine;



/// <summary>
/// Classe de l'état "FOLLOW"
/// </summary>
class FollowState : IState
{
    // Référence sur le script Enemy
    private Enemy parent;

    // Marage d'alignement
    private Vector3 offset;


    /// <summary>
    /// Entrée dans l'état "FOLLOW"
    /// </summary>
    /// <param name="enemyScript">le parent de l'ennemi</param>
    public void Enter(Enemy enemyScript)
    {
        parent = enemyScript;

        // Ajoute l'ennemi dans la liste des attaquants
        Player.MyInstance.MyAttackers.Add(parent);

        // Réinitialise le chemin
        parent.MyPath = null;
    }

    /// <summary>
    /// Mise à jour dans l'état "FOLLOW"
    /// </summary>
    public void Exit()
    {
        // Réinitialise la direction/mouvement
        parent.MyDirection = Vector2.zero;
        parent.MyRigidbodyCharacter.velocity = Vector2.zero;
    }

    /// <summary>
    /// Sortie de l'état "FOLLOW"
    /// </summary>
    public void Update()
    {
        // S'il y a une cible
        if (parent.MyTarget != null)
        {
            // Trouve la direction de la cible
            parent.MyDirection = ((parent.MyTarget.transform.position + offset) - parent.transform.position).normalized;

            // Distance entre l'ennemi et la cible
            float distance = Vector2.Distance(parent.MyTarget.transform.position + offset, parent.transform.position);

            string animationName = parent.MySpriteRenderer.sprite.name;

            // Verification du sprite
            if (animationName.Contains("right"))
            {
                offset = new Vector3(0.5f, 0.8f);
            }
            else if (animationName.Contains("left"))
            {
                offset = new Vector3(-0.5f, 0.8f);
            }
            else if (animationName.Contains("up"))
            {
                offset = new Vector3(0f, 1.2f);
            }
            else if (animationName.Contains("down"))
            {
                offset = new Vector3(0, 0);
            }

            // Si la cible est à portée d'attaque
            if (distance <= parent.MyAttackRange)
            {
                // Passage à l'état d'attaque
                parent.ChangeState(new AttackState());
            }
        }

        // Si la cible n'est plus à portée
        if (!parent.InRange)
        {
            // Passage à l'état d'évasion
            parent.ChangeState(new EvadeState());
        }
        // Si l'ennemi ne peut pas voir le joueur
        else if (!parent.CanSeePlayer())
        {
            // Passage à l'état de recherche
            parent.ChangeState(new PathState());
        }
    }
}