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
}