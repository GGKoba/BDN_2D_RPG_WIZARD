using UnityEngine;
using UnityEngine.UI;



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
    public void CreateText(Vector2 position, string text, Color color)
    {
        // Instancie un objet "CombatText"
        Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();

        // Actualise la position
        sct.transform.position = position;

        // Actualise le texte
        sct.text = text;

        // Actualise la color
        sct.color = color;
    }
}