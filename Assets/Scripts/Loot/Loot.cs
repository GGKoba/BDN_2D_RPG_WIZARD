using System;
using UnityEngine;



/// <summary>
/// Classe de l'objet "Butin"
/// </summary>
[Serializable]
public class Loot
{
    // Item
    [SerializeField]
    private Item item = default;

    // Propriété d'accès à l'item
    public Item MyItem { get => item; }

    // Chance d'obtention
    [SerializeField]
    private float dropChance = default;

    // Propriété d'accès à la hance d'obtention
    public float MyDropChance { get => dropChance; }
}
