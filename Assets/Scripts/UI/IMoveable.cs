using UnityEngine;



/// <summary>
/// Interface de déplacement des éléments de l'interface
/// </summary>
public interface IMoveable
{
    // Propriété d'accès à l'image liée à l'élément déplaçable
    Sprite MyIcon { get; }
}