using UnityEngine;



/// <summary>
/// Classe de l'état "EVADE"
/// </summary>
class EvadeState : IState
{
    // Référence sur le script Enemy
    private Enemy parent;


    /// <summary>
    /// Entrée dans l'état "EVADE"
    /// </summary>
    /// <param name="enemyScript">le parent de l'ennemi</param>
    public void Enter(Enemy enemyScript)
    {
        parent = enemyScript;
    }

    /// <summary>
    /// Sortie de l'état "EVADE"
    /// </summary>
    public void Exit()
    {
        // Réinitialise la direction
        parent.MyDirection = Vector2.zero;

        // Réinitialise les données de l'ennemi
        parent.Reset();
    }

    /// <summary>
    /// Mise à jour dans l'état "EVADE"
    /// </summary>
    public void Update()
    {
        // Direction entre la position de départ et la position courante de l'ennemi
        parent.MyDirection = (parent.MyStartPosition - parent.transform.position).normalized;

        // Distance entre la position de départ et la position courante de l'ennemi
        float distance = Vector2.Distance(parent.MyStartPosition, parent.transform.position);

        // Si l'ennemi est à sa position de départ
        if (distance <= 0.1f)
        {
            // Passage à l'état d'attente
            parent.ChangeState(new IdleState());
        }
    }
}