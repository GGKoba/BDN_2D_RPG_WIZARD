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