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

    // Propriété d'accès sur l'expérience
    public float MyXp { get; set; }

    // Propriété d'accès sur l'expérience max
    public float MyMaxXp { get; set; }

    // Propriété d'accès sur la vie
    public float MyHealth { get; set; }

    // Propriété d'accès sur la vie max
    public float MyMaxHealth { get; set; }

    // Propriété d'accès sur le mana
    public float MyMana { get; set; }

    // Propriété d'accès sur le mana max
    public float MyMaxMana { get; set; }

    // Propriété d'accès sur l'argent
    public int MyGold { get; set; }

    // Propriété d'accès sur la position
    public float MyX { get; set; }
    public float MyY { get; set; }


    // Constructeur
    public PlayerData(int level, float xp, float maxXp, float health, float maxHealth, float mana, float maxMana, int gold, Vector2 position)
    {
        MyLevel = level;
        MyXp = xp;
        MyMaxXp = maxXp;
        MyHealth = health;
        MyMaxHealth = maxHealth;
        MyMana = mana;
        MyMaxMana = maxMana;
        MyGold = gold;
        MyX = position.x;
        MyY = position.y;
    }
}