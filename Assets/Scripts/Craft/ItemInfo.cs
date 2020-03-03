using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Item à afficher
    [SerializeField]
    private Item item = default;

    // Propriété d'accès à l'item
    public Item MyItem { get => item; set => item = value; }

    // Image de l'item
    [SerializeField]
    private Image image = default;

    // Texte de l'item
    [SerializeField]
    private Text title = default;

    // Stack de l'item
    [SerializeField]
    private Text stack = default;

    // Nombre courant d'items
    [SerializeField]
    private int count = default;


    /// <summary>
    /// Initialisation de l'item
    /// </summary>
    /// <param name="item">Item à initialiser</param>
    /// <param name="count">Nombre d'items</param>
    public void Initialize(Item newItem, int nb)
    {
        // Assigne l'item
        item = newItem;
        // Assigne le nombre
        count = nb;

        // Actualise l'image
        image.sprite = newItem.MyIcon;

        // Actualise le titre
        title.text = newItem.GetQualitedTitle(false);

        // S'il y a plus d'un élément
        if (count > 1)
        {
            // Active le texte
            stack.enabled = true;

            // Actualise les informations en fonction du nombre d'éléments dans l'inventaire 
            //stack.text = InventoryScript.MyInstance.GetItemCount(item.MyTitle).ToString() + "/" + count.ToString();
            UpdateStackCount();
        }
    }

    /// <summary>
    /// Mise à jour du nombre d'éléments dans l'inventaire 
    /// </summary>
    public void UpdateStackCount()
    {
        stack.text = InventoryScript.MyInstance.GetItemCount(item.MyTitle).ToString() + "/" + count.ToString();
    }

    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    /// <summary>
    /// Sortie du curseur
    /// </summary>
    /// <param name="eventData">Evenement de sortie</param>
    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
