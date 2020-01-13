using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des boutons de la feuille du personnage
/// </summary>
public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Item d'équipement
    private Armor equippedArmor;

    // Type d'équipement
    [SerializeField]
    private ArmorType armorType = default;

    // Image de l'item
    [SerializeField]
    private Image icon = default;


    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Si l'emplacement n'est pas vide
        if (equippedArmor != null)
        {
            // Affiche le tooltip
            UIManager.MyInstance.ShowTooltip(new Vector2(0, 0), transform.position, equippedArmor);
        }

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
        // Clic gauche
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // S'il on déplace un item Armor
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
    public void EquipArmor(Armor armor)
    {
        // Retire l'item de l'inventaire
        armor.Remove();
        
        // Si l'emplacement n'est pas vide
        if (equippedArmor != null)
        {
            // Ajoute l'item équipé dans l'emplacement de l'item à equiper
            armor.MySlot.AddItem(equippedArmor);

            // Actualise le tooltip
            UIManager.MyInstance.RefreshTooltip(equippedArmor);
        }
        else
        {
            // Masque le tooltip
            UIManager.MyInstance.HideTooltip();
        }

        // Actualise l'image de l'item
        icon.sprite = armor.MyIcon;

        // Nouvelle couleur
        Color buttonColor = Color.clear;
        
        // La couleur varie en fonction de la qualité de l'item
        ColorUtility.TryParseHtmlString(QualityColor.MyColors[armor.MyQuality], out buttonColor);

        // Actualise la couleur du bouton
        GetComponent<Image>().color = buttonColor;

        // Active l'image de l'item
        icon.enabled = true;

        // Référence sur l'item
        equippedArmor = armor;

        // S'il on déplace un item Armor
        if (Hand.MyInstance.MyMoveable == (armor as IMoveable))
        {
            // Libère l'item
            Hand.MyInstance.Drop();
        }
    }
}
