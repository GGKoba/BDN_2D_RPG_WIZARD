using UnityEngine;



enum ArmorType { Head, Shoulders, Chest, Hands, Legs, Feet, MainHand, OffHand };


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

    // Propriété d'accès au type d'équipement
    internal ArmorType MyArmorType { get => armorType; }


    /// <summary>
    /// Retourne la description de l'item : Surcharge la fonction GetDescription du script Item
    /// </summary>
    public override string GetDescription()
    {
        string stats = string.Empty;

        // Si l'item a une Stat Endurance
        if (stamina > 0)
        {
            stats += string.Format("\nEndurance : <color=cyan><b>{0}</b></color>", stamina);
        }

        // Si l'item a une Stat Intelligence
        if (intellect > 0)
        {
            stats += string.Format("\nIntelligence : <color=cyan><b>{0}</b></color>", intellect);
        }

        // Si l'item a une Stat Force
        if (strengh > 0)
        {
            stats += string.Format("\nForce : <color=cyan><b>{0}</b></color>", strengh);
        }

        // Appelle GetDescription sur la classe mère et ajoute la description de l'item
        return base.GetDescription() + string.Format("\n<color=#ECECEC>{0}</color>", stats);
    }

    /// <summary>
    /// Equipe l'item
    /// </summary>
    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }
}