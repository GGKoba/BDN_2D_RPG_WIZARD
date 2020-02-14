using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de la fenêtre des quêtes du joueur
/// </summary>
public class QuestWindow : Window
{
    // Instance de classe (singleton)
    private static QuestWindow instance;

    // Propriété d'accès à l'instance
    public static QuestWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type QuestWindow (doit être unique)
                instance = FindObjectOfType<QuestWindow>();
            }

            return instance;
        }
    }

    // Quête sélectionnée
    private Quest selected;

    // Prefab de l'objet "Quête"
    [SerializeField]
    private GameObject questPrefab = default;

    // Conteneur de la liste des quêtes
    [SerializeField]
    private Transform questArea = default;

    // Conteneur de la description de la quête
    [SerializeField]
    private Text questDescription = default;

    // Liste des scripts des quêtes
    private readonly List<QuestScript> questScripts = new List<QuestScript>();

    // Liste des quêtes
    private List<Quest> quests = new List<Quest>();

    // Propriété d'accès à la liste des quêtes
    public List<Quest> MyQuests { get => quests; set => quests = value; }

    // Texte du compteur de quêtes
    [SerializeField]
    private Text questCount = default;

    // Nombre maximum de quêtes
    [SerializeField]
    private int maxCount = default;


    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // Actualise le compteur du nombre de quêtes
        questCount.text = quests.Count + "/" + maxCount;
    }

    /// <summary>
    /// Accepte une quête
    /// </summary>
    /// <param name="quest">Quête à accepter</param>
    public void AcceptQuest(Quest quest)
    {
        // Si le nombre de quête maximum n'est pas atteint
        if (quests.Count < maxCount)
        {
            // Pour chacun des objectifs de collecte de la quête
            foreach (CollectObjective collectObjective in quest.MyCollectObjectives)
            {
                // Abonnement sur l'évènement de mise à jour du nombre d'élements de l'item
                InventoryScript.MyInstance.ItemCountChangedEvent += new ItemCountChanged(collectObjective.UpdateItemCount);

                // Actualise le nombre d'items
                collectObjective.UpdateItemCount();
            }

            // Pour chacun des objectifs d'ennemi de la quête
            foreach (KillObjective killObjective in quest.MyKillObjectives)
            {
                // Abonnement sur l'évènement de la mort d'un personnage
                GameManager.MyInstance.KillConfirmedEvent += new KillConfirmed(killObjective.UpdateKillCount);
            }

            // Instantie un objet "Quête"
            GameObject go = Instantiate(questPrefab, questArea);

            // Actualise le titre de la quête
            go.GetComponent<Text>().text = quest.MyTitle;

            // Script lié à l'objet "Quête"
            QuestScript questScript = go.GetComponent<QuestScript>();

            // Référence sur le script de la quête
            quest.MyQuestScript = questScript;

            // Référence sur la quête
            questScript.MyQuest = quest;

            // Ajoute la quête dans la liste
            quests.Add(quest);
            questScripts.Add(questScript);

            // Vérifie la complétude des quêtes de la liste
            CheckCompletion();
        }
    }

    /// <summary>
    /// Affiche la description d'une quête
    /// </summary>
    /// <param name="quest">Quête affichée</param>
    public void ShowDescription(Quest quest)
    {
        // S'il y a une quête
        if (quest != null)
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
    }

    /// <summary>
    /// Actualise la quête
    /// </summary>
    public void UpdateSelected()
    {
        //Affiche la description d'une quête
        ShowDescription(selected);           
    }

    /// <summary>
    /// Vérifie la complétude des quêtes de la liste
    /// </summary>
    public void CheckCompletion()
    {
        // Pour chaque quête
        foreach (QuestScript questScript in questScripts)
        {
            // Actualise le status de la quête
            questScript.MyQuest.MyQuestGiver.UpdateQuestStatus();

            // Vérifie si la quête est terminée
            questScript.IsComplete();
        }
    }

    /// <summary>
    /// Existence de la quête dans la liste des quêtes
    /// </summary>
    /// <param name="quest">Quête recherchée</param>
    /// <returns></returns>
    public bool HasQuest(Quest quest)
    {
        return quests.Exists(q => q.MyTitle == quest.MyTitle);
    }

    /// <summary>
    /// Retire une quête
    /// </summary>
    /// <param name="qs">Script lié à la quête</param>
    public void RemoveQuest(QuestScript qs)
    {
        // Retire le script de la quête de la liste des scripts des quêtes
        questScripts.Remove(qs);
        
        // Detruit l'objet
        Destroy(qs.gameObject);

        // Retire la quête de la liste des quêtes
        quests.Remove(qs.MyQuest);

        // Réinitialise la description de la quête
        questDescription.text = string.Empty;

        // Réinitialise la quête sélectionnée
        selected = null;

        // Actualise le status de la quête
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();

        // Réinitialise le script de la quête
        qs = null;
    }


    /// <summary>
    /// Clic sur le bouton Abandonner : Abandonne la quête
    /// </summary>
    public void AbandonQuest()
    {
        // Pour chacun des objectifs de collecte de la quête
        foreach (CollectObjective collectObjective in selected.MyCollectObjectives)
        {
            // Désabonnement sur l'évènement de mise à jour du nombre d'élements de l'item
            InventoryScript.MyInstance.ItemCountChangedEvent -= new ItemCountChanged(collectObjective.UpdateItemCount);

            // Complète l'objectif de collecte
            collectObjective.Complete();
        }

        // Pour chacun des objectifs d'ennemi de la quête
        foreach (KillObjective killObjective in selected.MyKillObjectives)
        {
            // Désabonnement sur l'évènement de la mort d'un personnage
            GameManager.MyInstance.KillConfirmedEvent -= new KillConfirmed(killObjective.UpdateKillCount);
        }

        // Supprime la quête
        RemoveQuest(selected.MyQuestScript);
    }

    /// <summary>
    /// Clic sur le bouton Suivre : Suivi de la quête
    /// </summary>
    public void TrackQuest()
    {

    }
}