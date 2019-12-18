using UnityEngine;



/// <summary>
/// Interface de déplacement des élements de l'interface
/// </summary>
public interface IMoveable
{
    // Propriété d'accès à l'image liée à l'élement déplaçable
    Sprite MyIcon { get; }
}