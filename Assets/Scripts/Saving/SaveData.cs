using System;
using UnityEngine;



/// <summary>
/// Classe des données de sauvegarde
/// </summary>
[Serializable]
public class SaveData
{
    // Propriété d'accès aux données du joueur
    public PlayerData MyPlayerData { get; set; }

    // Constructeur
    public SaveData()
    {

    }
}

/// <summary>
/// Classe des données du joueur
/// </summary>
[Serializable]
public class PlayerData
{
    // Propriété d'accès sur le niveau 
    public int MyLevel { get; set; }

    // Constructeur
    public PlayerData(int level)
    {
        MyLevel = level;
    }
}