using System.Collections.Generic;



// Liste des qualités d'un item
public enum Quality { Common, Uncommon, Rare, Epic, Legendary };


/// <summary>
/// Classe statique des couleurs liées à la qualité d'un item
/// </summary>
public static class QualityColor
{
    // Dictionnaire des couleurs des différentes qualités
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        { Quality.Common, "#DDE2E2" },
        { Quality.Uncommon, "#0ED145" },
        { Quality.Rare, "#298EDB" },
        { Quality.Epic, "#9D29DB" },
        { Quality.Legendary, "#FF812B" }
    };


    // Propriété d'accès au disctionnaire des couleurs
    public static Dictionary<Quality, string> MyColors { get => colors; }
}