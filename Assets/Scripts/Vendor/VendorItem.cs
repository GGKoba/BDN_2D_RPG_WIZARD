using System;
using UnityEngine;



/// <summary>
/// Classe de gestion des items du vendeur
/// </summary>
[Serializable]
public class VendorItem
{
    // Item du vendeur
    [SerializeField]
    private Item item = default;

    // Propriété d'accès à l'iteml
    public Item MyItem { get => item; }

    // Quantité de l'item
    [SerializeField]
    private int quantity = default;

    // Propriété d'accès à la quantité
    public int MyQuantity { get => quantity; set => quantity = value; }

    // Disponible en illimité
    [SerializeField]
    private bool unlimited = default;

    // Propriété d'accès à la disponibilité
    public bool MyUnlimited { get => unlimited; }
}
