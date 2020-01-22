using UnityEngine;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux personnage non jouable
/// </summary>
public class NPC : IInteractable
{
    // Fenêtre liée au NPC 
    [SerializeField]
    private Window window;

    // Propriété d'accès sur l'état d'interaction
    public bool IsInteracting { get; set; }


    /// <summary>
    /// Interaction avec le personnage : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual void Interact()
    {
        // S'il n'y a pas d'interaction
        if (!IsInteracting)
        {
            // Début de l'interaction
            IsInteracting = true;

            // Ouverture de la fenêtre
            window.Open(this);
        }
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage : virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual void StopInteract()
    {
        // S'il y a pas d'interaction
        if (IsInteracting)
        {
            // Fin de l'interaction
            IsInteracting = false;

            // Fermeture de la fenêtre
            window.Close();
        }
    }
}