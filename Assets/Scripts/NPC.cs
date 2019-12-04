using UnityEngine;


// Gestion du changement de la valeur de la vie d'un PNJ
public delegate void HealthChanged(float health);

// Gestion de la disparition du personnage
public delegate void CharacterRemoved();


/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux personnage non jouable
/// </summary>
public class NPC : Character
{
    // Evènement de changement de valeur de la vie
    public event HealthChanged HealthChanged;

    // Evènement de disparition du personnage
    public event CharacterRemoved CharacterRemoved;


    // Image de la cible
    [SerializeField]
    private Sprite portrait = default;

    // Propriété d'accès à l'image de la cible
    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }



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
        // Désabonnement de l'évènement de changement de vie
        HealthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);

        // Désabonnement de l'évènement de disparition du personnage
        CharacterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
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
    /// <param name="health">Vie actuelle</param>
    public void OnHealthChanged(float health)
    {
        // S'il existe un abonnement à cet évènement
        if (HealthChanged != null)
        {
            // Déclenchement de l'évènement de changement de la valeur de la vie
            HealthChanged(health);
        }
    }

    /// <summary>
    /// Appelle l'évènement de disparition du personnage
    /// </summary>
    public void OnCharacterRemoved()
    {
        // S'il existe un abonnement à cet évènement
        if (CharacterRemoved != null)
        {
            // Déclenchement de l'évènement de disparition du personnage
            CharacterRemoved();
        }

        // Destruction du personnage
        Destroy(gameObject);
    }
}