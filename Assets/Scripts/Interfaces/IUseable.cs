using UnityEngine;



/// <summary>
/// Interface d'utilisation des boutons d'actions
/// </summary>
public interface IUseable
{
    // Propriété d'accès à l'image liée au bouton d'action
    Sprite MyIcon { get; }

    // Fonction d'utilisation (lancement d'un sort, consommation d'un item, ...)
    void Use();
}