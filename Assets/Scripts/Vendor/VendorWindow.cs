using UnityEngine;



/// <summary>
/// Classe de gestion de la fenêtre du vendeur
/// </summary>
public class VendorWindow : MonoBehaviour
{
    // Canvas de la fenêtre
    private CanvasGroup canvasGroup ;

    // Tableau des boutons des butins
    [SerializeField]
    private VendorButton[] vendorButtons = default;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le canvas de la fenêtre
        canvasGroup = GetComponent<CanvasGroup>();

        // Fermeture de la fenêtre du vendeur
        Close();
    }

    /// <summary>
    /// Ouverture de la fenêtre du vendeur
    /// </summary>
    public void Open()
    {
        // Débloque les interactions
        canvasGroup.blocksRaycasts = true;

        // Masque la fenêtre des butins
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Fermeture de la fenêtre du vendeur
    /// </summary>
    public void Close()
    {
        // Débloque les interactions
        canvasGroup.blocksRaycasts = false;

        // Masque la fenêtre des butins
        canvasGroup.alpha = 0;
    }


    /// <summary>
    /// Création de la liste des pages de butin
    /// </summary>
    /// <param name="items">Liste des items du butin</param>
    public void CreatePages(VendorItem[] items)
    {
        // pour tous les items du vendeur
        for (int i = 0; i < items.Length; i++)
        {
            vendorButtons[i].AddItem(items[i]);
        }
    }
}