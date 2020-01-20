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