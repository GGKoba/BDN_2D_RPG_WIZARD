using System;
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
    /*
    // Indicateur sur la complétude de l'objectif
    private bool completed;

    // Propriété d'accès à l'indicateur sur la complétude de l'objectif
    public bool MyCompleted { get => completed; set => completed = value; }
    */
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
        }
    }
}