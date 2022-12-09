using UnityEngine;



/// <summary>
/// Classe de gestion des pommes
/// </summary>
[CreateAssetMenu(fileName = "Apple", menuName = "Items/Apple", order = 1)]
public class Apple : Item
{
    /// <summary>
    /// Retourne la description de l'item : Surcharge la fonction GetDescription du script Item
    /// </summary>
    public override string GetDescription()
    {
        // Appelle GetDescription sur la classe mère et ajoute la description de l'item
        return base.GetDescription() + string.Format("\n\n<color=#ECECEC>Une bonne pomme !!</color>");
    }
}
