using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Clase de gestion de la fenêtre du donneur de quêtes
/// </summary>
public class QuestGiverWindow : Window
{
    // Instance de classe (singleton)
    private static QuestGiverWindow instance;

    // Propriété d'accès à l'instance
    public static QuestGiverWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type QuestGiverWindow (doit être unique)
                instance = FindObjectOfType<QuestGiverWindow>();
            }

            return instance;
        }
    }

    // Liste des quêtes
    private List<GameObject> quests = new List<GameObject>();

    // Référence sur le donneur de quêtes
    private QuestGiver questGiver;

    // Quête sélectionnée
    private Quest selected;

    // Préfab des quêtes du donneur de quêtes
    [SerializeField]
    private GameObject questPrefab = default;

    // Conteneur de la liste des quêtes
    [SerializeField]
    private Transform questArea = default;

    // Conteneur de la description de la quête
    [SerializeField]
    private GameObject questDescription = default;

    [Header("Boutons")]
    // Référence sur le bouton "Accepter"
    [SerializeField]
    private GameObject acceptButton = default;

    // Référence sur le bouton "Retour"
    [SerializeField]
    private GameObject backButton = default;

    // Référence sur le bouton "Terminer"
    [SerializeField]
    private GameObject completeButton = default;


    /// <summary>
    /// Affiche la liste des quêtes
    /// </summary>
    /// <param name="questGiverRef">Donneur de quêtes</param>
    public void ShowQuests(QuestGiver questGiverRef)
    {
        // Référence sur le donneur de quêtes 
        questGiver = questGiverRef;

        // Pour chaque quête de la liste
        foreach (GameObject questObject in quests)
        {
            // Detruit la quête
            Destroy(questObject);
        }

        // Pour chaque quête de la liste
        foreach (Quest quest in questGiver.MyQuests)
        {
            // Si la quête n'est pas nulle
            if (quest != null)
            {
                // Instantie un objet "Quête"
                GameObject go = Instantiate(questPrefab, questArea);

                // Actualise le titre de la quête
                go.GetComponent<Text>().text = quest.MyTitle;

                // Référence sur la quête
                go.GetComponent<QuestGiverScript>().MyQuest = quest;

                // Ajoute la quête dans la liste
                quests.Add(go);

                // Si le joueur a deja la quête et que la quête est "Terminée"
                if (QuestWindow.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    // Actualise le titre de la quête
                    go.GetComponent<Text>().text += " (Terminée)";
                }
                // Si le joueur a deja la quête
                else if (QuestWindow.MyInstance.HasQuest(quest))
                {
                    Color color = go.GetComponent<Text>().color;

                    // Actualise la transparence de la couleur de la quête
                    color.a = 0.5f;

                    // Actualise la couleur du texte
                    go.GetComponent<Text>().color = color;
                }
            }
        }

        // Masque la description
        questDescription.SetActive(false);

        // Affiche la liste des quêtes
        questArea.gameObject.SetActive(true);
    }

    /// <summary>
    /// Open : Surcharge la fonction Open du script Window
    /// </summary>
    /// <param name="npcRef"></param>
    public override void Open(NPC npcRef)
    {
        // Masque la description
        questDescription.SetActive(false);

        // Affiche la liste des quêtes
        ShowQuests((npcRef as QuestGiver));

        // Appelle Open sur la classe mère
        base.Open(npcRef);
    }

    /// <summary>
    /// Close : Surcharge la fonction Close du script Window
    /// </summary>
    public override void Close()
    {
        // Masque le bouton Terminer
        completeButton.SetActive(false);

        // Appelle Close sur la classe mère
        base.Close();
    }

    /// <summary>
    /// Affiche les informations d'une quête
    /// </summary>
    /// <param name="quest">Quête affichée</param>
    public void ShowQuestInfo(Quest quest)
    {
        // S'il y a une quête
        if (quest != null)
        {
            // Si le joueur a deja la quête et que la quête est "Terminée"
            if (QuestWindow.MyInstance.HasQuest(quest) && quest.IsComplete)
            {
                // Masque le bouton Accepter
                acceptButton.SetActive(false);

                // Affiche le bouton Terminer
                completeButton.SetActive(true);
            }
            // Si le joueur n'a pas la quête
            else if (!QuestWindow.MyInstance.HasQuest(quest))
            {
                // Affiche le bouton Accepter
                acceptButton.SetActive(true);
            }

            // Affiche le bouton Retour
            backButton.SetActive(true);

            // Masque la liste des quêtes
            questArea.gameObject.SetActive(false);

            // Actualise la description de la quête sélectionnée
            questDescription.GetComponent<Text>().text = quest.GetDescription();

            // Affiche la description
            questDescription.SetActive(true);

            /*
            // S'il y a déjà une quête sélectionnée différente de la quête à afficher
            if (selected != null && selected != quest)
            {
                // Désélectionne la quête
                selected.MyQuestScript.DeSelect();
            }
            */
            // Actualise la quête sélectionnée
            selected = quest;

        }
    }

    /// <summary>
    /// Clic sur le bouton Accepter : Accepte une quête
    /// </summary>
    public void AcceptQuest()
    {
        // Ajoute la quête à la liste du joueur
        QuestWindow.MyInstance.AcceptQuest(selected);

        // Retourne à la liste des quêtes
        Back();
    }

    /// <summary>
    /// Clic sur le bouton Retour : Retourne à la liste des quêtes
    /// </summary>
    public void Back()
    {
        // Masque des boutons
        acceptButton.SetActive(false);
        backButton.SetActive(false);
        completeButton.SetActive(false);

        // Affiche la liste des quêtes
        ShowQuests(questGiver);
    }

    /// <summary>
    /// Clic sur le bouton Terminer : Termine la quête
    /// </summary>
    public void CompleteQuest()
    {
        // Si la quête sélectionnée est "Terminée"
        if (selected.IsComplete)
        {
            // Pour chaque quête
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                // Si la quête sélectionnée est la même que celle du donneur de quêtes
                if (selected == questGiver.MyQuests[i])
                {
                    // Retire la quête de la liste
                    questGiver.MyQuests[i] = null;
                }
            }

            // Retourne à la liste des quêtes
            Back();
        }
    }

}
