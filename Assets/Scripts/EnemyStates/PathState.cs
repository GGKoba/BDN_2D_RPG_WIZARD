using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de l'état "PATH"
/// </summary>
public class PathState : IState
{
    // Stack du chemin
    private Stack<Vector3> path;

    // Objectif
    private Vector3 goal;

    // Destination
    private Vector3 destination;

    // Noeud courant
    private Vector3 currentNode;

    // Objet à manipuler
    private Transform transform;

    private float speed;


    /// <summary>
    /// Entrée dans l'état "PATH"
    /// </summary>
    public void Enter(Enemy enemyScript)
    {
        // Définit l'objet à déplacer
        transform = enemyScript.transform;

        // Définit le chemin
        path = enemyScript.MyAstar.Algorithm(enemyScript.transform.parent.position, enemyScript.MyTarget.position);

        // Définit le point de départ
        currentNode = path.Pop();

        // Définit la 1ere destination
        destination = path.Pop();

        // Définit le point d'arrivée
        goal = enemyScript.MyTarget.parent.position;

        // Définit la vitesse
        speed = enemyScript.MySpeed;
    }

    /// <summary>
    /// Mise à jour dans l'état "PATH"
    /// </summary>
    public void Exit()
    {

    }

    /// <summary>
    /// Sortie de l'état "PATH"
    /// </summary>
    public void Update()
    {
        // S'il y a un chemin
        if (path != null)
        {
            // Déplacement vers la destination
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, speed * Time.deltaTime);

            // Distance à parcourir
            float distance = Vector2.Distance(destination, transform.parent.position);

            // Si la distance est nulle
            if (distance <= 0f)
            {
                // S'il y a un chemin à faire
                if (path.Count > 0)
                {
                    // Mise à jour du noeud courant
                    currentNode = destination;

                    // Mise à jour de la destination
                    destination = path.Pop();
                }
                else
                {
                    // Réinitialise le chemin
                    path = null;
                }
            }
        } 
    }
}
