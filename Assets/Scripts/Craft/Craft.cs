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
        // Pour chaque materiaux de la liste de fabrication
        foreach (GameObject material in materials)
        {
            //ItemInfo tmp = material.GetComponent<ItemInfo>();
            //tmp.UpdateStackCount();

            // Mise à jour du nombre d'éléments dans l'inventaire
            material.GetComponent<ItemInfo>().UpdateStackCount();
        }
    }

    /// <summary>
    /// Fabrique un item
    /// </summary>
    public void CraftItem()
    {
        // Démarre la routine
        StartCoroutine(CraftRoutine(0));
    }

    /// <summary>
    /// Routine de fabrication
    /// </summary>
    /// <param name="castable">Nombre d'item à fabriquer</param>
    private IEnumerator CraftRoutine(int count)
    {
        // Routine de fabrication
        yield return Player.MyInstance.MyRoutine = StartCoroutine(Player.MyInstance.CraftActionRoutine(selectedRecipe));
    }

    /// <summary>
    /// Ajout d'item(s) dans l'inventaire
    /// </summary>
    public void AddItemsToInventory()
    {
        // Ajoute dans l'inventaire
        InventoryScript.MyInstance.AddItem(craftItemInfo.MyItem);
    }
}
