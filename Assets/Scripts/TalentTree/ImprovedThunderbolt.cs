/// <summary>
/// Classe de gestion du talent Eclair
/// </summary>
public class ImprovedThunderbolt : Talent
{
    // % d'augmentation
    private int percent = 5;


    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe mère
        if (base.Click())
        {
            // Récupère le sort
            Spell thunderbolt = SpellBook.MyInstance.GetSpell("Thunderbolt");

            // Ajoute l'habilité du talent
            thunderbolt.MyDamage += (thunderbolt.MyDamage / 100) * percent;

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
        return string.Format($"Improved Thunderbolt\n<color=#FFD100>Augmente les dégâts\nde Thunderbolt de { percent }%.</color>");
    }
}