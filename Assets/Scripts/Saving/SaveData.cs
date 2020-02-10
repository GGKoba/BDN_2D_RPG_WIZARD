using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe des données de sauvegarde
/// </summary>
[Serializable]
public class SaveData
{
    // Propriété d'accès aux données du joueur
    public PlayerData MyPlayerData { get; set; }

    // Propriété d'accès aux données des coffres
    public List<ChestData> MyChestData { get; set; }


    // Constructeur
    public SaveData()
    {
        MyChestData = new List<ChestData>();
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

/// <summary>
/// Classe des données des items
/// </summary>
[Serializable]
public class ItemData
{
    // Propriété d'accès sur le titre 
    public string MyTitle { get; set; }

    // Propriété d'accès sur le nombre d'éléments
    public int MyStackCount { get; set; }

    // Propriété d'accès sur l'emplacement
    public int MySlotIndex { get; set; }


    // Constructeur
    public ItemData(string title, int stackCount = 0, int slotIndex = 0)
    {
        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
    }
}

/// <summary>
/// Classe des données des coffres
/// </summary>
[Serializable]
public class ChestData
{
    // Propriété d'accès au nom
    public string MyName { get; set; }

    // Propriété d'accès aux items
    public List<ItemData> MyItems { get; set; }


    // Constructeur
    public ChestData(string name)
    {
        MyName = name;
        MyItems = new List<ItemData>();
    }
}