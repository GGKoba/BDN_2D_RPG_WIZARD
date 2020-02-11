using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



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


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipmentButtons = FindObjectsOfType<CharacterButton>();
        actionButtons = FindObjectsOfType<ActionButton>();
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // [DEBUG] : [V] - Sauvegarde
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("SAVE");
            Save();
        }

        // [DEBUG] : [W] - Chargement
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("LOAD");
            Load();
        }
    }

    /// <summary>
    /// Sauvegarde les données
    /// </summary>
    private void Save()
    {
        try
        {
            // Formatteur de données
            BinaryFormatter bf = new BinaryFormatter();

            // Gestion des fichiers
            FileStream file = File.Open(Application.persistentDataPath + "/rpgSaveTest.dat", FileMode.Create);

            // Données de sauvegarde
            SaveData data = new SaveData();

            // Enregistrement des données des équipements
            SaveEquipment(data);

            // Enregistrement des données des sacs
            SaveBags(data);

            // Enregistrement des données du joueur
            SavePlayer(data);

            // Enregistrement des données des coffres
            SaveChests(data);

            // Enregistrement des données des boutons d'actions
            SaveActionButtons(data);

            // Serialisation des données
            bf.Serialize(file, data);

            // Fermeture du fichier
            file.Close();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Charge les données
    /// </summary>
    private void Load()
    {
        try
        {
            // Formatteur de données
            BinaryFormatter bf = new BinaryFormatter();

            // Gestion des fichiers
            FileStream file = File.Open(Application.persistentDataPath + "/rpgSaveTest.dat", FileMode.Open);

            // Données de chargement
            SaveData data = (SaveData)bf.Deserialize(file);

            // Fermeture du fichier
            file.Close();

            // Chargement des données des équipements
            LoadEquipment(data);

            // Chargement des données des sacs
            LoadBags(data);

            // Chargement des données du joueur
            LoadPlayer(data);

            // Chargement des données des coffres
            LoadChests(data);

            // Chargement des données des boutons d'actions
            LoadActionButtons(data);
        }
        catch (System.Exception)
        {
            throw;
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
                Item item = Array.Find(items, aItem => aItem.MyTitle == itemData.MyTitle);
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
            CharacterButton button = Array.Find(equipmentButtons, btn => btn.name == equipmentData.MyType);
            button.EquipArmor(Array.Find(items, item => item.MyTitle == equipmentData.MyTitle) as Armor);
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
}