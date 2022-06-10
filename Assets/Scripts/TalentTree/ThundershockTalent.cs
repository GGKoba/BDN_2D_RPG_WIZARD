/// <summary>
/// Classe de gestion du talent D.O.T. Foudre
/// </summary>
public class ThundershockTalent : Talent
{
    // Débuff de foudre
    ThundershockDebuff debuff = new ThundershockDebuff();

    // Chance de proc du débuff
    private float procChance;

    // Augmentation de la chance de proc du débuff
    private float procChanceIncrease = 5;

    // Description du rang suivant
    private string nextRank = string.Empty;


    /// <summary>
    /// Start
    /// </summary>
    public void Start()
    {
        // Initialise le debuff : Par défaut, la chance de proc est de 5%;
        procChance = 5;
        debuff.MyProcChance = procChance;

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
            debuff.MyProcChance = procChance;

            if (MyCurrentCount < 3)
            {
                // Mise à jour des de la vitesse de réduction pour le rang suivant
                procChance += procChanceIncrease;
                nextRank = $"<color=#CCCCCC>\n\nRang suivant :</color>\n<color=#FFD100>Thunderbolt a {procChance}% de chance d'étourdir\nla cible pendant {debuff.MyDuration} secondes.</color>";
            }
            else
            {
                nextRank = string.Empty;
            }

            // Rafraîchit le tooltip
            UIManager.MyInstance.RefreshTooltip(this);

            // Ajoute l'habilité du talent (Ajoute un débuff sur le sort de foudre)
            SpellBook.MyInstance.GetSpell("Thunderbolt").MyDebuff = debuff;

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
        return $"Thundershock<color=#FFD100>\nThunderbolt a {debuff.MyProcChance}% de chance d'étourdir\nla cible pendant {debuff.MyDuration} secondes.</color>{nextRank}";
    }
}