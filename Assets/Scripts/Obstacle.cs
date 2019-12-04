using System;
using UnityEngine;



/// <summary>
/// Classe de gestion des obstacles
/// </summary>
public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    public SpriteRenderer MySpriteRenderer { get; set; }


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
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
}
