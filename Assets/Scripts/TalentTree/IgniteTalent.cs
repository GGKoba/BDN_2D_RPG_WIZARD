/// <summary>
/// Classe de gestion du talent D.O.T. Feu
/// </summary>
public class IgniteTalent : Talent
{
    // D�buff de feu
    IgniteDebuff debuff = new IgniteDebuff();

    // D�gats du d�buff
    private float tickDamage = 5;

    // Augmentation des d�gats du d�buff
    private float damageIncrease = 2;

    // Description du rang suivant
    private string nextRank = string.Empty;


    /// <summary>
    /// Start
    /// </summary>
    public void Start()
    {
        // Initialise le debuff : Par d�faut, le d�buff cause 5 points de d�g�ts par tick
        debuff.MyTickDamage = tickDamage;
    }

    /// <summary>
    /// Clic sur le talent : Surcharge la fonction Click du script Talent
    /// </summary>
    public override bool Click()
    {
        // Appelle Click sur la classe m�re
        if (base.Click())
        {
            // Actualise les d�g�ts
            debuff.MyTickDamage = tickDamage;

            if (MyCurrentCount < 3)
            {
                // Mise � jour des d�g�ts pour le rang suivant
                tickDamage += damageIncrease;
                nextRank = $"<color=#CCCCCC>\n\nRang suivant :</color>\n<color=#FFD100>Fireball applique un d�buff\n� la cible qui inflige\n{tickDamage * debuff.MyDuration} points de d�g�ts sur {debuff.MyDuration} secondes.</color>\n<color=red>D�g�ts par secondes : {tickDamage}</color>";
            } 
            else
            {
                nextRank = string.Empty;
            }

            // Rafra�chit le tooltip
            UIManager.MyInstance.RefreshTooltip(this);

            // Ajoute l'habilit� du talent (Ajoute un d�buff sur le sort de feu)
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
        return $"Ignite<color=#FFD100>\nFireball applique un d�buff\n� la cible qui inflige\n{debuff.MyTickDamage * debuff.MyDuration} points de d�g�ts sur {debuff.MyDuration} secondes.</color>\n<color=red>D�g�ts par secondes : {debuff.MyTickDamage}</color>{nextRank}";
    }
}