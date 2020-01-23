using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des quêtes
/// </summary>
public class QuestScript : MonoBehaviour
{
    // Propriété d'accès à l'objet "Quête"
    public Quest MyQuest { get; set; }

    // Définit si la quête est marquée comme "Terminée"
    private bool markedComplete = false;


    /// <summary>
    /// Sélectionne une quête
    /// </summary>
    public void Select()
    {
        // Change la couleur de la quête sélectionnée
        GetComponent<Text>().color = Color.cyan;

        // Affiche la description de la quête sélectionnée
        QuestWindow.MyInstance.ShowDescription(MyQuest);
    }

    /// <summary>
    /// Désélectionne une quête
    /// </summary>
    public void DeSelect()
    {
        // Réinitialise la couleur de la quête
        GetComponent<Text>().color = Color.white;
    }

    /// <summary>
    /// Flag la quête comme "Terminée"
    /// </summary>
    public void IsComplete()
    {
        // Si la quête est achevée et n'est pas marquée comme "Terminée"
        if (MyQuest.IsComplete && !markedComplete)
        {
            // Actualise le titre de la quête
            GetComponent<Text>().text += " (Terminée)";

            // Marque la quête comme "Terminée"
            markedComplete = true;
        }
        // Si la quête n'est pas achevée
        else if (!MyQuest.IsComplete)
        {
            // Réinitialise le titre de la quête
            GetComponent<Text>().text = MyQuest.MyTitle;

            // Marque la quête comme "Non Terminée"
            markedComplete = false;
        }
    }
}