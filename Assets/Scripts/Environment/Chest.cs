using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de gestion des coffres
/// </summary>
public class Chest : MonoBehaviour, IInteractable
{
    // Référence sur le SpriteRenderer
    // [SerializeField]
    private SpriteRenderer spriteRenderer;

    // Image du coffre fermé
    [SerializeField]
    private Sprite closedSprite = default;

    // Image du coffre ouvert
    [SerializeField]
    private Sprite openSprite = default;

    // Propriété d'accès sur l'indicateur d'ouverture du coffre
    private bool isOpen;

    // Canvas du coffre
    [SerializeField]
    private CanvasGroup canvasGroup = default;

    // Liste des items
    private List<Item> items;

    // Propriété d'accès sur le coffre
    [SerializeField]
    private ChestScript bank = default;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Interaction avec le personnage
    /// </summary>
    public void Interact()
    {
        // Si le coffre est ouvert
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            // Ajoute les items stockés
            AddItems();

            // Définit le coffre "Ouvert
            isOpen = true;

            // Actualise l'image du coffre
            spriteRenderer.sprite = openSprite;

            // Affiche le coffre
            canvasGroup.alpha = 1;

            // Bloque les interactions
            canvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage
    /// </summary>
    public void StopInteract()
    {
        //Stocke les items
        StoreItems();

        // [TODO : Partage le coffre] Réinitialise le contenu du coffre
        bank.Clear();

        // Définit le coffre "Fermé
        isOpen = false;

        // Actualise l'image du coffre
        spriteRenderer.sprite = closedSprite;

        // Masque le coffre
        canvasGroup.alpha = 0;

        // Débloque les interactions
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Stocke les items
    /// </summary>
    public void StoreItems()
    {
        // Liste des items du coffre
        items = bank.GetItems();
    }

    /// <summary>
    /// Ajoute les items stockés
    /// </summary>
    public void AddItems()
    {
        if (items != null)
        {
            foreach (Item item in items)
            {
                // Ajoute l'item dans son emplacement
                item.MySlot.AddItem(item);
            }
        }
    }
}
