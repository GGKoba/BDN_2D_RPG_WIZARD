using UnityEngine;



/// <summary>
/// Classe de gestion des potions de vie
/// </summary>
[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUseable
{
    [Header("HealthPotion")]

    // Points de vie de la potion
    [SerializeField]
    private int health = default;


    /// <summary>
    /// Utilisation de la potion de vie
    /// </summary>
    public void Use()
    {
        // Si le joueur a besoin de vie
        if (Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            // Supprime l'item de l'emplacement
            Remove();

            // Ajoute de la vie au joueur
            Player.MyInstance.MyHealth.MyCurrentValue += health;
        }
    }

    /// <summary>
    /// Retourne la description de l'item : Surcharge la fonction GetDescription du script Item
    /// </summary>
    public override string GetDescription()
    {
        // Appelle GetDescription sur la classe mère et ajoute la description de l'item
        return base.GetDescription() + string.Format("\n\n<color=#ECECEC><color=#FFF2BE>Utiliser :</color> Restaure <color=cyan>{0}</color> points de vie</color>", health);
    }
}
