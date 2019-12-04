using UnityEngine;



/// <summary>
/// Classe de gestion de l'ordre des layers
/// </summary>
public class LayerSorter : MonoBehaviour
{
    // SpriteRenderer du joueur
    private SpriteRenderer parentRenderer;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence au SpriteRenderer du joueur
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        
    }


    /// <summary>
    /// Détection de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le joueur touche un "Obstable"
        if (collision.tag == "Obstable")
        {
            // Le joueur passe derrière l'obstacle
            parentRenderer.sortingOrder = collision.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
    }


    /// <summary>
    /// Détection de fin de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Si le joueur ne touche plus un "Obstable"
        if (collision.tag == "Obstable")
        {
            // Le joueur passe derrière l'obstacle
            parentRenderer.sortingOrder = 200;
        }
    }
}
