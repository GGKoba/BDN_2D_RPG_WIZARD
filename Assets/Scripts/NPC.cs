using UnityEngine;


// Gestion du changement de la valeur de la vie d'un PNJ
public delegate void HealthChanged(float health);


/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux personnage non jouable
/// </summary>
public class NPC : Character
{
    // Evènement de changement de valeur de la vie
    public event HealthChanged HealthChanged;


    /// <summary>
    /// Start : Ecrase la fonction Update du script Character
    /// </summary>
    protected override void Start()
    {
        // Appelle Start sur la classe mère (abstraite)
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

    /// <summary>
    /// Appelle l'évènement de changement de valeur de la vie
    /// </summary>
    public void OnHealthChanged(float health)
    {
        // S'il existe un abonnement à cet évènement
        if (HealthChanged != null)
        {
            // Déclenchement de l'évènement de changement de la valeur de la vie
            HealthChanged(health);
        }
    }
}