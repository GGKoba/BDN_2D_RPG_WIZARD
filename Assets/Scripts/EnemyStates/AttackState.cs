using UnityEngine;



/// <summary>
/// Classe de l'état "ATTACK"
/// </summary>
class AttackState : IState
{
    // Référence sur le script Enemy
    private Enemy enemy;


    /// <summary>
    /// Entrée dans l'état "IDLE"
    /// </summary>
    public void Enter(Enemy enemyScript)
    {
        enemy = enemyScript;
    }

    /// <summary>
    /// Mise à jour dans l'état "IDLE"
    /// </summary>
    public void Exit()
    {
    }

    /// <summary>
    /// Sortie de l'état "IDLE"
    /// </summary>
    public void Update()
    {
        // S'il y a une cible
        if (enemy.MyTarget != null)
        {
            // Distance entre l'ennemi et la cible
            float distance = Vector2.Distance(enemy.MyTarget.position, enemy.transform.position);

            // Si la cible est à portée d'attaque
            if (distance >= enemy.MyAttackRange)
            {
                // Passage à l'état de poursuite
                enemy.ChangeState(new FollowState());
            }
        }
        else
        {
            // Passage à l'état d'attente
            enemy.ChangeState(new IdleState());
        }
    }
}