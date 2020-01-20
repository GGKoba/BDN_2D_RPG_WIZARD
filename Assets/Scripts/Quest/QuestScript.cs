using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des quêtes
/// </summary>
public class QuestScript : MonoBehaviour
{
    // Propriété d'accès à l'objet "Quête"
    public Quest MyQuest { get; set; }


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {

    }

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
}