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
    }

    /// <summary>
    /// Mise à jour dans l'état "FOLLOW"
    /// </summary>
    public void Exit()
    {
        // Reset de la direction
        enemy.MyDirection = Vector2.zero;
    }

    /// <summary>
    /// Sortie de l'état "FOLLOW"
    /// </summary>
    public void Update()
    {
        // S'il y a une cible à portée
        if (enemy.MyTarget != null)
        {
            // Trouve la direction de la cible
            enemy.MyDirection = (enemy.MyTarget.transform.position - enemy.transform.position).normalized;

            // Déplacement vers la cible
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.MyTarget.position, enemy.MySpeed * Time.deltaTime);
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }
}