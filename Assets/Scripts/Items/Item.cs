using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les objets héritent
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    [Header("Item")]

    // Taille empilable de l'item
    [SerializeField]
    private int stackSize = default;

    // Image de l'item
    [SerializeField]
    private Sprite icon = default;

    // Titre de l'item
    [SerializeField]
    private string title = default;

    // Qualité de l'item
    [SerializeField]
    private Quality quality = default;
    
    // Propriété d'accès à la taille empilable de l'item
    public int MyStackSize { get => stackSize; }

    // Propriété d'accès à l'image de l'item
    public Sprite MyIcon { get => icon; }

    // Propriété d'accès au titre de l'item
    public string MyTitle { get => title; }

    // Propriété d'accès à la qualité de l'item
    public Quality MyQuality { get => quality; }

    // Référence sur l'emplacement de l'item
    private SlotScript slot;

    // Propriété d'accès à l'emplacement de l'item
    public SlotScript MySlot { get => slot; set => slot = value; }

    // Propriété d'accès à l'emplacement de l'item sur la feuille du personnage
    public CharacterButton MyCharacterButton { get; set; }


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

            // Réinitialise l''emplacement
            MySlot = null;
        }
    }

    /// <summary>
    /// Retourne le titre de l'item : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual string GetDescription()
    {
        // Retourne un titre adapté à sa qualité
        return string.Format("<color={0}><b>{1}</b></color>", QualityColor.MyColors[quality], title);
    }
}