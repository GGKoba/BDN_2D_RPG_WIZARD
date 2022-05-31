/// <summary>
/// Classe de gestion du talent Feu
/// </summary>
public class ImprovedFireball : Talent
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
            SpellBook.MyInstance.GetSpell("Fireball").MyCastTime -= 0.1f;

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
        return string.Format("Improved Fireball\n<color=#FFD100>Réduit le temps d'incantation\nde Fireball de 0,1s.</color>");
    }
}