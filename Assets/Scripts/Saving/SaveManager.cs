using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe de gestion des sauvegardes
/// </summary>
public class SaveManager : MonoBehaviour
{
    // Tableau des coffres
    private Chest[] chests;

    // Tableau des items
    [SerializeField]
    private Item[] items = default;

    // Tableau des boutons d'équipements
    private CharacterButton[] equipmentButtons;

    // Tableau des boutons d'actions
    private ActionButton[] actionButtons;

    // Tableau des sauvegardes
    [SerializeField]
    private SavedGame[] saveSlots = default;

    // Action à effectuer
    private string action;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipmentButtons = FindObjectsOfType<CharacterButton>();
        actionButtons = FindObjectsOfType<ActionButton>();

        foreach (SavedGame savedGame in saveSlots)
        {
            ShowSavedFiles(savedGame);
        }
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // [DEBUG]
        Debug.Log(Application.persistentDataPath);
    }

    /// <summary>
    /// Récupère les informations à partir d'un fichier de sauvegarde
    /// </summary>
    /// <param name="savedGame">Emplacement de sauvegarde</param>
    private void ShowSavedFiles(SavedGame savedGame)
    {
        string filePath = Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat";

        // Si le fichier de sauvegarde existe
        if (File.Exists(filePath))
        {
            // Formatteur de données
            BinaryFormatter bf = new BinaryFormatter();

            // Gestion des fichiers
            FileStream file = File.Open(filePath, FileMode.Open);

            // Données de chargement
            SaveData data = (SaveData)bf.Deserialize(file);

            // Fermeture du fichier
            file.Close();

            // Affiche les informations du fichier
            savedGame.ShowInfo(data);
        }
    }

    /// <summary>
    /// Sauvegarde les données
    /// </summary>
    /// <param name="savedGame">Emplacement de sauvegarde</param>
    private void Save(SavedGame savedGame)
    {
        try
        {
            // Formatteur de données
            BinaryFormatter bf = new BinaryFormatter();

            // Fichier de sauvegarde
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);

            // Données de sauvegarde
            SaveData data = new SaveData();

            // Récupèration du nom de la scène utilisée
            data.MyScene = SceneManager.GetActiveScene().name;

            // Enregistrement des données des équipements
            SaveEquipment(data);

            // Enregistrement des données des sacs
            SaveBags(data);

            // Enregistrement des données de l'inventaire
            SaveInventory(data);

            // Enregistrement des données du joueur
            SavePlayer(data);

            // Enregistrement des données des coffres
            SaveChests(data);

            // Enregistrement des données des boutons d'actions
            SaveActionButtons(data);

            // Enregistrement des données des quêtes
            SaveQuests(data);

            // Enregistrement des données des donneurs dequêtes
            SaveQuestsGiver(data);

            // Serialisation des données
            bf.Serialize(file, data);

            // Fermeture du fichier
            file.Close();

            // Récupèration des informations à partir d'un fichier de sauvegarde
            ShowSavedFiles(savedGame);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Charge les données
    /// </summary>
    /// <param name="savedGame">Emplacement de sauvegarde</param>
    private void Load(SavedGame savedGame)
    {
        try
        {
            // Formatteur de données
            BinaryFormatter bf = new BinaryFormatter();

            // Fichier de sauvegarde
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            // Données de chargement
            SaveData data = (SaveData)bf.Deserialize(file);

            // Fermeture du fichier
            file.Close();

            // Chargement des données des équipements
            LoadEquipment(data);

            // Chargement des données des sacs
            LoadBags(data);

            // Chargement des données de l'inventaire
            LoadInventory(data);

            // Chargement des données du joueur
            LoadPlayer(data);

            // Chargement des données des coffres
            LoadChests(data);

            // Chargement des données des boutons d'actions
            LoadActionButtons(data);

            // Chargement des données des quêtes
            LoadQuests(data);

            // Chargement des données des donneurs dequêtes
            LoadQuestsGiver(data);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Supprime les données
    /// </summary>
    /// <param name="savedGame">Emplacement de sauvegarde</param>
    private void Delete(SavedGame savedGame)
    {
        // Supprime le fichier de sauvegarde
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");

        // Masque les informations de l'emplacement
        savedGame.HideVisuals();
    }

    /// <summary>
    /// Fenêtre d'interaction
    /// </summary>
    /// <param name="clickButton">Emplacement de sauvegarde</param>
    public void ShowDialog(GameObject clickButton)
    {
        action = clickButton.name;
        SavedGame savedGame = clickButton.GetComponentInParent<SavedGame>();

        switch (action)
        {
            // Action de suppression
            case "Delete":
                Delete(savedGame);
                break;
            // Action de chargement
            case "Load":
                Load(savedGame);
                break;
            // Action de sauvegarde
            case "Save":
                Save(savedGame);
                break;
        }
    }



    /// <summary>
    /// Enregistrement des données du joueur
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SavePlayer(SaveData data)
    {
        // Données du joueur
        data.MyPlayerData = new PlayerData(
            Player.MyInstance.MyLevel,
            Player.MyInstance.MyXp.MyCurrentValue,
            Player.MyInstance.MyXp.MyMaxValue,
            Player.MyInstance.MyHealth.MyCurrentValue,
            Player.MyInstance.MyHealth.MyMaxValue,
            Player.MyInstance.MyMana.MyCurrentValue,
            Player.MyInstance.MyMana.MyMaxValue,
            Player.MyInstance.MyGold,
            Player.MyInstance.transform.position
        );
    }

    /// <summary>
    /// Enregistrement des données des coffres
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SaveChests(SaveData data)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            data.MyChestData.Add(new ChestData(chests[i].name));

            foreach (Item item in chests[i].MyItems)
            {
                if (chests[i].MyItems.Count > 0)
                {
                    data.MyChestData[i].MyItems.Add(new ItemData(item.MyTitle, item.MySlot.MyItems.Count, item.MySlot.MyIndex));
                }
            }
        }
    }

    /// <summary>
    /// Enregistrement des données des sacs
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScript.MyInstance.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.MyInstance.MyBags[i].MySlotsCount, InventoryScript.MyInstance.MyBags[i].MyBagButton.MyBagIndex));
        }
    }

    /// <summary>
    /// Enregistrement des données des équipements
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SaveEquipment(SaveData data)
    {
        foreach (CharacterButton button in equipmentButtons)
        {
            if (button.MyEquippedArmor != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(button.MyEquippedArmor.MyTitle, button.name));
            }
        }
    }

    /// <summary>
    /// Enregistrement des données des boutons d'actions
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SaveActionButtons(SaveData data)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].MyUseable != null)
            {
                ActionButtonData actionButtonData;

                if (actionButtons[i].MyUseable is Spell)
                {
                    actionButtonData = new ActionButtonData((actionButtons[i].MyUseable as Spell).MyName, false, i); 
                }
                else
                {
                    actionButtonData = new ActionButtonData((actionButtons[i].MyUseable as Item).MyTitle, true, i);
                    
                }

                data.MyActionButtonData.Add(actionButtonData);
            }
        }
    }

    /// <summary>
    /// Enregistrement des données de l'inventaire
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();

        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    /// <summary>
    /// Enregistrement des données des quêtes
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SaveQuests(SaveData data)
    {
        foreach (Quest quest in QuestWindow.MyInstance.MyQuests)
        {
            data.MyQuestData.Add(new QuestData(quest.MyTitle, quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives, quest.MyQuestGiver.MyQuestGiverId));
        }
    }

    /// <summary>
    /// Enregistrement des données des donneurs de quêtes
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SaveQuestsGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiver questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.MyQuestGiverId, questGiver.MyCompletedQuests));
        }
    }

    /// <summary>
    /// Chargement des données du joueur
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadPlayer(SaveData data)
    {
        // Données du joueur
        Player.MyInstance.MyLevel = data.MyPlayerData.MyLevel;
        Player.MyInstance.MyXp.Initialize(data.MyPlayerData.MyXp, data.MyPlayerData.MyMaxXp);
        Player.MyInstance.MyHealth.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);
        Player.MyInstance.MyMana.Initialize(data.MyPlayerData.MyMana, data.MyPlayerData.MyMaxMana);
        Player.MyInstance.MyGold = data.MyPlayerData.MyGold;
        Player.MyInstance.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);

        // Actualise le texte du niveau du joueur
        Player.MyInstance.RefreshPlayerLevelText();
    }

    /// <summary>
    /// Chargement des données des coffres
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadChests(SaveData data)
    {
        foreach (ChestData chest in data.MyChestData)
        {
            Chest c = Array.Find(chests, aChest => aChest.name == chest.MyName);

            foreach (ItemData itemData in chest.MyItems)
            {
                Item item = Array.Find(items, aItem => aItem.MyTitle.ToLower() == itemData.MyTitle.ToLower());
                item.MySlot = c.MyBank.MySlots.Find(slot => slot.MyIndex == itemData.MySlotIndex);
                c.MyItems.Add(item);
            }
        }
    }

    /// <summary>
    /// Chargement des données des sacs
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {
            // Création d'un sac
            Bag newBag = (Bag)Instantiate(items[0]);

            // Initialisation du sac
            newBag.Initialize(bagData.MySlotCount);

            // Ajoute le sac
            InventoryScript.MyInstance.AddBag(newBag, bagData.MyBagIndex);
        }
    }

    /// <summary>
    /// Chargement des données des équipements
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            CharacterButton button = Array.Find(equipmentButtons, btn => btn.name.ToLower() == equipmentData.MyType.ToLower());
            button.EquipArmor(Array.Find(items, item => item.MyTitle.ToLower() == equipmentData.MyTitle.ToLower()) as Armor);
        }
    }

    /// <summary>
    /// Chargement des données des boutons d'actions
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData actionButtonData in data.MyActionButtonData)
        {
            if (actionButtonData.IsItem)
            {
                actionButtons[actionButtonData.MyIndex].SetUseable(InventoryScript.MyInstance.GetUseable(actionButtonData.MyAction));
            }
            else
            {
                actionButtons[actionButtonData.MyIndex].SetUseable(SpellBook.MyInstance.GetSpell(actionButtonData.MyAction));
            }
        }
    }

    /// <summary>
    /// Chargement des données de l'inventaire
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            Item item = Array.Find(items, aItem => aItem.MyTitle.ToLower() == itemData.MyTitle.ToLower());

            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                InventoryScript.MyInstance.PlaceInSlot(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }

    /// <summary>
    /// Chargement des données des quêtes
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestData questData in data.MyQuestData)
        {
            QuestGiver questGiver = Array.Find(questGivers, qg => qg.MyQuestGiverId == questData.MyQuestGiverId);
            Quest quest = Array.Find(questGiver.MyQuests, q => q.MyTitle == questData.MyTitle);
            quest.MyQuestGiver = questGiver;
            quest.MyKillObjectives = questData.MyKillObjectives;

            QuestWindow.MyInstance.AcceptQuest(quest);
        }
    }

    /// <summary>
    /// Chargement des données des donneurs de quêtes
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadQuestsGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiverData questGiverData in data.MyQuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, qg => qg.MyQuestGiverId == questGiverData.MyQuestGiverId);
            questGiver.MyCompletedQuests = questGiverData.MyCompletedQuests;
            questGiver.UpdateQuestStatus();
        }
    }
}