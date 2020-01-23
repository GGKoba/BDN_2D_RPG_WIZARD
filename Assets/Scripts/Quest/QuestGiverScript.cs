using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion du donneur de quêtes
/// </summary>
public class QuestGiverScript : MonoBehaviour
{
    // Propriété d'accès à l'objet "Quête"
    public Quest MyQuest { get; set; }


    /// <summary>
    /// Sélectionne une quête
    /// </summary>
    public void Select()
    {
        // Affiche la description de la quête sélectionnée
        QuestGiverWindow.MyInstance.ShowQuestInfo(MyQuest);
    }
}
