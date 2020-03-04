using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les objets héritent
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    [Header("Item")]

    // Clé de l'item
    [SerializeField]
    private string key = default;

    // Propriété d'accès à la clé de l'item
    public string MyKey { get => key; }

    // Taille empilable de l'item
    [SerializeField]
    private int stackSize = default;

    // Propriété d'accès à la taille empilable de l'item
    public int MyStackSize { get => stackSize; }

    // Image de l'item
    [SerializeField]
    private Sprite icon = default;

    // Propriété d'accès à l'image de l'item
    public Sprite MyIcon { get => icon; }

    // Titre de l'item
    [SerializeField]
    private string title = default;

    // Propriété d'accès au titre de l'item
    public string MyTitle { get => title; }

    // Qualité de l'item
    [SerializeField]
    private Quality quality = default;

    // Propriété d'accès à la qualité de l'item
    public Quality MyQuality { get => quality; }

    // Prix de l'item
    [SerializeField]
    private int price = default;

    // Propriété d'accès au prix de l'item
    public int MyPrice { get => price; }

    // Référence sur l'emplacement de l'item
    private SlotScript slot;

    // Propriété d'accès à l'emplacement de l'item
    public SlotScript MySlot { get => slot; set => slot = value; }

    // Emplacement de l'item sur la feuille du personnage
    private CharacterButton characterButton;


    // Propriété d'accès à l'emplacement de l'item sur la feuille du personnage
    public CharacterButton MyCharacterButton
    { 
        get => characterButton;
        set
        {
            // Réinitialise l''emplacement
            MySlot = null;

            characterButton = value;
        }
    }


    /// <summary>
    /// Supprime un item
    /// </summary>
    public void Remove()
    {
        // S'il y a un emplacement pour l'item
        if (MySlot != null)
        {
            // Supprime l'item de l'emplacement
            MySlot.RemoveItem(this);
        }
    }

    /// <summary>
    /// Retourne le titre de l'item : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual string GetDescription()
    {
        // Retourne un titre adapté à sa qualité
        return GetQualitedTitle();
    }

    /// <summary>
    /// Retourne le titre adapté à sa qualité
    /// </summary>
    /// <returns></returns>
    public string GetQualitedTitle(bool boldTitle = true)
    {
        string formattedString = string.Empty;
        formattedString += "<color={0}>";
        if (boldTitle)
        {
            formattedString += "<b>";
        }
        formattedString += "{1}";
        if (boldTitle)
        {
            formattedString += "</b>";
        }
        formattedString += "</color>";

        return string.Format(formattedString, QualityColor.MyColors[quality], title);
    }

}