using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de l'objet "Quête"
/// </summary>
[Serializable]
public class Quest
{
    // Titre de la quête
    [SerializeField]
    private string title = default;

    // Propriété d'accès au titre de la quête
    public string MyTitle { get => title; set => title = value; }

    // Description de la quête
    [SerializeField]
    private string description = default;

    // Propriété d'accès à la description de la quête
    public string MyDescription { get => description; set => description = value; }

    // Propriété d'accès à au script de la quête
    public QuestScript MyQuestScript { get; set; }

    // Propriété d'accès au donneur de quêtes
    public QuestGiver MyQuestGiver { get; set; }

    // Niveau de la quête
    [SerializeField]
    private int level = default;

    // Propriété d'accèsau niveau de la quête
    public int MyLevel { get => level; }

    // Expérience de la quête
    [SerializeField]
    private int xp = default;

    // Propriété d'accès à l'expérience de la quête
    public int MyXp { get => xp; }

    // Tableau des objectifs de collecte
    [SerializeField]
    private CollectObjective[] collectObjectives = default;

    // Propriété d'accès au tableau des objectifs de collecte
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    // Tableau des objectifs d'ennemi
    [SerializeField]
    private KillObjective[] killObjectives = default;

    // Propriété d'accès au tableau des objectifs d'ennemi
    public KillObjective[] MyKillObjectives { get => killObjectives; }

    // Propriété d'accès à l'indicateur sur la complétude de la quête
    public bool IsComplete
    {
        get
        {
            // Pour tous les objectifs de collecte
            foreach (Objective collectObjective in collectObjectives)
            {
                if (!collectObjective.IsComplete)
                {
                    // Retourne que c'est KO
                    return false;
                }
            }
            // Pour tous les objectifs d'ennemi
            foreach (Objective killObjective in killObjectives)
            {
                if (!killObjective.IsComplete)
                {
                    // Retourne que c'est KO
                    return false;
                }
            }
            // Retourne que c'est OK
            return true;
        }
    }

    /// <summary>
    /// Retourne la description formatée de la quête
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        // Description de la quête
        string questDescription = string.Empty;
        questDescription += string.Format("<color=#820D0D><b>{0}</b></color>\n\n<size=12>{1}</size>\n\n<color=#546320>XP : {2}</color>", MyTitle, MyDescription, XPManager.CalculateXP(this));

        return questDescription;
    }
}


/// <summary>
/// Classe abstraite de l'objet "Objectifs"
/// </summary>
[Serializable]
public abstract class Objective
{
    // Type de l'objectif
    [SerializeField]
    private string type = default;

    // Propriété d'accès au type de l'objectif
    public string MyType { get => type; }

    // Nombre à atteindre pour l'objectif
    [SerializeField]
    private int amount = default;

    // Propriété d'accès au nombre à atteindre pour l'objectif
    public int MyAmount { get => amount; }

    // Nombre courant
    private int currentAmount;

    // Propriété d'accès au nombre courant
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }

    // Propriété d'accès à l'indicateur sur la complétude de l'objectif
    public bool IsComplete { get => currentAmount >= amount; }


    /// <summary>
    /// Retourne une message de l'objectif
    /// </summary>
    /// <param name="objective">Objectif à afficher</param>
    public string GetObjectiveMessage()
    {
        string message = string.Empty;

        // Si l'objectif n'est pas encore atteint
        if (MyCurrentAmount <= MyAmount)
        {
            // Message par défaut
            message += string.Format("{0} : {1}/{2}", MyType, MyCurrentAmount, MyAmount);
        }

        // Retourne le message
        return message;
    }

    /// <summary>
    /// Actualise les informations de la quête
    /// </summary>
    /// <param name="displayMessage">Affichage ou non d'un message</param>
    public void RefreshObjectives(bool displayMessage = false)
    {
        // Si l'on doit afficher un message
        if (displayMessage)
        {
            // Affiche le message de progression de l'objectif
            MessageFeedManager.MyInstance.WriteMessage(GetObjectiveMessage());
        }

        // Vérifie si la quête est terminée
        QuestWindow.MyInstance.CheckCompletion();

        // Actualise les informations de la quête
        QuestWindow.MyInstance.UpdateSelected();
    }
}


/// <summary>
/// Classe abstraite de l'objet "Objectifs" : Objectifs de collecte
/// </summary>
[Serializable]
public class CollectObjective : Objective
{
    /// <summary>
    /// Actualise le nombre d'item pour l'objectif
    /// </summary>
    /// <param name="item">Item courant</param>
    public void UpdateItemCount(Item item)
    {
        // Si c'est le même item que celui de l'objectif
        if (MyType.ToLower() == item.GetType().ToString().ToLower())
        {
            // Item du même type contenu dans l'inventaire
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);

            // Actualise les informations de la quête
            RefreshObjectives(true);
        }
    }

    /// <summary>
    /// Actualise le nombre d'item pour l'objectif
    /// </summary>
    public void UpdateItemCount()
    {
        // Item du même type contenu dans l'inventaire
        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);

        // Actualise les informations de la quête
        RefreshObjectives();
    }

    /// <summary>
    /// Complète un objectif de collecte
    /// </summary>
    public void Complete()
    {
        // Stack de l'item
        Stack<Item> items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);

        // Pour chaque item de la stack
        foreach (Item item in items)
        {
            // Supprime l'item
            item.Remove();
        }
    }
}


/// <summary>
/// Classe abstraite de l'objet "Objectifs" : Objectifs d'ennemi
/// </summary>
[Serializable]
public class KillObjective : Objective
{
    /// <summary>
    /// Actualise le nombre de personnages pour l'objectif
    /// </summary>
    public void UpdateKillCount(Character character)
    {
        // Si c'est le même personnage que celui de l'objectif
        if (MyType.ToLower() == character.MyType.ToLower())
        {
            // Si l'objectif n'est pas encore atteint
            if (MyCurrentAmount < MyAmount)
            {
                // Incrémente le compteur
                MyCurrentAmount++;

                // Actualise les informations de la quête
                RefreshObjectives(true);
            }
        }
    }
}