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
    private Quality quality;

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
    /// Retourne le titre de l'item
    /// </summary>
    public string GetDescription()
    {
        string color = string.Empty;

        // Adapte la couleur suivant la qualité
        switch (quality)
        {
            case Quality.Common:
                color = "white";
                break;
            case Quality.Uncommon:
                color = "green";
                break;
            case Quality.Rare:
                color = "blue";
                break;
            case Quality.Epic:
                color = "purple";
                break;
            case Quality.Legendary:
                color = "orange";
                break;
            default:
                color = "grey";
                break;
        }

        // Retourne un titre adapté à sa qualité
        return string.Format("<color={0}>{1}</color>", color, title);
    }
}