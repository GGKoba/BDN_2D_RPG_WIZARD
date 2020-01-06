using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Interface des éléments descriptibles
/// </summary>
public interface IDescribable
{
    // Propriété d'accès au texte de l'élément
    string GetDescription();
}