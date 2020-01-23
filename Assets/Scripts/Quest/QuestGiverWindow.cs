using UnityEngine;
using UnityEngine.UI;



public class QuestGiverWindow : Window
{
    // Référence sur le donneur de quêtes
    private QuestGiver questGiver;

    // Préfab des quêtes du donneur de quêtes
    [SerializeField]
    private GameObject questPrefab = default;

    // Conteneur de la liste des quêtes
    [SerializeField]
    private Transform questParent = default;


    /// <summary>
    /// Affiche la liste des quêtes
    /// </summary>
    /// <param name="questGiverRef">Donneur de quêtes</param>
    public void ShowQuests(QuestGiver questGiverRef)
    {
        // Référence sur le donneur de quêtes 
        questGiver = questGiverRef;

        foreach (Quest quest in questGiver.MyQuests)
        {
            // Instantie un objet "Quête"
            GameObject go = Instantiate(questPrefab, questParent);

            // Actualise le titre de la quête
            go.GetComponent<Text>().text = quest.MyTitle;
        }
    }


    /// <summary>
    /// Open : Surcharge la fonction Open du script Window
    /// </summary>
    /// <param name="npcRef"></param>
    public override void Open(NPC npcRef)
    {
        // Affiche la liste des quêtes
        ShowQuests((npcRef as QuestGiver));

        // Appelle Open sur la classe mère
        base.Open(npcRef);
    }
}
