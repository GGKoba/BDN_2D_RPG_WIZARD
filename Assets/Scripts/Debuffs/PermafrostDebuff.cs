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

    // Temps d'application du débuff
    private float elapsed;


    // Constructeur
    public PermafrostDebuff()
    {
        // Par défaut, le débuff dure 20s
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
}