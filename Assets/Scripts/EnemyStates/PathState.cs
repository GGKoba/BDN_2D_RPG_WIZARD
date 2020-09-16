﻿using System.Collections.Generic;
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

    // Vitesse de déplacement
    private float speed;

    // Ennemi
    private Enemy parent;

    // Position de la cible
    private Vector3 targetPosition;


    /// <summary>
    /// Entrée dans l'état "PATH"
    /// </summary>
    public void Enter(Enemy enemyScript)
    {
        // Définit l'ennemi
        parent = enemyScript;

        // Définit la vitesse
        speed = enemyScript.MySpeed;

        // Définit l'objet à déplacer
        transform = enemyScript.transform.parent;

        // Définit la position de la cible
        targetPosition = Player.MyInstance.MyCurrentTile.position;

        // Si la cible n'est pas au même endroit
        if(targetPosition != enemyScript.MyCurrentTile.position)
        {
            // Définit le chemin
            path = enemyScript.MyAstar.Algorithm(enemyScript.MyCurrentTile.position, targetPosition);
        }

        // S'il y a un chemin
        if (path != null)
        {
            // Définit le point de départ
            currentNode = path.Pop();

            // Définit la 1ere destination
            destination = path.Pop();

            // Définit le point d'arrivée
            goal = parent.MyCurrentTile.position;
        }
        else
        {
            // Changement d'état
            parent.ChangeState(new EvadeState());
        }
    }

    /// <summary>
    /// Mise à jour dans l'état "PATH"
    /// </summary>
    public void Exit()
    {
        // Réinitialise le chemin
        path = null;
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
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            // Position de la cellule de destination
            Vector3Int dest = parent.MyAstar.MyTilemap.WorldToCell(destination);

            // Position de la cellule de courante
            Vector3Int nodeCurrent = parent.MyAstar.MyTilemap.WorldToCell(currentNode);

            // Distance à parcourir
            float distance = Vector2.Distance(destination, transform.position);

            // Direction vers le bas
            if (nodeCurrent.y > dest.y)
            {
                parent.MyDirection = Vector2.down;
            }
            // Direction vers le haut
            else if (nodeCurrent.y < dest.y)
            {
                parent.MyDirection = Vector2.up;
            }

            if (nodeCurrent.y == dest.y)
            {
                // Direction vers la gauche
                if (nodeCurrent.x > dest.x)
                {
                    parent.MyDirection = Vector2.left;
                }
                // Direction vers la droite
                else if (nodeCurrent.x < dest.x)
                {
                    parent.MyDirection = Vector2.right;
                }
            }

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

                    // Si la cible n'est pas au même endroit
                    if (targetPosition != Player.MyInstance.MyCurrentTile.position)
                    {
                        // Changement d'état
                        parent.ChangeState(new PathState());
                    }

                }
                else
                {
                    // Réinitialise le chemin
                    path = null;

                    // Changement d'état
                    parent.ChangeState(new EvadeState());
                }
            }
        } 
    }
}
