using UnityEngine;



/// <summary>
/// Classe de gestion des vendeurs
/// </summary>
public class Vendor : NPC, IInteractable
{
    // Liste des items du vendeur
    [SerializeField]
    private VendorItem[] items = default;
}