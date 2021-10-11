using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Calsse de gestion des fabrications
/// </summary>
public class Craft : Window
{
    // Instance de classe (singleton)
    private static Craft instance;

    // Propriété d'accès à l'instance
    public static Craft MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type Craft (doit être unique)
                instance = FindObjectOfType<Craft>();
            }

            return instance;
        }
    }

    // Titre de la recette
    [SerializeField]
    private Text title = default;

    // Description de la recette
    [SerializeField]
    private Text description = default;

    // Prefab de la recette
    [SerializeField]
    private GameObject materialPrefab = default;

    // Conteneur de la liste des matériaux de la recette
    [SerializeField]
    private Transform descriptionArea = default;

    // Liste des materiaux de fabrication
    private List<GameObject> materials = new List<GameObject>();

    // Recette séletionnée
    [SerializeField]
    private Recipe selectedRecipe = default;

    // Texte du Nombre fabricable
    [SerializeField]
    private Text countText = default;

    // Script d'informations sur les items
    [SerializeField]
    private ItemInfo craftItemInfo = default;

    // Nombre maximum fabricable
    private int maxAmount;

    // Nombre à fabriquer
    private int amount;

    // Propriété d'accès au nombre à fabriquer
    private int MyAmount
    {
        get { return amount; }
        set
        {
            countText.text = value.ToString();
            amount = value;
        }
    }

    // Liste des nombres à fabriquer
    private List<int> amounts = new List<int>();


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Abonnement sur l'évènement de mise à jour du nombre d'élements de l'item
        InventoryScript.MyInstance.ItemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);

        // Affiche la description de la 1ère recette
        ShowDescription(selectedRecipe);
    }

    /// <summary>
    /// Affiche la description d'une recette de fabrication
    /// </summary>
    /// <param name="quest">Recette sélectionnée</param>
    public void ShowDescription(Recipe recipe)
    {
        // S'il y a une recette
        if (recipe != null)
        {
            // S'il y a déjà une recette sélectionnée différente de la recette à afficher
            if (selectedRecipe != null && selectedRecipe != recipe)
            {
                // Désélectionne la recette
                selectedRecipe.DeSelect();
            }

            // Actualise la recette sélectionnée
            selectedRecipe = recipe;

            // Sélectionne la recette
            selectedRecipe.Select();

            // Réinitialise la liste des matériaux
            foreach (GameObject go in materials)
            {
                Destroy(go);
            }
            materials.Clear();

            // Actualise la titre de la recette
            title.text = recipe.MyCraftedItem.MyTitle;

            // Actualise la description de la recette
            description.text = recipe.MyDescription;

            // Actualise les informations de l'item de la recette
            craftItemInfo.Initialize(recipe.MyCraftedItem, 1);

            // Actualise la liste des matériaux de la recette
            foreach (CraftingMaterial craftingMaterial in recipe.MyMaterials)
            {
                // Instantie un objet "Materiel"
                GameObject material = Instantiate(materialPrefab, descriptionArea);

                // Actualise les informations des matériaux de la recette
                material.GetComponent<ItemInfo>().Initialize(craftingMaterial.MyItem, craftingMaterial.MyCount);

                // Ajoute l'objet dans la liste des matériaux
                materials.Add(material);
            }
        }

        // Actualise le nombre d'éléments
        UpdateMaterialCount(null);
    }

    /// <summary>
    /// Actualise le nombre d'éléments de l'item
    /// </summary>
    /// <param name="item">Item du materiau</param>
    private void UpdateMaterialCount(Item item)
    {
        amounts.Sort();

        // Pour chaque materiaux de la liste de fabrication
        foreach (GameObject material in materials)
        {
            //ItemInfo tmp = material.GetComponent<ItemInfo>();
            //tmp.UpdateStackCount();

            // Mise à jour du nombre d'éléments dans l'inventaire
            material.GetComponent<ItemInfo>().UpdateStackCount();
        }

        if (CanCraft())
        {
            maxAmount = amounts[0];

            // Définit le nombre par défaut
            if (countText.text == "0")
            {
                // Actualise le nombre possible à fabriquer
                MyAmount = 1;
            }
            else if (int.Parse(countText.text) > maxAmount)
            {
                // Actualise le nombre possible à fabriquer
                MyAmount = maxAmount;
            }
        }
        else
        {
            // Actualise le nombre possible à fabriquer
            MyAmount = 0;
            maxAmount = 0;
        }
    }

    /// <summary>
    /// Changement du nombre à fabriquer
    /// </summary>
    /// <param name="i">Nombre à fabriquer</param>
    public void ChangeAmount(int i)
    {
        // Suivant le nombre à fabriquer
        if ((amount + i) > 0 && amount + i <= maxAmount)
        {
            // Actualise le nombre à fabriquer
            MyAmount += i;
        }
    }


    /// <summary>
    /// Fabrique un item
    /// </summary>
    /// <param name="all">Fabriquer TOUT</param>
    public void CraftItem(bool all)
    {
        // [DEBUG] isAttacking pour ne pas pouvoir spam
        if (CanCraft() && !Player.MyInstance.IsAttacking)
        {
            int count;

            if (all)
            {
                amounts.Sort();
                countText.text = maxAmount.ToString();

                // Le nombre à fabriquer devient le nombre maximum à fabriquer 
                count = amounts[0];
            }
            else
            {
                // Le nombre à fabriquer devient le nombre saisi 
                count = MyAmount;
            }

            // Démarre la routine
            StartCoroutine(CraftRoutine(count));
        }
    }

    /// <summary>
    /// Routine de fabrication
    /// </summary>
    /// <param name="castable">Nombre d'item à fabriquer</param>
    private IEnumerator CraftRoutine(int count)
    {
        // Pour le nombre à fabraiquer
        for (int i = 0; i < count; i++)
        {
            // Routine de fabrication
            yield return Player.MyInstance.MyRoutine = StartCoroutine(Player.MyInstance.CraftActionRoutine(selectedRecipe));
        }
    }

    /// <summary>
    /// Ajout d'item(s) dans l'inventaire
    /// </summary>
    public void AddItemsToInventory()
    {
        // Si on peut ajouter dans l'inventaire
        if (InventoryScript.MyInstance.AddItem(craftItemInfo.MyItem))
        {
            // Pour chaque item de fabrication 
            foreach (CraftingMaterial material in selectedRecipe.MyMaterials)
            {
                // Pour chaque élément nécessaire
                for (int i = 0; i < material.MyCount; i++)
                {
                    InventoryScript.MyInstance.RemoveItem(material.MyItem);
                }
            }
        }
    }

    private bool CanCraft()
    {
        bool canCraft = true;
        amounts = new List<int>();

        // Pour chaque item de fabrication 
        foreach (CraftingMaterial material in selectedRecipe.MyMaterials)
        {
            // Nombre d'éléments
            int count = InventoryScript.MyInstance.GetItemCount(material.MyItem.MyKey);

            if (count < material.MyCount)
            {
                canCraft = false;
                break;
            }
            else
            {
                // Actualise le nombre possible à fabriquer
                amounts.Add(count / material.MyCount);
                continue;
            }
        }

        return canCraft;
    }
}
