/// <summary>
/// Classe de gestion du talent D.O.T. Foudre
/// </summary>
public class ThundershockTalent : Talent
{
    // D�buff de foudre
    ThundershockDebuff debuff = new ThundershockDebuff();

    // Chance de proc du d�buff
    private float procChance;

    // Augmentation de la chance de proc du d�buff
    private float procChanceIncrease = 5;

    // Description du rang suivant
    private string nextRank = string.Empty;


    /// <summary>
    /// Start
    /// </summary>
    public void Start()
    {
        // Initialise le debuff : Par d�faut, la chance de proc est de 5%;
        procChance = 5;
        debuff.MyProcChance = procChance;

    }

    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe m�re
        if (base.Click())
        {
            // Actualise les la vitesse de r�duction
            debuff.MyProcChance = procChance;

            if (MyCurrentCount < 3)
            {
                // Mise � jour des de la vitesse de r�duction pour le rang suivant
                procChance += procChanceIncrease;
                nextRank = $"<color=#CCCCCC>\n\nRang suivant :</color>\n<color=#FFD100>Thunderbolt a {procChance}% de chance d'�tourdir\nla cible pendant {debuff.MyDuration} secondes.</color>";
            }
            else
            {
                nextRank = string.Empty;
            }

            // Rafra�chit le tooltip
            UIManager.MyInstance.RefreshTooltip(this);

            // Ajoute l'habilit� du talent (Ajoute un d�buff sur le sort de foudre)
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
        return $"Thundershock<color=#FFD100>\nThunderbolt a {debuff.MyProcChance}% de chance d'�tourdir\nla cible pendant {debuff.MyDuration} secondes.</color>{nextRank}";
    }
}