using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de gestion du jeu
/// </summary>
public class GameManager : MonoBehaviour
{
    // Référence sur le joueur
    [SerializeField]
    private Player player = default;


    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {

    }

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
        // Clic droit
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast depuis la position de la souris dans le jeu
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            
            // Si l'on touche quelque chose
            if (hit.collider != null)
            {
                // On verifie que c'est un ennemi
                if (hit.collider.tag == "Enemy")
                {
                    // Si c'est un ennemi, on le cible
                    player.MyTarget = hit.transform;
                }
            }
            else
            {
                // On décible la cible
                player.MyTarget = null;
            }

        }
    }
}
