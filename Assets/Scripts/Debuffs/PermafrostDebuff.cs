using UnityEngine;



/// <summary>
/// Classe de gestion du Débuff de Glace
/// </summary>
class PermafrostDebuff : Debuff
{
    // Propriété d'accès à la valeur de réduction de vitesse du débuff
    public float MySpeedReduction { get; set; }


    // Propriété d'accès au nom du débuff : Surcharge la propriété MyName du script Debuff
    public override string MyName
    {
        get => "Permafrost";
    }

    // Vitesse de base
    private float originalSpeed;


    // Constructeur
    public PermafrostDebuff()
    {
        // Par défaut, le débuff dure 3s
        MyDuration = 3;
    }

    /// <summary>
    /// Clone le debuff actuel : Surcharge la fonction Clone du script Debuff
    /// </summary>
    public override Debuff Clone()
    {
        // Clone le debuff actuel
        PermafrostDebuff clone = (PermafrostDebuff)this.MemberwiseClone();

        // Retourne le débuff cloné
        return clone;
    }

    /// <summary>
    /// Applique le debuff : Surcharge la fonction Apply du script Debuff
    /// </summary>
    public override void Apply(Character character)
    {
        // Sauvegarde la vitesse d'origine
        originalSpeed = character.MySpeed;

        // Applique le débuff au personnage
        character.MySpeed = character.MySpeed - (character.MySpeed * (MySpeedReduction /100));

        // Appelle Apply sur la classe mère
        base.Apply(character);
    }

    /// <summary>
    /// Retire le debuff : Surcharge la fonction Remove du script Debuff
    /// </summary>
    public override void Remove()
    {
        // Retire le débuff au personnage
        character.MySpeed = originalSpeed;

        // Appelle Remove sur la classe mère
        base.Remove();
    }
}