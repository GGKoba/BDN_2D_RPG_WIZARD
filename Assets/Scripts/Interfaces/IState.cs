/// <summary>
/// Interface des états des personnages
/// </summary>
public interface IState
{
    // Entrée dans l'état
    void Enter(Enemy enemyScript);

    // Mise à jour dans l'état
    void Update();

    // Sortie de l'état
    void Exit();
}