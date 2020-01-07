using UnityEngine;



public enum Quality { Common, Uncommon, Rare, Epic, Legendary  };


/// <summary>
/// Classe abstraite dont tous les objets héritent
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
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

    // Propriété d'accès à l'image de l'item
    public Sprite MyIcon { get => icon; }

    // Propriété d'accès à la taille empilable de l'item
    public int MyStackSize { get => stackSize; }

    // Référence sur l'emplacement de l'item
    private SlotScript slot;

    // Propriété d'accès à l'emplacement de l'item
    public SlotScript MySlot { get => slot; set => slot = value; }


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
    /// Retourne le titre de l'item : virtual pour être écrasée pour les sous-classes
    /// </summary>
    public virtual string GetDescription()
    {
        string textColor = string.Empty;

        // Adapte la couleur suivant la qualité
        switch (quality)
        {
            case Quality.Common:
                textColor = "#DDE2E2";
                break;
            case Quality.Uncommon:
                textColor = "#0ED145";
                break;
            case Quality.Rare:
                textColor = "#298EDB";
                break;
            case Quality.Epic:
                textColor = "#9D29DB";
                break;
            case Quality.Legendary:
                textColor = "#FF812B";
                break;
        }

        // Retourne un titre adapté à sa qualité
        return string.Format("<color={0}>{1}</color>", textColor, title);
    }
}