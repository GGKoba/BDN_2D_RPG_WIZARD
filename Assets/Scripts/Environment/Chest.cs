using UnityEngine;



/// <summary>
/// Classe de gestion des coffres
/// </summary>
public class Chest : MonoBehaviour, IInteractable
{
    // Référence sur le SpriteRenderer
    // [SerializeField]
    private SpriteRenderer spriteRenderer;

    // Image du coffre fermé
    [SerializeField]
    private Sprite closedSprite = default;

    // Image du coffre ouvert
    [SerializeField]
    private Sprite openSprite = default;

    // Propriété d'accès sur l'indicateur d'ouverture du coffre
    private bool isOpen;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Interaction avec le personnage
    /// </summary>
    public void Interact()
    {
        // Si le coffre est ouvert
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            // Définit le coffre "Ouvert
            isOpen = true;

            // Actualise l'image du coffre
            spriteRenderer.sprite = openSprite;
        }
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage
    /// </summary>
    public void StopInteract()
    {
        // Définit le coffre "Fermé
        isOpen = false;

        // Actualise l'image du coffre
        spriteRenderer.sprite = closedSprite;
    }
}
