using UnityEngine;



/// <summary>
/// Classe de l'état "EVADE"
/// </summary>
class EvadeState : IState
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
    /// Mise à jour dans l'état "EVADE"
    /// </summary>
    public void Exit()
    {
        // Réinitialise la direction
        enemy.MyDirection = Vector2.zero;

        // Réinitialise les données de l'ennemi
        enemy.Reset();
    }

    /// <summary>
    /// Sortie de l'état "IDLE"
    /// </summary>
    public void Update()
    {
        // Direction entre la position de départ et la position courante de l'ennemi
        enemy.MyDirection = (enemy.MyStartPosition - enemy.transform.position).normalized;

        // Déplacement vers la position de départ
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.MyStartPosition, enemy.MySpeed * Time.deltaTime);

        // Distance entre la position de départ et la position courante de l'ennemi
        float distance = Vector2.Distance(enemy.MyStartPosition, enemy.transform.position);

        // Si l'ennemi est à sa position de départ
        if (distance <= 0)
        {
            // Passage à l'état d'attente
            enemy.ChangeState(new IdleState());
        }
    }
}