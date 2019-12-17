using UnityEngine;



/// <summary>
/// Classe abstraite dont tous les objets héritent
/// </summary>
public abstract class Item : ScriptableObject
{
    // Icône de l'item
    [SerializeField]
    private Sprite icon;

    // Propriété d'accès à l'icône de l'item
    public Sprite MyIcon { get => icon; }

    // Taille empilable de l'item
    [SerializeField]
    private int stackSize;

    // Propriété d'accès à la taille empilable de l'item
    public int MyStackSize { get => stackSize; }

    // Référence sur l'emplacement de l'item
    private Slot slot;

    // Propriété d'accès à l'emplacement de l'item
    protected Slot MySlot { get => slot; set => slot = value; }
}