﻿using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;



// Gestion de la mort d'un personnage
public delegate void KillConfirmed(Character character);


/// <summary>
/// Classe de gestion du jeu
/// </summary>
public class GameManager : MonoBehaviour
{
    // Instance de classe (singleton)
    private static GameManager instance;

    // Propriété d'accès à l'instance
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type GameManager (doit être unique)
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    // Référence à la caméra
    private Camera mainCamera;

    // Evènement de mort d'un personnage
    public event KillConfirmed KillConfirmedEvent;

    // Référence sur le joueur
    [SerializeField]
    private Player player = default;

    // Référence sur les layers
    [SerializeField]
    private LayerMask clickableLayer = default, groundLayer = default;

    // Cible courante
    private Enemy currentTarget;

    // Référence sur l'argent du joueur
    [SerializeField]
    private Text goldText = default;

    // Index de la cible
    private int targetIndex;

    // Liste des zones "non particables"
    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    // Propriété d'accès à la liste des zones "non particables"
    public HashSet<Vector3Int> MyBlocked { get => blocked; set => blocked = value; }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // Exécute le clic sur une cible
        ClickTarget();

        // Gestion des cibles
        NextTarget();

        // Actualise l'argent du joueur
        goldText.text = player.MyGold.ToString();
    }

    /// <summary>
    /// Gestion du clic sur une cible
    /// </summary>
    private void ClickTarget()
    {
        // Raycast depuis la position de la souris dans le jeu
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

        // Clic gauche et que l'on ne pointe pas sur un élément de l'interface (par exemple un bouton d'action)
        if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject())
        {
            // Désélection de la cible courante
            DeSelectTarget();

            // Si l'on touche un ennemi
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // Sélection de la nouvelle cible
                SelectTarget(hit.collider.GetComponent<Enemy>());
            }
            // Désélection de la cible
            else
            {
                // Masque la frame de la cible
                UIManager.MyInstance.HideTargetFrame();

                // Supprime les références à la cible
                currentTarget = null;
                player.MyTarget = null;
            }
        }
        // Clic droit et que l'on ne pointe pas sur un élément de l'interface (par exemple un bouton d'action)
        else if (Input.GetMouseButtonDown(1) & !EventSystem.current.IsPointerOverGameObject())
        {
            // S'il y a une entité avec laquelle le joueur est en interaction
            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

                // Si l'on touche quelque chose et que celle-ci a un tag "Enemy" ou Interactable et que cette entité est dans la liste avec laquelle le joueur est en interaction
                if (hit.collider != null && (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Interactable")) && player.MyInteractables.Contains(entity))
                {
                    // Interaction avec le personnage
                    entity.Interact();
                }
            }
            else
            {
                // Raycast depuis la position de la souris dans le jeu
                RaycastHit2D hitGround = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

                // S'il y a une entité avec laquelle le joueur est en interaction
                if (hitGround.collider != null)
                {
                    // Calcul le chemin jusqu'au point cliqué
                    player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                }
            }
        }
    }

    /// <summary>
    /// Appelle l'évènement de la mort d'un personnage
    /// </summary>
    public void OnKillConfirmed(Character character)
    {
        // S'il y a un abonnement à cet évènement
        if (KillConfirmedEvent != null)
        {
            // Déclenchement de l'évènement de la mort d'un personnage
            KillConfirmedEvent.Invoke(character);
        }
    }

    /// <summary>
    /// Cible la prochaine cible
    /// </summary>
    private void NextTarget()
    {
        // [TAB]
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Désélectionne la cible
            DeSelectTarget();

            if (Player.MyInstance.MyAttackers.Count > 0)
            {
                // Si l'index est inférieur au nombre de la liste
                if (targetIndex < Player.MyInstance.MyAttackers.Count)
                {
                    // Sélectionne la cible
                    SelectTarget(Player.MyInstance.MyAttackers[targetIndex]);

                    // Incrémente l'index de la cible
                    targetIndex++;

                    // Si l'index dépasse le nombre de la liste
                    if (targetIndex >= Player.MyInstance.MyAttackers.Count)
                    {
                        // Réinitialise l'index
                        targetIndex = 0;
                    }
                }
                else
                {
                    // Réinitialise l'index
                    targetIndex = 0;
                }
            }
        }
    }

    /// <summary>
    /// Désélectionne la cible courante
    /// </summary>
    private void DeSelectTarget()
    {
        // S'il y a une cible
        if (currentTarget != null)
        {
            // Désélectionne la cible
            currentTarget.DeSelect();
        }
    }

    /// <summary>
    /// Sélectionne la cible
    /// </summary>
    private void SelectTarget(Enemy enemy)
    {
        // Actualise la cible
        currentTarget = enemy;

        // Sélectionne la cible
        player.MyTarget = currentTarget.Select();

        // Affiche la frame de la cible
        UIManager.MyInstance.ShowTargetFrame(currentTarget);
    }
}