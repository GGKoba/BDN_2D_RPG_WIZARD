using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface des éléments cliquables de l'interface
/// </summary>
public interface IClickable
{
    // Propriété d'accès à l'image liée à l'élément cliquable
    Image MyIcon { get; set; }

    // Propriété d'accès au nombre d'élément de l'emplacement
    int MyCount { get; }
}
