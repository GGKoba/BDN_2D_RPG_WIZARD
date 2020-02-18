using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de gestion de la table des butins de récolte
/// </summary>
public class GatherLootTable : LootTable
{
    // Référence sur le SpriteRenderer
    [SerializeField]
    private SpriteRenderer spriteRenderer = default;

    // Référence sur l'image par défaut
    [SerializeField]
    private Sprite defaultSprite = default;

    // Référence sur l'image de récolte
    [SerializeField]
    private Sprite gatherSprite = default;


    /// <summary>
    /// RollLoot : Surcharge la fonction RollLoot du script LootTable
    /// </summary>
    protected override void RollLoot()
    {
        // Liste des butins
        MyDroppedItems = new List<Drop>();

        foreach (Loot loot in loots)
        {
            // Nombre aléatoire entre 0 et 100
            int roll = Random.Range(0, 100);

            // Si le nombre aléatoire est plus petit que la chance d'obtention de l'item
            if (roll < loot.MyDropChance)
            {
                // Nombre aléatoire entre 1 et 5
                int itemCount = Random.Range(1, 6);

                for (int i = 0; i < itemCount; i++)
                {
                    // Ajoute l'item dans la liste du butin
                    MyDroppedItems.Add(new Drop(loot.MyItem, this));
                }

                // Actualise l'image
                spriteRenderer.sprite = gatherSprite;
            }
            else
            {
                // Masque l'objet
                gameObject.SetActive(false);
            }
        }
    }
}
