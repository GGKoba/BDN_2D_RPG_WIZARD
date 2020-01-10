using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des boutons de la feuille du personnage
/// </summary>
public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Type d'équipement
    [SerializeField]
    private ArmorType armorType;

    // Item d'équipement
    private Armor armor;

    // Image de l'item
    [SerializeField]
    private Image icon;



    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Affiche le tooltip
        //UIManager.MyInstance.ShowTooltip(transform.position, armor);
    }

    /// <summary>
    /// Sortie du curseur
    /// </summary>
    /// <param name="eventData">Evenement de sortie</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Masque le tooltip
        //UIManager.MyInstance.HideTooltip();
    }

    /// <summary>
    /// Gestion du clic
    /// </summary>
    /// <param name="eventData">Evenement de clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Clic gauche
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // S'il l'on déplace un item Armor
            if (Hand.MyInstance.MyMoveable is Armor)
            {
                // Item Armor
                Armor stuff = Hand.MyInstance.MyMoveable as Armor;

                if (stuff.MyArmorType == armorType)
                {
                    // Equipement de l'item
                    EquipArmor(stuff);
                }
            }
        }
    }

    // Equipe un item
    public void EquipArmor(Armor item)
    {
        // Actualise l'image de l'item
        icon.sprite = item.MyIcon;

        // Active l'image de l'item
        icon.enabled = true;

        // Référence sur l'item
        armor = item;

        // Libère l'item
        Hand.MyInstance.DeleteItem();
    }
}
