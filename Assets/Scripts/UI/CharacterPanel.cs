using UnityEngine;



/// <summary>
/// Classe de gestion de la feuille du personnage
/// </summary>
public class CharacterPanel : MonoBehaviour
{
    // CanvasGroup de la feuille du personnage
    [SerializeField]
    private CanvasGroup canvasGroup;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le canvasGroup de la fenêtre des butins
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Fermeture de la fenêtre des butins
        OpenClose();
    }

    /// <summary>
    /// Ouverture/Fermeture de la feuille du personnage
    /// </summary>
    public void OpenClose()
    {
        // Bloque/débloque les interactions
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;

        // Masque(0) /Affiche(1) le menu
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
    }
}
