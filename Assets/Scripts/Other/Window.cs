using UnityEngine;



/// <summary>
/// Classe de gestion des fenêtres
/// </summary>
public class Window : MonoBehaviour
{
    // Canvas de la fenêtre
    protected CanvasGroup canvasGroup;

    // NPC lié à la fenêtre
    private NPC npc;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le canvas de la fenêtre
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Ouverture de la fenêtre du vendeur
    /// </summary>
    public void Open(NPC npcRef)
    {
        // Référence sur le vendeur
        npc = npcRef;

        // Débloque les interactions
        canvasGroup.blocksRaycasts = true;

        // Masque la fenêtre du vendeur
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Fermeture de la fenêtre du vendeur
    /// </summary>
    public void Close()
    {
        // Définit la fenêtre du vendeur comme "fermée"
        npc.IsInteracting = false;

        // Débloque les interactions
        canvasGroup.blocksRaycasts = false;

        // Masque la fenêtre du vendeur
        canvasGroup.alpha = 0;

        // Réinitialise la référence sur le vendeur
        npc = null;
    }
}