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

    // CanvasGroup de la fenêtre des quêtes du joueur
    // private CanvasGroup canvasGroup;

    // Liste des quêtes
    private readonly List<QuestScript> questScripts = new List<QuestScript>();


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le canvasGroup de la fenêtre des quêtes du joueur
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Fermeture de la fenêtre des quêtes du joueur
       OpenClose();
    }

    /*
    /// <summary>
    /// Ouverture de la fenêtre des quêtes du joueur
    /// </summary>
    public void Open()
    {
        // Bloque les interactions
        canvasGroup.blocksRaycasts = true;

        // Affiche la fenêtre des quêtes du joueur
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Fermeture de la fenêtre des quêtes du joueur
    /// </summary>
    public void Close()
    {
        // Débloque les interactions
        canvasGroup.blocksRaycasts = false;

        // Masque la fenêtre des quêtes du joueur
        canvasGroup.alpha = 0;
    }
    */

    /// <summary>
    /// Ouverture/Fermeture de la fenêtre des quêtes du joueur
    /// </summary>
    public void OpenClose()
    {
        // Bloque/débloque les interactions
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;

        // Masque(0) /Affiche(1) le menu
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
    }

    /// <summary>
    /// Accepte une quête
    /// </summary>
    /// <param name="quest">Quête à accepter</param>
    public void AcceptQuest(Quest quest)
    {
        // Pour chacun des objectifs de la quête
        foreach (CollectObjective collectObjective in quest.MyCollectObjectives)
        {
            // Abonnement sur l'évènement de mise à jour du nombre d'élements de l'item
            InventoryScript.MyInstance.ItemCountChangedEvent += new ItemCountChanged(collectObjective.UpdateItemCount);
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
        questScripts.Add(questScript);
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
            if (quest.MyCollectObjectives.Length > 0)
            {
                string objectivesText = string.Empty;

                // Pour chaque objectif
                foreach (Objective objective in quest.MyCollectObjectives)
                {
                    objectivesText += string.Format("<size=12><i>{0} : {1}/{2}</i></size>\n", objective.MyType, objective.MyCurrentAmount, objective.MyAmount);
                }
                description += string.Format("\n\n<color=#3F6E8E>Objectifs</color>\n{0}", objectivesText);
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
            // Vérifie si la quête est terminée
            questScript.IsComplete();
        }
    }
}