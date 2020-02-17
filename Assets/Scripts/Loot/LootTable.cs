using System.Collections.Generic;
using UnityEngine;



public class LootTable : MonoBehaviour
{
    // Tableau des butins
    [SerializeField]
    private Loot[] loots = default;

    // Propriété d'accès à la liste des items du butin
    public List<Drop> MyDroppedItems { get; set; }

    // L'attribution est-elle déjà faite ?
    private bool rolled = false;


    /// <summary>
    /// Liste des butins
    /// </summary>
    public List<Drop> GetLoots()
    {
        // Si l'attribution n'est pas déjà faite
        if (!rolled)
        {
            // Liste des butins
            MyDroppedItems = new List<Drop>();

            // Attribution du butin
            RollLoot();
        }

        // Retourne la liste des pages de butin
        return MyDroppedItems;
    }

    // Attribution des loots 
    private void RollLoot()
    {
        foreach (Loot loot in loots)
        {
            // Nombre aléatoire entre 0 et 100
            int roll = Random.Range(0, 100);

            // Si le nombre aléatoire est plus petit que la chance d'obtention de l'item
            if (roll < loot.MyDropChance)
            {
                // Ajoute l'item dans la liste du butin
                MyDroppedItems.Add(new Drop(loot.MyItem, this));
            }
        }

        // Définit l'attribution comme faite
        rolled = true;
    }

}
