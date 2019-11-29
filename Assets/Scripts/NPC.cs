using UnityEngine;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux personnage non jouable
/// </summary>
public class NPC : Character
{
    /// <summary>
    /// Start
    /// </summary>
    protected override void Start()
    {
        // Appel Start sur la classe mère (abstraite)
        base.Start();
    }

    /// <summary>
    /// Désélection d'un NPC : virtual pour être écrasée pour les sous-classes
    /// </summary>
    public virtual void DeSelect()
    {

    }

    /// <summary>
    /// Sélection d'un NPC : virtual pour être écrasée pour les sous-classes
    /// </summary>
    public virtual Transform Select()
    {
        // Retourne la référence à la HitBox du personnage
        return hitBox;
    }
}