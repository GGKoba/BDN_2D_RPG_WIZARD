using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les objets héritent
/// </summary>
public abstract class Item : ScriptableObject
{
    // Image de l'item
    [SerializeField]
    private Sprite icon = default;

    // Propriété d'accès à l'image de l'item
    public Sprite MyIcon { get => icon; }

    // Taille empilable de l'item
    [SerializeField]
    private int stackSize = default;

    // Propriété d'accès à la taille empilable de l'item
    public int MyStackSize { get => stackSize; }

    // Référence sur l'emplacement de l'item
    private SlotScript slot;

    // Propriété d'accès à l'emplacement de l'item
    protected SlotScript MySlot { get => slot; set => slot = value; }
}