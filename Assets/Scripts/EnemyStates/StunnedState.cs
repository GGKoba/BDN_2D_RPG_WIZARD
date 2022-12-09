using UnityEngine;



/// <summary>
/// Classe de l'état "STUN"
/// </summary>
class StunnedState : IState
{
    // Référence sur le script Enemy
    private Enemy parent;


    /// <summary>
    /// Entrée dans l'état "STUN"
    /// </summary>
    /// <param name="enemyScript">le parent de l'ennemi</param>
    public void Enter(Enemy enemyScript)
    {
        parent = enemyScript;

        // Stoppe l'ennemi
        parent.MyDirection = Vector2.zero;
    }

    /// <summary>
    /// Sortie de l'état "STUN"
    /// </summary>
    public void Exit()
    {
    }

    /// <summary>
    /// Mise à jour dans l'état "STUN"
    /// </summary>
    public void Update()
    {
    }
}