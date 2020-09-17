using UnityEngine;



/// <summary>
/// Classe de l'état "EVADE"
/// </summary>
class EvadeState : IState
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
    }

    /// <summary>
    /// Mise à jour dans l'état "EVADE"
    /// </summary>
    public void Exit()
    {
        // Réinitialise la direction
        parent.MyDirection = Vector2.zero;

        // Réinitialise les données de l'ennemi
        parent.Reset();
    }

    /// <summary>
    /// Sortie de l'état "IDLE"
    /// </summary>
    public void Update()
    {
        // Direction entre la position de départ et la position courante de l'ennemi
        parent.MyDirection = (parent.MyStartPosition - parent.transform.position).normalized;

        // Déplacement vers la position de départ
        parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyStartPosition, parent.MySpeed * Time.deltaTime);

        // Distance entre la position de départ et la position courante de l'ennemi
        float distance = Vector2.Distance(parent.MyStartPosition, parent.transform.position);

        // Si l'ennemi est à sa position de départ
        if (distance <= 0)
        {
            // Passage à l'état d'attente
            parent.ChangeState(new IdleState());
        }
    }
}