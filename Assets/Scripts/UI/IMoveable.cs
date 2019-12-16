using UnityEngine;



/// <summary>
/// Interface de déplacement des élements de l'interface
/// </summary>
public interface IMoveable
{
    // Propriété d'accès à l'icône liée à l'élement déplaçable
    Sprite MyIcon { get; }
}