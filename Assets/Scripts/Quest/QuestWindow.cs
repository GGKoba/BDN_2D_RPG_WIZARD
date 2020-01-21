using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de la fenêtre des quêtes du joueur
/// </summary>
public class QuestWindow : MonoBehaviour
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
    private Transform questParent = default;

    // Conteneur de la description de la quête
    [SerializeField]
    private Text questDescription = default;

    // CanvasGroup de la fenêtre des quêtes du joueur
    private CanvasGroup canvasGroup;


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
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {

    }

    /// <summary>
    /// Ouverture de la fenêtre des butins
    /// </summary>
    public void Open()
    {
        // Bloque les interactions
        canvasGroup.blocksRaycasts = true;

        // Affiche la fenêtre des butins
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Fermeture de la fenêtre des butins
    /// </summary>
    public void Close()
    {
        // Débloque les interactions
        canvasGroup.blocksRaycasts = false;

        // Masque la fenêtre des butins
        canvasGroup.alpha = 0;
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
        GameObject go = Instantiate(questPrefab, questParent);

        // Actualise le titre de la quête
        go.GetComponent<Text>().text = quest.MyTitle;

        // Script lié à l'objet "Quête"
        QuestScript questScript = go.GetComponent<QuestScript>();

        // Référence sur le script de la quête
        quest.MyQuestScript = questScript;

        // Référence sur la quête
        questScript.MyQuest = quest;
    }

    /// <summary>
    /// Affiche la description d'une quête
    /// </summary>
    /// <param name="quest">Quête affichée</param>
    public void ShowDescription(Quest quest)
    {
        // S'il y a une quête sélectionnée
        if (selected != null)
        {
            selected.MyQuestScript.DeSelect();
        }

        // Actualise la quête sélectionnée
        selected = quest;

        // Description de la quête
        string fullDescription = string.Empty;
        fullDescription += string.Format("<color=#820D0D><b>{0}</b></color>\n\n<size=12>{1}</size>", quest.MyTitle, quest.MyDescription);

        // Ajout des éventuels objectifs
        if(quest.MyCollectObjectives.Length > 0)
        {
            string objectivesText = string.Empty;

            // Pour chaque objectif
            foreach (Objective objective in quest.MyCollectObjectives)
            {
                objectivesText += string.Format("<size=12><i>{0} : {1}/{2}</i></size>\n", objective.MyType, objective.MyCurrentAmount, objective.MyAmount);
            }
            fullDescription += string.Format("\n\n<color=#3F6E8E>Objectifs</color>\n{0}", objectivesText);
        }

        // Actualise la description de la quête sélectionnée
        questDescription.text = fullDescription;
    }

    /// <summary>
    /// Actualise la quête
    /// </summary>
    public void UpdateSelected()
    {
        //Affiche la description d'une quête
        ShowDescription(selected);           
    }
}