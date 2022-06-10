using UnityEngine.UI;



/// <summary>
/// Classe de gestion du Débuff de Foudre
/// </summary>
public class ThundershockDebuff : Debuff
{
    // Propriété d'accès à la valeur de réduction de vitesse du débuff
    public float MySpeedReduction { get; set; }


    // Propriété d'accès au nom du débuff : Surcharge la propriété MyName du script Debuff
    public override string MyName
    {
        get => "Thundershock";
    }


    // Constructeur
    public ThundershockDebuff(Image icon) : base(icon)
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
        ThundershockDebuff clone = (ThundershockDebuff)this.MemberwiseClone();

        // Retourne le débuff cloné
        return clone;
    }

    /// <summary>
    /// Applique le debuff : Surcharge la fonction Apply du script Debuff
    /// </summary>
    public override void Apply(Character character)
    {
        // Applique le débuff au personnage : le personnage est stoppé
        (character as Enemy).ChangeState(new StunnedState());

        // Appelle Apply sur la classe mère
        base.Apply(character);
    }

    /// <summary>
    /// Retire le debuff : Surcharge la fonction Remove du script Debuff
    /// </summary>
    public override void Remove()
    {
        // Retire le débuff au personnage : le personnage reprend conscience
        (character as Enemy).ChangeState(new PathState());

        // Appelle Remove sur la classe mère
        base.Remove();
    }
}
