using UnityEngine;



/// <summary>
/// Classe de gestion des vendeurs
/// </summary>
public class Vendor : MonoBehaviour, IInteractable
{
    // Référence sur la fenêtre du vendeur
    [SerializeField]
    private VendorWindow vendorWindow = default;

    // Liste des items du vendeur
    [SerializeField]
    private VendorItem[] items = default;


    /// <summary>
    /// Interaction avec le personnage
    /// </summary>
    public void Interact()
    {
        // Création des pages de la fênêtre du vendeur
        vendorWindow.CreatePages(items);

        // Ouverture de la fenêtre du vendeur
        vendorWindow.Open();
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage
    /// </summary>
    public void StopInteract()
    {
        // Fermeture de la fenêtre du vendeur
        vendorWindow.Close();
    }
}