using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des boutons des butins
/// </summary>
public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

    // Propriété d'accès à l'item du bouton
    public Item MyLoot { get; set; }

    // Référence sur le script LootWindow
    private LootWindow lootWindow;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le script LootWindow
        lootWindow = GetComponentInParent<LootWindow>();
    }

    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Affiche le tooltip
        UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, MyLoot);
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

            // Retire l'item de la page et de la liste des butins
            lootWindow.TakeLoot(MyLoot);

            // Masque le tooltip
            UIManager.MyInstance.HideTooltip();
        }
    }
}