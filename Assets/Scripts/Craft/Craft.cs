using UnityEngine;


/// <summary>
/// Calsse de gestion des fabrications
/// </summary>
public class Craft : MonoBehaviour
{
    // Recctte séletionnée
    private Recipe selectedRecipe;



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
