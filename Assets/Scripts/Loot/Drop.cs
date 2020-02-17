using UnityEngine;



/// <summary>
/// Classe de gestion de récupération des butins
/// </summary>
public class Drop
{
    // Propriété d'accès sur l'item
    public Item MyItem { get; set; }

    // Propriété d'accès sur la table des butins
    public LootTable MyLootTable { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public Drop(Item item, LootTable lootTable)
    {
        MyItem = item;
        MyLootTable = lootTable;
    }

    /// <summary>
    /// Supprime un objet de la liste des butins
    /// </summary>
    public void Remove()
    {
        MyLootTable.MyDroppedItems.Remove(this);
    }
}
