
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de gestion de l'ordre des layers
/// </summary>
public class LayerSorter : MonoBehaviour
{
    // OrderInLayer par défaut du joueur 
    private int defaultOrderInLayer = 200;
    
    // SpriteRenderer du joueur
    private SpriteRenderer parentRenderer;

    // Liste de tous les obstacles avec lesquels le joueur est en collision
    private List<Obstacle> obstacles = new List<Obstacle>();


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence au SpriteRenderer du joueur
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Détection de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le joueur touche un "Obstacle"
        if (collision.CompareTag("Obstacle"))
        {
            // Référence sur l'obstacle
            Obstacle o = collision.GetComponent<Obstacle>();

            // S'il n'y a pas d'autres collisions ou qu'il y a collision avec un obstacle qui a un OrderInLayer inférieur à celui du joueur
            if (obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder)
            {
                // Le joueur passe derrière l'obstacle
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;
            }

            // Ajoute l'obstacle dans la liste
            obstacles.Add(o);
        }
    }
    
    /// <summary>
    /// Détection de fin de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Si le joueur ne touche plus un "Obstacle"
        if (collision.CompareTag("Obstacle"))
        {
            // Référence sur l'obstacle
            Obstacle o = collision.GetComponent<Obstacle>();

            // Retire l'obstacle dans la liste 
            obstacles.Remove(o);

            // S'il n'y a pas d'autres obstacles
            if (obstacles.Count == 0)
            {
                // Le joueur repasse en avant de la scène
                parentRenderer.sortingOrder = defaultOrderInLayer;
            }
            else
            {
                // Tri des obstacles
                obstacles.Sort();

                // Change l'OrderInLayer en fonction du premier obstacle (OrderInLayer le plus petit)
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
        }
    }
}