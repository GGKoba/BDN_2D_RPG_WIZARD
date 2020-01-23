﻿using UnityEngine;
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

    // Référence sur le donneur de quêtes
    private QuestGiver questGiver;

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
            GameObject go = Instantiate(questPrefab, questArea);

            // Actualise le titre de la quête
            go.GetComponent<Text>().text = quest.MyTitle;

            // Référence sur la quête
            go.GetComponent<QuestGiverScript>().MyQuest = quest;
        }
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
    /// Affiche les informations d'une quête
    /// </summary>
    /// <param name="quest">Quête affichée</param>
    public void ShowQuestInfo(Quest quest)
    {
        // S'il y a une quête
        if (quest != null)
        {
            // Masque la liste des quêtes
            questArea.gameObject.SetActive(false);

            // Affiche les boutons
            acceptButton.SetActive(true);
            backButton.SetActive(true);

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

            // Actualise la quête sélectionnée
            selected = quest;

            */
        }
    }

    /// <summary>
    /// Clic sur le bouton Accepter : Accepte une quête
    /// </summary>
    public void AcceptQuest()
    {

    }
    
    /// <summary>
    /// Clic sur le bouton Retour : Retourne à la liste des quêtes
    /// </summary>
    public void Back()
    {
        // Masque des boutons
        acceptButton.SetActive(false);
        backButton.SetActive(false);

        // Masque la description
        questDescription.SetActive(false);

        // Affiche la liste des quêtes
        questArea.gameObject.SetActive(true);
    }
}
