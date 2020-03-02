using System;
using UnityEngine;


/// <summary>
/// Classe de gestion des matériaux de fabrication
/// </summary>
[Serializable]
public class CraftingMaterial
{
    // Item de fabrication
    [SerializeField]
    private Item item = default;

    // Propriété d'accès à l'item de fabrication
    public Item MyItem { get => item; }

    // Nombre nécessaire à la fabrication
    [SerializeField]
    private int count = default;

    // Propriété d'accès au nombre nécessaire à la fabrication
    public int MyCount { get => count; }
}