using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les objets héritent
/// </summary>
public abstract class Item : ScriptableObject, IMoveable
{
    // Taille empilable de l'item
    [SerializeField]
    private int stackSize = default;

    // Image de l'item
    [SerializeField]
    private Sprite icon = default;

    // Propriété d'accès à l'image de l'item
    public Sprite MyIcon { get => icon; }

    // Propriété d'accès à la taille empilable de l'item
    public int MyStackSize { get => stackSize; }

    // Référence sur l'emplacement de l'item
    private SlotScript slot;

    // Propriété d'accès à l'emplacement de l'item
    public SlotScript MySlot { get => slot; set => slot = value; }


    // Supprime un item
    public void Remove()
    {
        // S'il y a un emplacement pour l'item
        if (MySlot != null)
        {
            // Supprime l'item de l'emplacement
            MySlot.RemoveItem(this);
        }
    }
}