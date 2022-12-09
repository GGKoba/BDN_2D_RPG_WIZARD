﻿using UnityEngine;
using UnityEngine.UI;



// Liste des types de messages
public enum CombatTextType { Damage, Heal, Experience };


/// <summary>
/// Classe de gestion des textes de combats
/// </summary>
public class CombatTextManager : MonoBehaviour
{
    // Instance de classe (singleton)
    private static CombatTextManager instance;

    // Propriété d'accès à l'instance
    public static CombatTextManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type CombatTextManager (doit être unique)
                instance = FindObjectOfType<CombatTextManager>();
            }

            return instance;
        }
    }

    // Prefab du texte de combat
    [SerializeField]
    private GameObject combatTextPrefab = default;


    /// <summary>
    /// Création d'un texte
    /// </summary>
    /// <param name="position">Position du texte</param>
    /// <param name="text">Contenu du texte</param>
    /// <param name="type">Type de texte</param>
    /// <param name="crit">texte "critique"</param>
    public void CreateText(Vector2 position, string text, CombatTextType type, bool crit)
    {
        //Decalage du texte
        position.y += 0.8f;

        // Instancie un objet "CombatText"
        Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();

        // Actualise la position
        sct.transform.position = position;

        // Préfixe/Suffixe
        string before = string.Empty;
        string after = string.Empty;

        switch (type)
        {
            case CombatTextType.Damage:
                before = "-";

                // Actualise la couleur
                sct.color = Color.red;
                break;

            case CombatTextType.Heal:
                before = "+";

                // Actualise la couleur
                sct.color = Color.green;
                break;

            case CombatTextType.Experience:
                before = "+";
                after = " XP";

                // Actualise la couleur
                sct.color = Color.yellow;
                break;
        }

        // Actualise le texte
        sct.text = before + text + after;

        // Si le etxte est critique
        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}