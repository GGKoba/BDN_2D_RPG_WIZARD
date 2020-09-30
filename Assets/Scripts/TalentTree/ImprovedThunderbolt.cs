using UnityEngine;
using UnityEngine.EventSystems;



/// <summary>
/// Classe de gestion du talent Eclair
/// </summary>
public class ImprovedThunderbolt : Talent, IPointerEnterHandler, IPointerExitHandler, IDescribable
{
    // % d'augmentation
    private int percent = 5;


    // Clic sur le talent : Surcharge la fonction Click du script Talent
    public override bool Click()
    {
        // Appelle Click sur la classe mère
        if (base.Click())
        {
            Spell thunderbolt = SpellBook.MyInstance.GetSpell("Thunderbolt");

            // Ajoute l'abilité du talent
            thunderbolt.MyDamage += (thunderbolt.MyDamage / 100) * percent;

            // Clic possible
            return true;
        }

        // Clic impossible
        return false;
    }

    public string GetDescription()
    {
        return string.Format($"Improved Thunderbolt\n<color=#FFD100>Augmente les dégâts \nde Thunderbolt de { percent }%.</color>");
    }

    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Affiche le tooltip
        UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, this);
    }

    /// <summary>
    /// Sortie du curseur
    /// </summary>
    /// <param name="eventData">Evenement de sortie</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Masque le tooltip
        UIManager.MyInstance.HideTooltip();
    }
}