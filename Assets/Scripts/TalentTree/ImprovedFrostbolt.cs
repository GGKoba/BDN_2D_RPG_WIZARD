/// <summary>
/// Classe de gestion du talent Glace
/// </summary>
public class ImprovedFrostbolt : Talent
{
    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe mère
        if (base.Click())
        {
            // Ajoute l'habilité du talent
            SpellBook.MyInstance.GetSpell("Frostbolt").MyRange += 1;

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
        return string.Format("Improved Frostbolt\n<color=#FFD100>Augmente la portée\nde Frostbolt de 1.</color>");
    }
}