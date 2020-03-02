using UnityEngine;


/// <summary>
/// Classe de gestion des minerais d'or
/// </summary>
[CreateAssetMenu(fileName = "GoldNugget", menuName = "Items/GoldNugget", order = 1)]
public class GoldNugget : Item
{
    /// <summary>
    /// Retourne la description de l'item : Surcharge la fonction GetDescription du script Item
    /// </summary>
    public override string GetDescription()
    {
        // Appelle GetDescription sur la classe mère et ajoute la description de l'item
        return base.GetDescription() + string.Format("\n\n<color=#ECECEC>Un minerai d'or !!</color>");
    }
}