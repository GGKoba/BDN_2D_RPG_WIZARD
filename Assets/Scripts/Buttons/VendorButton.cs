using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des boutons des items du vendeur
/// </summary>
public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Image de l'item du bouton
    [SerializeField]
    private Image icon = default;

    // Propriété d'accès à l'image de l'item du bouton
    public Image MyIcon { get => icon; }

    // Titre de l'item du bouton
    [SerializeField]
    private Text title = default;

    // Propriété d'accès au titre de l'item du bouton
    public Text MyTitle { get => title; }

    // Prix de l'item du bouton
    [SerializeField]
    private Text price = default;

    // Quantité de l'item du bouton
    [SerializeField]
    private Text quantity = default;

    // Item du bouton
    private VendorItem vendorItem;


    // Référence sur le script VendorWindow
    //private VendorWindow vendorWindow;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le script VendorWindow
        //vendorWindow = GetComponentInParent<VendorWindow>();
    }

    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Affiche le tooltip
        UIManager.MyInstance.ShowTooltip(new Vector2(0, 1), transform.position, vendorItem.MyItem);
    }

    /// <summary>
    /// Sortie du curseur
    /// </summary>
    /// <param name="eventData">Evenement de sortie</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Masque le tooltip
        UIManager.MyInstance.HideTooltip();
    }

    /// <summary>
    /// Gestion du clic
    /// </summary>
    /// <param name="eventData">Evenement de clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Si le joueur a assez d'argent et qu'il a de la place 
        if ((Player.MyInstance.MyGold >= vendorItem.MyItem.MyPrice) && InventoryScript.MyInstance.AddItem(Instantiate(vendorItem.MyItem)))
        {
            // Vente de l'item
            SellItem();
        }
    }

    /// <summary>
    /// Ajoute un item
    /// </summary>
    /// <param name="vendorItem">item du vendeur</param>
    public void AddItem(VendorItem itemToAdd)
    {
        // Assigne l'item du bouton
        vendorItem = itemToAdd;

        // Si j'ai une quantité pour cet item ou que je n'ai pas de quantité mais que l'item est en disponibilité illimitée
        if (itemToAdd.MyQuantity > 0 || (itemToAdd.MyQuantity == 0 && itemToAdd.MyUnlimited))
        {
            // Actualise l'image de l'item du bouton
            icon.sprite = itemToAdd.MyItem.MyIcon;

            // Actualise le titre de l'item du bouton
            title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[itemToAdd.MyItem.MyQuality], itemToAdd.MyItem.MyTitle);

            // Actualise le prix de l'item du bouton
            price.text = (itemToAdd.MyItem.MyPrice > 0) ? string.Format("Prix : {0}", itemToAdd.MyItem.MyPrice) : string.Empty;

            // Si la disponibilité est limitée
            if (!itemToAdd.MyUnlimited)
            {
                // Actualise la quantité de l'item du bouton
                quantity.text = itemToAdd.MyQuantity.ToString();
            }
            else
            {
                // Réinitialise la quantité de l'item du bouton
                quantity.text = string.Empty;
            }

            // Nouvelle couleur
            Color buttonColor = Color.clear;

            // La couleur varie en fonction de la qualité de l'item
            ColorUtility.TryParseHtmlString(QualityColor.MyColors[itemToAdd.MyItem.MyQuality], out buttonColor);

            // Actualise la couleur de l'image de l'emplacement
            GetComponent<Image>().color = buttonColor;

            // Active le bouton de l'item
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Vente de l'item
    /// </summary>
    private void SellItem()
    {
        // Deduit l'argent au joueur
        Player.MyInstance.MyGold -= vendorItem.MyItem.MyPrice;

        // Si la disponibilité n'est pas illimitée
        if (!vendorItem.MyUnlimited)
        {
            // Reduit la quantité
            vendorItem.MyQuantity--;

            // Actualise la quantité de l'item du bouton
            quantity.text = vendorItem.MyQuantity.ToString();

            if (vendorItem.MyQuantity == 0)
            {
                // Réinitialisation du bouton
                gameObject.SetActive(false);

                // Masque le tooltip
                UIManager.MyInstance.HideTooltip();
            }
        }
    }
}
