/// <summary>
/// Classe de gestion du talent D.O.T. Glace
/// </summary>
class PermafrostTalent : Talent
{
    // Débuff de glace
    PermafrostDebuff debuff;

    // Valeur de réduction de vitesse
    private float speedReduction = 20;

    // Augmentation de la réducution de vitesse du débuff
    private float reductionIncrease = 20;

    // Description du rang suivant
    private string nextRank = string.Empty;


    /// <summary>
    /// Start
    /// </summary>
    public void Start()
    {
        // Initialise le debuff
        debuff = new PermafrostDebuff(icon);

        // Par défaut, le débuff réduit la vitesse de 20%
        debuff.MySpeedReduction = speedReduction;  
    }

    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe mère
        if (base.Click())
        {
            // Actualise les la vitesse de réduction
            debuff.MySpeedReduction = speedReduction;

            if (MyCurrentCount < 3)
            {
                // Mise à jour des de la vitesse de réduction pour le rang suivant
                speedReduction += reductionIncrease;
                nextRank = $"<color=#CCCCCC>\n\nRang suivant :</color>\n<color=#FFD100>Frostbolt applique un débuff à la cible\nqui réduit sa vitesse de déplacement\nde {speedReduction}% pendant {debuff.MyDuration} secondes.</color>";
            }
            else
            {
                nextRank = string.Empty;
            }

            // Rafraîchit le tooltip
            UIManager.MyInstance.RefreshTooltip(this);

            // Ajoute l'habilité du talent (Ajoute un débuff sur le sort de glace)
            SpellBook.MyInstance.GetSpell("Frostbolt").MyDebuff = debuff;

            // Clic possible
            return true;
        }

        // Clic impossible
        return false;
    }

    /// <summary>
    /// Description du talent : Surcharge la fonction Click du script Talent
    /// </summary>
    /// <returns></returns>
    public override string GetDescription()
    {
        return $"Permafrost<color=#FFD100>\nFrostbolt applique un débuff à la cible\nqui réduit sa vitesse de déplacement\nde {debuff.MySpeedReduction}% pendant {debuff.MyDuration} secondes.</color>{nextRank}";
    }
}