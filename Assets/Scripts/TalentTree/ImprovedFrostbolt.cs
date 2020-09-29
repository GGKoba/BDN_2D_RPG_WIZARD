using UnityEngine;
using UnityEngine.EventSystems;



/// <summary>
/// Classe de gestion du talent Glace
/// </summary>
public class ImprovedFrostbolt : Talent, IPointerEnterHandler, IPointerExitHandler, IDescribable
{
    // Clic sur le talent : Surcharge la fonction Click du script Talent
    public override bool Click()
    {
        // Appelle Click sur la classe mère
        if (base.Click())
        {
            // Ajoute l'abilité du talent
            SpellBook.MyInstance.GetSpell("Frostbolt").MyRange += 1;

            // Clic possible
            return true;
        }

        // Clic impossible
        return false;
    }

    public string GetDescription()
    {
        return string.Format("Improved Frostbolt\n<color=#FFD100>Augmente la portée \nde Frostbolt de 1.</color>");
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
