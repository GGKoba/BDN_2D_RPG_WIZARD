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

    // Propriété d'accès sur l'indicateur d'ouverture de la fenêtre du vendeur
    public bool IsOpen { get; set; }


    /// <summary>
    /// Interaction avec le personnage
    /// </summary>
    public void Interact()
    {
        // Si la fenêtre du vendeur n'est pas ouverte
        if (!IsOpen)
        {
            // Définit la fenêtre comme "ouverte"
            IsOpen = true;

            // Création des pages de la fênêtre du vendeur
            vendorWindow.CreatePages(items);

            // Ouverture de la fenêtre du vendeur
            vendorWindow.Open(this);
        }
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage
    /// </summary>
    public void StopInteract()
    {
        // Si la fenêtre du vendeur est ouverte
        if (IsOpen)
        {
            // Définit la fenêtre comme "fermée"
            IsOpen = false;

            // Suppression des pages de la fênêtre du vendeur
            //vendorWindow.DeletePages();

            // Fermeture de la fenêtre du vendeur
            vendorWindow.Close();
        }
    }
}