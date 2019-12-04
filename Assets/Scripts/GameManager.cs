using UnityEngine;
using UnityEngine.EventSystems;



/// <summary>
/// Classe de gestion du jeu
/// </summary>
public class GameManager : MonoBehaviour
{
    // Référence sur le joueur
    [SerializeField]
    private Player player = default;

    // Cible courante
    private NPC currentTarget;


    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // Exécute le clic sur une cible
        ClickTarget();
    }

    /// <summary>
    /// Gestion du clic sur une cible
    /// </summary>
    private void ClickTarget()
    {
        // Clic droit et que l'on ne pointe pas sur un élément de l'interface (par exemple un bouton d'action)
        if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject())
        {
            // Raycast depuis la position de la souris dans le jeu
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));

            // S'il y a déjà une cible
            if (currentTarget != null)
            {
                // Désélection de la cible courante
                currentTarget.DeSelect();
            }

            // Si l'on touche quelque chose
            if (hit.collider != null)
            {
                // Sélection de la nouvelle cible
                currentTarget = hit.collider.GetComponent<NPC>();

                // Affecte la nouvelle cible au joueur
                player.MyTarget = currentTarget.Select();

                // Affiche la frame de la cible
                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            // Désélection de la cible
            else
            {
                // Affiche la frame de la cible
                UIManager.MyInstance.HideTargetFrame();

                // Supprime les références à la cible
                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }
}