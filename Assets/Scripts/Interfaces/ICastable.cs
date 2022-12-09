using UnityEngine;



/// <summary>
/// Interface des éléments incantables
/// </summary>
public interface ICastable
{
    // Propriété d'accès au nom
    string MyTitle { get; }

    // Propriété d'accès à l'image
    Sprite MyIcon { get; }

    // Propriété d'accès au temps d'incantation
    float MyCastTime { get; }

    // Propriété d'accès à la couleur de la barre d'incantation
    Color MyBarColor { get; }
}