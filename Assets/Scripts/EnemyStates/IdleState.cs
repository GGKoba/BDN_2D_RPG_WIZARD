/// <summary>
/// Classe de l'état "IDLE"
/// </summary>
class IdleState : IState
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
            // Passage à l'état de poursuite
            enemy.ChangeState(new FollowState());
        }
    }
}