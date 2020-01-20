using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion de la fenêtre des quêtes du joueur
/// </summary>
public class QuestWindow : MonoBehaviour
{
    // Prefab de l'objet "Quête"
    [SerializeField]
    private GameObject questPrefab = default;

    // Conteneur de la liste des quêtes du joueur
    [SerializeField]
    private Transform questParent = default;


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

        go.GetComponent<Text>().text = quest.MyTitle;
    }
}