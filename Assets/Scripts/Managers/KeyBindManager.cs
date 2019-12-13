using System.Collections.Generic;
using System.Linq;
using UnityEngine;



/// <summary>
/// Classe de gestion des raccourcis
/// </summary>
public class KeyBindManager : MonoBehaviour
{
    // Instance de classe (singleton)
    private static KeyBindManager instance;

    // Propriété d'accès à l'instance
    public static KeyBindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type KeyBindManager (doit être unique)
                instance = FindObjectOfType<KeyBindManager>();
            }

            return instance;
        }
    }

    // Propriété d'accès au dictionnaire des raccourcis
    public Dictionary<string, KeyCode> KeyBinds { get; private set; }

    // Propriété d'accès au dictionnaire des raccourcis d'action
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    // Nom de la clé
    private string bindName;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Initialise les dictionnaires
        KeyBinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();

        /// Raccourcis par défaut
        InitBinds();
    }


    /// <summary>
    /// Initialise les raccourcis
    /// </summary>
    private void InitBinds()
    {
        BindKey("UP", KeyCode.Z);
        BindKey("DOWN", KeyCode.S);
        BindKey("LEFT", KeyCode.Q); 
        BindKey("RIGHT", KeyCode.D);
        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }



    /// <summary>
    /// Assigne une touche
    /// </summary>
    /// <param name="key">Identifiant de la touche</param>
    /// <param name="keyBind">Touche à assigner</param>
    public void BindKey(string key, KeyCode keyBind)
    {
        // Assignation du dictionnaire courant
        Dictionary<string, KeyCode> currentDictionary = KeyBinds;

        // Si la clé contient "ACT"
        if (key.Contains("ACT"))
        {
            // Le dictionnaire courant est celui des actions
            currentDictionary = ActionBinds;
        }

        // Si c'est une nouvelle clé
        if (!currentDictionary.ContainsKey(key))
        {
            // Ajoute dans le dictionnaire
            currentDictionary.Add(key, keyBind);

            // Mise à jour du texte de la touche
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        // Si c'est une valeur existante
        else if (currentDictionary.ContainsValue(keyBind))
        {
            // Récuperation de la clé assignée à la valeur existante
            string existingKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            // Enlève la valeur de la clé existante
            currentDictionary[existingKey] = KeyCode.None;
            
            // Mise à jour du texte de la touche
            UIManager.MyInstance.UpdateKeyText(existingKey, KeyCode.None);
        }

        // Ajoute le nouveau raccourci
        currentDictionary[key] = keyBind;

        // Mise à jour du texte de la touche
        UIManager.MyInstance.UpdateKeyText(key, keyBind);

        // Vide le nom de la clé
        bindName = string.Empty;
    }

    /// <summary>
    /// Bind une touche
    /// </summary>
    /// <param name="name">Nom du bind</param>
    public void KeyBindOnClick(string name)
    {
        bindName = name;
    }

    /// <summary>
    /// Evènement de bind
    /// </summary>
    public void OnGUI()
    {
        // S'il y a un bind à faire
        if (bindName != string.Empty)
        {
            // Récupération de l'évènement
            Event e = Event.current;

            // Si c'est un touche
            if (e.isKey)
            {
                // Assigne la touche
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
