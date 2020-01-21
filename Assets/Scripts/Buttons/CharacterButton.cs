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

    // Référence sur l'emplacement de l'equipement sur le personnage
    [SerializeField]
    private GearSocket gearSocket = default;


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
            // // Si un item Armor est en train d'être déplacé
            if (Hand.MyInstance.MyMoveable is Armor)
            {
                // Item Armor
                Armor stuff = Hand.MyInstance.MyMoveable as Armor;

                if (stuff.MyArmorType == armorType)
                {
                    // Equipement de l'item
                    EquipArmor(stuff);
                }

                // Actualise le tooltip
                UIManager.MyInstance.RefreshTooltip(stuff);
            }
            // Si rien n'est en train d'être déplacé et qu'il y a un clic sur un emplacement qui n'est pas vide
            else if (Hand.MyInstance.MyMoveable == null && equippedArmor != null)
            {
                // Récupération de l'item
                Hand.MyInstance.TakeMoveable(equippedArmor);

                // Référence sur le bouton
                CharacterPanel.MyInstance.MySelectedButton = this;

                // Grise l'image de l'item
                icon.color = Color.grey; 
            }
        }
        // Clic droit
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Si l'emplacement n'est pas vide
            if (equippedArmor != null)
            {
                // Ajoute un item dans un sac de l'inventaire
                InventoryScript.MyInstance.AddItem(equippedArmor);

                /// Déséquipe l'item
                UnequipArmor();
            }
        }
    }

    /// <summary>
    /// Equipe un item
    /// </summary>
    /// <param name="armor"></param>
    public void EquipArmor(Armor armor)
    {
        // Retire l'item de l'inventaire
        armor.Remove();

        // Si l'emplacement n'est pas vide
        if (equippedArmor != null)
        {
            // Si l'item à équiper est différent de l'item équipé
            if (equippedArmor != armor)
            {
                // Ajoute l'item équipé dans l'emplacement de l'item à equiper
                armor.MySlot.AddItem(equippedArmor);
            }

            // Actualise le tooltip
            UIManager.MyInstance.RefreshTooltip(equippedArmor);
        }
        else
        {
            // Masque le tooltip
            UIManager.MyInstance.HideTooltip();
        }

        // Actualise la couleur de l'image de l'item
        icon.color = Color.white;

        // Actualise l'image de l'item
        icon.sprite = armor.MyIcon;

        // Nouvelle couleur
        Color buttonColor = Color.clear;

        // La couleur varie en fonction de la qualité de l'item
        ColorUtility.TryParseHtmlString(QualityColor.MyColors[armor.MyQuality], out buttonColor);

        // Actualise la couleur de l'image de l'emplacement
        GetComponent<Image>().color = buttonColor;

        // Active l'image de l'item
        icon.enabled = true;

        // Référence sur l'item
        equippedArmor = armor;

        // Référence sur l'emplacement de l'item
        equippedArmor.MyCharacterButton = this;

        // S'il on déplace un item Armor
        if (Hand.MyInstance.MyMoveable == (armor as IMoveable))
        {
            // Libère l'item
            Hand.MyInstance.Drop();
        }

        // S'il y a un emplacement pour l'équipement sur le personnage et que l'équipement a des animations
        if (gearSocket != null && equippedArmor.MyAnimationClips != null && equippedArmor.MyAnimationClips.Length > 0)
        {
            // Ecrase les animations de l'équipement
            gearSocket.Equip(equippedArmor.MyAnimationClips);
        }
    }

    /// <summary>
    /// Déséquipe un item
    /// </summary>
    /// <param name="armor"></param>
    public void UnequipArmor()
    {
        // Actualise la couleur de l'image de l'item
        icon.color = Color.white;

        // Actualise l'image de l'item
        icon.sprite = null;

        // Actualise la couleur de l'image de l'emplacement
        GetComponent<Image>().color = Color.white;

        // Active l'image de l'item
        icon.enabled = false;

        // S'il y a un emplacement pour l'équipement sur le personnage et que l'équipement a des animations
        if (gearSocket != null && equippedArmor.MyAnimationClips != null)
        {
            // Réinitialise les animations d'un équipement
            gearSocket.Unequip();
        }

        // Réinitialise l'emplacement de l'item
        equippedArmor.MyCharacterButton = null;

        // Réinitialise la référence sur l'item
        equippedArmor = null;
    }
}
