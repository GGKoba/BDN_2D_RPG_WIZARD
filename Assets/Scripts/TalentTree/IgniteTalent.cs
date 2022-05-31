/// <summary>
/// Classe de gestion du talent D.O.T. Feu
/// </summary>
public class IgniteTalent : Talent
{
    // Dégats du débuff
    private float tickDamage = 5;

    // Augmentation des dégats du débuff
    private float damageIncrease = 2;

    // Description du rang suivant
    private string nextRank = string.Empty;

    // Durée du débuff
    private float tmpDuration = 20;

    // Dégats temporaire du débuff
    private float tmpDamage = 5;


    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe mère
        if (base.Click())
        {
            // Actualise les dégâts
            tmpDamage = tickDamage;

            if (MyCurrentCount < 3)
            {
                // Mise à jour des dégâts pour le rang suivant
                tickDamage += damageIncrease;
                nextRank = $"<color=#CCCCCC>\n\nRang suivant :</color>\n<color=#FFD100>Fireball applique un débuff\nà la cible qui inflige\n{tickDamage * tmpDuration} points de dégâts sur {tmpDuration} secondes.</color>\n<color=red>Dégâts par secondes : {tickDamage}</color>";
            } 
            else
            {
                nextRank = string.Empty;
            }

            // Rafraîchit le tooltip
            UIManager.MyInstance.RefreshTooltip(this);

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
        return $"Ignite<color=#FFD100>\nFireball applique un débuff\nà la cible qui inflige\n{tmpDamage * tmpDuration} points de dégâts sur {tmpDuration} secondes.</color>\n<color=red>Dégâts par secondes : {tmpDamage}</color>{nextRank}";
    }
}
