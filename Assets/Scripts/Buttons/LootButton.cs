using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Classe de gestion des boutons des butins
/// </summary>
public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Image du bouton
    [SerializeField]
    private Image icon = default;

    // Propriété d'accès à l'image du bouton
    public Image MyIcon { get => icon; }

    // Titre du bouton
    [SerializeField]
    private Text title = default;

    // Propriété d'accès au titre du bouton
    public Text MyTitle { get => title; }

    // Propriété d'accès à l'item du butin
    public Item MyLoot { get; set; }


    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Affiche le tooltip
        UIManager.MyInstance.ShowTooltip(transform.position, MyLoot);
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
        // Récupère l'item s'il y a de la place 
        if (InventoryScript.MyInstance.AddItem(MyLoot))
        {
            // Réinitialisation du bouton
            gameObject.SetActive(false);

            // Masque le tooltip
            UIManager.MyInstance.HideTooltip();
        }
    }
}