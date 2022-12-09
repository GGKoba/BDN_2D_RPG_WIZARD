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

    // Propriété d'accès sur la liste des items
    public List<Item> MyItems { get => items; set => items = value; }

    // Référence sur le coffre
    [SerializeField]
    private ChestScript bank = default;

    // Propriété d'accès sur le coffre
    public ChestScript MyBank { get => bank; set => bank = value; }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur la sprit renderer du coffre
        spriteRenderer = GetComponent<SpriteRenderer>();

        Close();

        items = new List<Item>();
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

            // Ouverture de la fenêtre
            Open();
        }
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage
    /// </summary>
    public void StopInteract()
    {
        // Si le coffre est ouvert
        if (isOpen)
        {
            //Stocke les items
            StoreItems();

            // [TODO : Partage le coffre] Réinitialise le contenu du coffre
            bank.Clear();

            // Définit le coffre "Fermé
            isOpen = false;

            // Actualise l'image du coffre
            spriteRenderer.sprite = closedSprite;

            // Fermeture de la fenêtre
            Close();
        }
    }

    /// <summary>
    /// Ouverture de la fenêtre
    /// </summary>
    public void Open()
    {
        // Affiche la fenêtre
        canvasGroup.alpha = 1;

        // Bloque les interactions
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Fermeture de la fenêtre
    /// </summary>
    public void Close()
    {
        // Masque la fenêtre
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
        items = MyBank.GetItems();
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
