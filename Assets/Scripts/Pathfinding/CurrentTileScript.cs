using UnityEngine;



/// <summary>
/// Classe de gestion de la tile courante
/// </summary>
public class CurrentTileScript : MonoBehaviour
{
    // Personnage
    [SerializeField]
    private Character character;


    // <summary>
    /// Détection de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // S'il y a une collision
        if (collision.CompareTag("Ground"))
        {
            // Assigne la tile courante
            character.MyCurrentTile = collision.transform;
        }
    }
}
