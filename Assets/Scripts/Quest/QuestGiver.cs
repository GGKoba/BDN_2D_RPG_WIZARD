using UnityEngine;



/// <summary>
/// Classe de gestion du donneur de quêtes
/// </summary>
public class QuestGiver : MonoBehaviour
{
    // Liste des quêtes
    [SerializeField]
    private Quest[] quests = default;

    // [DEBUG] : Référence sur la fenêtre des quêtes du joueur
    [SerializeField]
    private QuestWindow questWindow = default;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // [DEBUG] : Accepte une quête
        questWindow.AcceptQuest(quests[0]);
    }
}