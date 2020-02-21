using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de gestion de la table des butins de récolte
/// </summary>
public class GatherLootTable : LootTable, IInteractable
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

    // Indicateur sur la minimap
    [SerializeField]
    private GameObject gatherIndicator = default;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        RollLoot();
    }

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

                // Affiche l'indicateur sur la minimap
                gatherIndicator.SetActive(true);
            }
            else
            {
                // Masque l'objet
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Interaction avec le personnage
    /// </summary>
    public void Interact()
    {
        // Lance la routine de récolte
        Player.MyInstance.Gather("Gather", MyDroppedItems);

        // Actualise la référence sur l'élément interactif
        LootWindow.MyInstance.MyInteractable = this;
    }

    /// <summary>
    /// Fin de l'interaction avec le personnage
    /// </summary>
    public void StopInteract()
    {
        // Réinitialise la référence sur l'élément interactif
        LootWindow.MyInstance.MyInteractable = null;

        // S'il n'y a plus d'élements à ramasser
        if (MyDroppedItems.Count == 0)
        {
            // Actualise l'image
            spriteRenderer.sprite = defaultSprite;

            // Masque l'objet
            gameObject.SetActive(false);

            // Masque l'indicateur sur la minimap
            gatherIndicator.SetActive(false);
        }

        // Fermeture de la fenêtre des butins
        LootWindow.MyInstance.Close();
    }
}
