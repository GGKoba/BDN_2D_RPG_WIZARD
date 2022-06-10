/// <summary>
/// Classe de gestion du talent D.O.T. Feu
/// </summary>
public class IgniteTalent : Talent
{
    // Débuff de feu
    IgniteDebuff debuff = new IgniteDebuff();

    // Dégats du débuff
    private float tickDamage = 5;

    // Augmentation des dégats du débuff
    private float damageIncrease = 2;

    // Description du rang suivant
    private string nextRank = string.Empty;


    /// <summary>
    /// Start
    /// </summary>
    public void Start()
    {
        // Initialise le debuff : Par défaut, le débuff cause 5 points de dégâts par tick
        debuff.MyTickDamage = tickDamage;
    }

    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe mère
        if (base.Click())
        {
            // Actualise les dégâts
            debuff.MyTickDamage = tickDamage;

            if (MyCurrentCount < 3)
            {
                // Mise à jour des dégâts pour le rang suivant
                tickDamage += damageIncrease;
                nextRank = $"<color=#CCCCCC>\n\nRang suivant :</color>\n<color=#FFD100>Fireball applique un débuff\nà la cible qui inflige\n{tickDamage * debuff.MyDuration} points de dégâts sur {debuff.MyDuration} secondes.</color>\n<color=red>Dégâts par secondes : {tickDamage}</color>";
            } 
            else
            {
                nextRank = string.Empty;
            }

            // Rafraîchit le tooltip
            UIManager.MyInstance.RefreshTooltip(this);

            // Ajoute l'habilité du talent (Ajoute un débuff sur le sort de feu)
            SpellBook.MyInstance.GetSpell("Fireball").MyDebuff = debuff;

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
        return $"Ignite<color=#FFD100>\nFireball applique un débuff\nà la cible qui inflige\n{debuff.MyTickDamage * debuff.MyDuration} points de dégâts sur {debuff.MyDuration} secondes.</color>\n<color=red>Dégâts par secondes : {debuff.MyTickDamage}</color>{nextRank}";
    }
}