using UnityEngine.UI;



/// <summary>
/// Classe de gestion du D�buff de Foudre
/// </summary>
public class ThundershockDebuff : Debuff
{
    // Propri�t� d'acc�s � la valeur de r�duction de vitesse du d�buff
    public float MySpeedReduction { get; set; }


    // Propri�t� d'acc�s au nom du d�buff : Surcharge la propri�t� MyName du script Debuff
    public override string MyName
    {
        get => "Thundershock";
    }


    // Constructeur
    public ThundershockDebuff(Image icon) : base(icon)
    {
        // Par d�faut, le d�buff dure 3s
        MyDuration = 3;
    }

    /// <summary>
    /// Clone le debuff actuel : Surcharge la fonction Clone du script Debuff
    /// </summary>
    public override Debuff Clone()
    {
        // Clone le debuff actuel
        ThundershockDebuff clone = (ThundershockDebuff)this.MemberwiseClone();

        // Retourne le d�buff clon�
        return clone;
    }

    /// <summary>
    /// Applique le debuff : Surcharge la fonction Apply du script Debuff
    /// </summary>
    public override void Apply(Character character)
    {
        // Applique le d�buff au personnage : le personnage est stopp�
        (character as Enemy).ChangeState(new StunnedState());

        // Appelle Apply sur la classe m�re
        base.Apply(character);
    }

    /// <summary>
    /// Retire le debuff : Surcharge la fonction Remove du script Debuff
    /// </summary>
    public override void Remove()
    {
        // Retire le d�buff au personnage : le personnage reprend conscience
        (character as Enemy).ChangeState(new PathState());

        // Appelle Remove sur la classe m�re
        base.Remove();
    }
}
