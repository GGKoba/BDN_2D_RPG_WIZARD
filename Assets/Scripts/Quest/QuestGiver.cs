using UnityEngine;



/// <summary>
/// Classe de gestion du donneur de quêtes
/// </summary>
public class QuestGiver : NPC
{
    // Liste des quêtes
    [SerializeField]
    private Quest[] quests = default;

    // Propriété d'accès à la liste des quêtes
    public Quest[] MyQuests { get => quests; }


    [Header("Questing Status")]

    // Conteneur de l'image
    [SerializeField]
    private SpriteRenderer statusRenderer = default;

    [SerializeField]
    // Image "Quête terminée"
    private Sprite question = default;

    // Image "Quête à rendre"
    [SerializeField]
    private Sprite questionSilver = default;

    // Image "Quête à prendre"
    [SerializeField]
    private Sprite exclamation = default;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Pour chaque quête
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    /// <summary>
    /// Mise à jour du status de la quête
    /// </summary>
    public void UpdateQuestStatus()
    {
        // Pour chaque quête
        foreach (Quest quest in quests)
        {
            // S'il y a une quête
            if (quest != null)
            {
                // Si le joueur a déjà la quête et que la quête est "Terminée"
                if (QuestWindow.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    // Actualise le status de la quête
                    statusRenderer.sprite = question;
                    break;
                }
                // Si le joueur n'a pas la quête
                else if (!QuestWindow.MyInstance.HasQuest(quest))
                {
                    // Actualise le status de la quête
                    statusRenderer.sprite = exclamation;
                    break;
                }
                // Si le joueur a déjà la quête mais qu'elle n'est pas terminée
                else if (QuestWindow.MyInstance.HasQuest(quest) && !quest.IsComplete)
                {
                    // Actualise le status de la quête
                    statusRenderer.sprite = questionSilver;
                }
            }
        }
    }
}