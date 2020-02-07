using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



/// <summary>
/// Classe de gestion des sauvegardes
/// </summary>
public class SaveManager : MonoBehaviour
{
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
            FileStream file = File.Open(Application.persistentDataPath + "/rpgSaveTest.dat", FileMode.OpenOrCreate);

            // Données de sauvegarde
            SaveData data = new SaveData();

            // Enregistre les données du joueur
            SavePlayer(data);

            // Serialisation des données
            bf.Serialize(file, data);

            // Fermeture du fichier
            file.Close();
        }
        catch (System.Exception)
        {

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

            // Chargement des données du joueur
            LoadPlayer(data);
        }
        catch (System.Exception)
        {

        }
    }



    /// <summary>
    /// Enregistre les données du joueur
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void SavePlayer(SaveData data)
    {
        // Données du joueur
        data.MyPlayerData = new PlayerData(Player.MyInstance.MyLevel);
    }

    /// <summary>
    /// Charge les données du joueur
    /// </summary>
    /// <param name="data">Données de sauvegarde</param>
    private void LoadPlayer(SaveData data)
    {
        // Données du joueur
        Player.MyInstance.MyLevel = data.MyPlayerData.MyLevel;

        // Actualise le texte du niveau du joueur
        Player.MyInstance.RefreshPlayerLevelText();
    }
}