using UnityEngine;


/// <summary>
/// Classe de gestion de la visibilité du personnage
/// </summary>
public class Range : MonoBehaviour
{
    // Référence sur le script Enemy
    private Enemy enemyScript;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }

    /// <summary>
    /// Détection d'entrée en collision
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enemyScript.MyTarget = collision.transform;
        }
    }


    /// <summary>
    /// Détection de sortie de collision
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemyScript.MyTarget = null;
        }
    }

}
