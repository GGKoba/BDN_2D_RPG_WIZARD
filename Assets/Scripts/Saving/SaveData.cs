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

    // Propriété d'accès aux données des sacs
    public InventoryData MyInventoryData { get; set; }

    // Propriété d'accès aux données des équipements
    public List<EquipmentData> MyEquipmentData { get; set; }

    // Propriété d'accès aux données des boutons d'actions
    public List<ActionButtonData> MyActionButtonData { get; set; }

    // Propriété d'accès aux données des quêtes
    public List<QuestData> MyQuestData { get; set; }

    // Propriété d'accès aux données des donneurs de quêtes
    public List<QuestGiverData> MyQuestGiverData { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyChestData = new List<ChestData>();
        MyEquipmentData = new List<EquipmentData>();
        MyActionButtonData = new List<ActionButtonData>();
        MyQuestData = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
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


    /// <summary>
    /// Constructeur
    /// </summary>
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

    // Propriété d'accès sur l'index de l'emplacement
    public int MySlotIndex { get; set; }

    // Propriété d'accès sur l'index du sac
    public int MyBagIndex { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public ItemData(string title, int stackCount = 0, int slotIndex = 0, int bagIndex = 0)
    {
        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
        MyBagIndex = bagIndex;
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


    /// <summary>
    /// Constructeur
    /// </summary>
    public ChestData(string name)
    {
        MyName = name;
        MyItems = new List<ItemData>();
    }
}

/// <summary>
/// Classe des données de l'inventaire
/// </summary>
[Serializable]
public class InventoryData
{
    // Propriété d'accès à la liste des sacs
    public List<BagData> MyBags { get; set; }

    // Propriété d'accès aux items
    public List<ItemData> MyItems { get; set; }

    /// <summary>
    /// Constructeur
    /// </summary>
    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
    }
}

/// <summary>
/// Classe des données des sacs
/// </summary>
[Serializable]
public class BagData
{
    // Propriété d'accès au nombre d'emplacement
    public int MySlotCount { get; set; }

    // Propriété d'accès à l'index du sac
    public int MyBagIndex { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public BagData(int count, int index)
    {
        MySlotCount = count;
        MyBagIndex = index;
    }
}


/// <summary>
/// Classe des données des équipements
/// </summary>
[Serializable]
public class EquipmentData
{
    // Propriété d'accès au nom
    public string MyTitle { get; set; }

    // Propriété d'accès au type
    public string MyType { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public EquipmentData(string title, string type)
    {
        MyTitle = title;
        MyType = type;
    }
}

/// <summary>
/// Classe des données des boutons d'action
/// </summary>
[Serializable]
public class ActionButtonData
{
    // Propriété d'accès au nom du bouton d'action
    public string MyAction { get; set; }

    // Propriété d'accès au type d'item
    public bool IsItem { get; set; }

    // Propriété d'accès à l'index du bouton
    public int MyIndex { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public ActionButtonData(string action, bool itemOrNot, int index)
    {
        MyAction = action;
        IsItem = itemOrNot;
        MyIndex = index;
    }
}

/// <summary>
/// Classe des données des quêtes
/// </summary>
[Serializable]
public class QuestData
{
    // Propriété d'accès au titre
    public string MyTitle { get; set; }

    // Propriété d'accès à la description
    public string MyDescription { get; set; }

    // Propriété d'accès aux objectifs de collecte
    public CollectObjective[] MyCollectObjectives { get; set; }

    // Propriété d'accès aux objectifs d'ennemi
    public KillObjective[] MyKillObjectives { get; set; }

    // Propriété d'accès à l'identifiant du donneur de quêtes
    public int MyQuestGiverId { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public QuestData(string title, string description, CollectObjective[] collect, KillObjective[] kill, int questGiverId)
    {
        MyTitle = title;
        MyDescription = description;
        MyCollectObjectives = collect;
        MyKillObjectives = kill;
        MyQuestGiverId = questGiverId;
    }
}


/// <summary>
/// Classe des données des donneurs de quêtes
/// </summary>
[Serializable]
public class QuestGiverData
{
    // Propriété d'accès à l'identifiant du donneur de quêtes
    public int MyQuestGiverId { get; set; }

    // Propriété d'accès à la liste des quêtes
    public List<string> MyCompletedQuests { get; set; }


    /// <summary>
    /// Constructeur
    /// </summary>
    public QuestGiverData(int questGiverId, List<string> completedQuests)
    {
        MyQuestGiverId = questGiverId;
        MyCompletedQuests = completedQuests;
    }
}