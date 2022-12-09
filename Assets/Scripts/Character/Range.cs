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
        if (collision.CompareTag("Player"))
        {
            // Définit la cible
            enemyScript.SetTarget(collision.GetComponent<Character>());
        }
    }
}
