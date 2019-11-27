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


    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        // Exécute le clic sur une cible
        ClickTarget();
    }

    /// <summary>
    /// Gestion du clic sur une cible
    /// </summary>
    private void ClickTarget()
    {
        // Clic droit et que l'on ne pointe pas sur un GameObject (par exemple un ennemi)
        if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject())
        {
            // Raycast depuis la position de la souris dans le jeu
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            
            // Si l'on touche quelque chose
            if (hit.collider != null)
            {
                // Vérifie que c'est un ennemi
                if (hit.collider.CompareTag("Enemy"))
                {
                    // Assigne la hitbox comme cible
                    player.MyTarget = hit.transform.GetChild(0);
                }
            }
            else
            {
                // Décible la cible
                player.MyTarget = null;
            }

        }
    }
}
