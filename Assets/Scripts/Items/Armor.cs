using UnityEngine;



enum ArmorType { Helmet, Shoulders, Chest, Gloves, Feet, MainHand, OffHand, TwoHand };


/// <summary>
/// Classe de gestion des équipements
/// </summary>
[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 1)]
public class Armor : Item
{
    [Header("Armor")]

    // Type d'équipement
    [SerializeField]
    private ArmorType armorType = default;

    // Stat Endurance
    [SerializeField]
    private int stamina = default;

    // Stat Intelligence
    [SerializeField]
    private int intellect = default;

    // Stat Force
    [SerializeField]
    private int strengh = default;


    /// <summary>
    /// Retourne la description de l'item : Surcharge la fonction GetDescription du script Item
    /// </summary>
    public override string GetDescription()
    {
        string stats = string.Empty;

        // Si l'item a une Stat Endurance
        if (stamina > 0)
        {
            stats += string.Format("\nEndurance : <b>{0}</b>", stamina);
        }

        // Si l'item a une Stat Intelligence
        if (intellect > 0)
        {
            stats += string.Format("\nIntelligence : <b>{0}</b>", intellect);
        }

        // Si l'item a une Stat Force
        if (strengh > 0)
        {
            stats += string.Format("\nForce : <b>{0}</b>", strengh);
        }

        // Appelle GetDescription sur la classe mère
        return base.GetDescription() + string.Format("\n{0}", stats);
    }
}