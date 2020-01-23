using UnityEngine;



/// <summary>
/// Classe de gestion des vendeurs
/// </summary>
public class Vendor : NPC, IInteractable
{
    // Liste des items du vendeur
    [SerializeField]
    private VendorItem[] items = default;

    // Propriété d'accès à la liste des items du vendeur
    public VendorItem[] MyItems { get => items; }
}