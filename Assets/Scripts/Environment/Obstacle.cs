using System;
using UnityEngine;



/// <summary>
/// Classe de gestion des obstacles
/// </summary>
public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    // Propriété d'accès au SpriteRenderer de l'obstacle
    public SpriteRenderer MySpriteRenderer { get; set; }

    // Couleur de l'obstacle
    private Color defaultColor;

    // Couleur pour le Fade
    private Color fadedColor;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence au SpriteRenderer de l'obstacle
        MySpriteRenderer = GetComponent<SpriteRenderer>();

        // Référence sur la couleur de l'obstacle
        defaultColor = MySpriteRenderer.color;

        // Initialisation de la fadedColor
        fadedColor = defaultColor;

        // Mise à jour de l'alpha de la fadedColor
        fadedColor.a = 0.7f;
    }

    /// <summary>
    /// Fonction de comparaison des Obstacles : Tri sur le OrderInLayer
    /// </summary>
    /// <param name="other">Obstacle de comparaison</param>
    public int CompareTo(Obstacle other)
    {
        // Si l'obstacle courant a un OrderInLayer plus grand que l'obstacle de comparaison 
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;
        }
        // Si l'obstacle courant a un OrderInLayer plus petit que l'obstacle de comparaison 
        else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;
        }

        // Si les 2 obstacles ont le même OrderInLayer 
        return 0;
    }

    /// <summary>
    /// La couleur de l'obstacle devient fadedColor
    /// </summary>
    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }

    /// <summary>
    /// La couleur de l'obstacle devient defaultColor
    /// </summary>
    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }



    /// <summary>
    /// Détection de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Collision avec le joueur
        if (collision.name == "OstacleCollider")
        {
            // Masque partiellement l'obstacle
            FadeOut();
        }
    }

    /// <summary>
    /// Détection de fin de collision du joueur avec des obstables
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        // Collision avec le joueur
        if (collision.name == "OstacleCollider")
        {
            // Affiche l'obstacle
            FadeIn();
        }
    }
}
