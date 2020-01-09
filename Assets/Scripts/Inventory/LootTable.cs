using System.Collections.Generic;
using UnityEngine;



public class LootTable : MonoBehaviour
{
    // Tableau des butins
    [SerializeField]
    private Loot[] loots = default;

    // Liste des items du butin
    private List<Item> droppedItems = new List<Item>();

    // L'attribution est-elle déjà faite ?
    private bool rolled = false;


    /// <summary>
    /// Affiche la fenêtre de butin
    /// </summary>
    public void ShowLoots()
    {
        // Si l'attribution n'est pas déjà faite
        if (!rolled)
        {
            // Attribution du butin
            RollLoot();
        }

        // Création de la liste des pages de butin
        LootWindow.MyInstance.CreatePages(droppedItems);
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
                // GG : l'item est dans la liste du butin
                droppedItems.Add(loot.MyItem);
            }
        }

        // Définit l'attribution comme faite
        rolled = true;
    }

}
