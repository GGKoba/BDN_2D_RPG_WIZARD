using UnityEngine;



/// <summary>
/// Classe de l'état "FOLLOW"
/// </summary>
class FollowState : IState
{
    // Référence sur le script Enemy
    private Enemy enemy;

    // Marage d'alignement
    private Vector3 offset;


    /// <summary>
    /// Entrée dans l'état "FOLLOW"
    /// </summary>
    public void Enter(Enemy enemyScript)
    {
        enemy = enemyScript;

        // Ajoute l'ennemi dans la liste des attaquants
        Player.MyInstance.MyAttackers.Add(enemy);
    }

    /// <summary>
    /// Mise à jour dans l'état "FOLLOW"
    /// </summary>
    public void Exit()
    {
        // Réinitialise la direction
        enemy.MyDirection = Vector2.zero;
    }

    /// <summary>
    /// Sortie de l'état "FOLLOW"
    /// </summary>
    public void Update()
    {
        // S'il y a une cible
        if (enemy.MyTarget != null)
        {
            // Trouve la direction de la cible
            enemy.MyDirection = ((enemy.MyTarget.position + offset) - enemy.transform.position).normalized;

            // Distance entre l'ennemi et la cible
            float distance = Vector2.Distance(enemy.MyTarget.position + offset, enemy.transform.position);

            string animationName = enemy.MySpriteRenderer.sprite.name;

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
            if (distance <= enemy.MyAttackRange)
            {
                // Passage à l'état d'attaque
                enemy.ChangeState(new AttackState());
            }
        }

        // Si la cible n'est plus à portée
        if (!enemy.InRange)
        {
            // Passage à l'état d'évasion
            enemy.ChangeState(new EvadeState());
        }
    }
}