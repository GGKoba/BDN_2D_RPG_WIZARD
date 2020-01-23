﻿using System;
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

    // Tableau des objectifs de collecte
    [SerializeField]
    private CollectObjective[] collectObjectives = default;

    // Propriété d'accès au tableau des objectifs de collecte
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    // Propriété d'accès à l'indicateur sur la complétude de la quête
    public bool IsComplete
    {
        get
        {
            // Pour tous les objectifs
            foreach (CollectObjective collectObjective in collectObjectives)
            {
                if (!collectObjective.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }

    // Retourne la description formatée de la quête
    public string GetDescription()
    {
        // Description de la quête
        string fullDescription = string.Empty;
        fullDescription += string.Format("<color=#820D0D><b>{0}</b></color>\n\n<size=12>{1}</size>", MyTitle, MyDescription);

        // Ajout des éventuels objectifs
        if (collectObjectives.Length > 0)
        {
            string objectivesText = string.Empty;

            // Pour chaque objectif
            foreach (Objective objective in collectObjectives)
            {
                objectivesText += string.Format("<size=12><i>{0} : {1}/{2}</i></size>\n", objective.MyType, objective.MyCurrentAmount, objective.MyAmount);
            }
            fullDescription += string.Format("\n\n<color=#3F6E8E>Objectifs</color>\n{0}", objectivesText);
        }

        return fullDescription;
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
}


/// <summary>
/// Classe abstraite de l'objet "Objectifs"
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
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item);

            // Actualise les informations de la quête
            QuestWindow.MyInstance.UpdateSelected();

            // Vérifie si la quête est terminée
            QuestWindow.MyInstance.CheckCompletion();
        }
    }
}