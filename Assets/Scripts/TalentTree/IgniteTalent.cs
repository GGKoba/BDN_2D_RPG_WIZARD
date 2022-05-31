/// <summary>
/// Classe de gestion du talent D.O.T. Feu
/// </summary>
public class IgniteTalent : Talent
{
    // D�gats du d�buff
    private float tickDamage = 5;

    // Augmentation des d�gats du d�buff
    private float damageIncrease = 2;

    // Description du rang suivant
    private string nextRank = string.Empty;

    // Dur�e du d�buff
    private float tmpDuration = 20;

    // D�gats temporaire du d�buff
    private float tmpDamage = 5;


    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe m�re
        if (base.Click())
        {
            // Actualise les d�g�ts
            tmpDamage = tickDamage;

            if (MyCurrentCount < 3)
            {
                // Mise � jour des d�g�ts pour le rang suivant
                tickDamage += damageIncrease;
                nextRank = $"<color=#CCCCCC>\n\nRang suivant :</color>\n<color=#FFD100>Fireball applique un d�buff\n� la cible qui inflige\n{tickDamage * tmpDuration} points de d�g�ts sur {tmpDuration} secondes.</color>\n<color=red>D�g�ts par secondes : {tickDamage}</color>";
            } 
            else
            {
                nextRank = string.Empty;
            }

            // Rafra�chit le tooltip
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
        return $"Ignite<color=#FFD100>\nFireball applique un d�buff\n� la cible qui inflige\n{tmpDamage * tmpDuration} points de d�g�ts sur {tmpDuration} secondes.</color>\n<color=red>D�g�ts par secondes : {tmpDamage}</color>{nextRank}";
    }
}
