/// <summary>
/// Classe de l'état "IDLE"
/// </summary>
class IdleState : IState
{
    // Référence sur le script Enemy
    private Enemy parent;


    /// <summary>
    /// Entrée dans l'état "IDLE"
    /// </summary>
    /// <param name="enemyScript">le parent de l'ennemi</param>
    public void Enter(Enemy enemyScript)
    {
        parent = enemyScript;

        // Réinitialise les données de l'ennemi
        parent.Reset();
    }

    /// <summary>
    /// Sortie de l'état "IDLE"
    /// </summary>
    public void Exit()
    {
    }

    /// <summary>
    /// Mise à jour dans l'état "IDLE"
    /// </summary>
    public void Update()
    {
        // S'il y a une cible
        if (parent.MyTarget != null)
        {
            // Passage à l'état de recherche
            parent.ChangeState(new PathState());
        }
    }
}