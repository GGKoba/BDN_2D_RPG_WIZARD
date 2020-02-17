using UnityEngine;



/// <summary>
/// Classe de l'état "FOLLOW"
/// </summary>
class FollowState : IState
{
    // Référence sur le script Enemy
    private Enemy enemy;


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
            enemy.MyDirection = (enemy.MyTarget.position - enemy.transform.position).normalized;

            // Déplacement vers la cible
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.MyTarget.position, enemy.MySpeed * Time.deltaTime);

            // Distance entre l'ennemi et la cible
            float distance = Vector2.Distance(enemy.MyTarget.position, enemy.transform.position);

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