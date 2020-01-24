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

        // Fermeture de la fenêtre
        Close();
    }

    /// <summary>
    /// Ouverture de la fenêtre : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual void Open(NPC npcRef)
    {
        // Référence sur le npc
        npc = npcRef;

        // Débloque les interactions
        canvasGroup.blocksRaycasts = true;

        // Masque la fenêtre
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Fermeture de la fenêtre : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual void Close()
    {
        // S'il y a une référense sur un npc
        if (npc)
        {
            // Définit la fenêtre comme "fermée"
            npc.IsInteracting = false;

            // Réinitialise la référence sur le npc
            npc = null;
        }

        // Débloque les interactions
        canvasGroup.blocksRaycasts = false;

        // Masque la fenêtre
        canvasGroup.alpha = 0;
    }
}