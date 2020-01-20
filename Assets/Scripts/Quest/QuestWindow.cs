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
    /// Accepte une quête
    /// </summary>
    /// <param name="quest">Quête à accepter</param>
    public void AcceptQuest(Quest quest)
    {
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

        string objectivesText = string.Empty;

        foreach (Objective objective in quest.MyCollectObjectives)
        {
            objectivesText += string.Format("<size=12><i>{0} : {1}/{2}</i></size>\n", objective.MyType, objective.MyCurrentAmount, objective.MyAmount);
        }

        // Actualise la quête sélectionnée
        selected = quest;

        // Actualise la description de la quête sélectionnée
        questDescription.text = string.Format("<color=#820D0D><b>{0}</b></color>\n\n<size=12>{1}</size>\n\n<color=#3F4D6C>Objectifs</color>\n{2}", quest.MyTitle, quest.MyDescription, objectivesText);
    }
}