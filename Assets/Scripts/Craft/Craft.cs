using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Calsse de gestion des fabrications
/// </summary>
public class Craft : MonoBehaviour
{
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

       /*
       // S'il y a une recette
       if (recipe != null)
       {
           // S'il y a déjà une quête sélectionnée différente de la quête à afficher
           if (selected != null && selected != quest)
           {
               // Désélectionne la quête
               selected.MyQuestScript.DeSelect();
           }

           // Actualise la quête sélectionnée
           selected = quest;

           // Description de la quête
           string description = quest.GetDescription();

           // Ajout des éventuels objectifs
           if (quest.MyCollectObjectives.Length > 0 || quest.MyKillObjectives.Length > 0)
           {
               string objectivesText = string.Format("\n\n<color=#468FC1>Objectifs</color>\n");

               // Pour chaque objectif de collecte
               foreach (Objective collectObjective in quest.MyCollectObjectives)
               {
                   objectivesText += string.Format("<color=#FFFFF><size=12><i>{0} : {1}/{2}</i></size></color>\n", collectObjective.MyType, collectObjective.MyCurrentAmount, collectObjective.MyAmount);
               }

               // Pour chaque objectif d'ennemi
               foreach (Objective killObjective in quest.MyKillObjectives)
               {
                   objectivesText += string.Format("<color=#FFFFF><size=12><i>{0} : {1}/{2}</i></size></color>\n", killObjective.MyType, killObjective.MyCurrentAmount, killObjective.MyAmount);
               }

               // Ajoute les objectifs à la description
               description += objectivesText;
           }

           // Actualise la description de la quête sélectionnée
           questDescription.text = description;
       }
       */
    }
}
