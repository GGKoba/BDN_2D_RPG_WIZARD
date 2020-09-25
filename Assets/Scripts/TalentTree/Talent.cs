using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des talents
/// </summary>
public class Talent : MonoBehaviour
{
    // Image du talent
    private Image sprite;

    // Texte du compteur
    [SerializeField]
    private Text countText;
  

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        sprite = GetComponent<Image>();
    }

    /// <summary>
    /// Verrouillage d'un talent
    /// </summary>
    public void Lock()
    {
        // Verrouille l'image (fond gris)
        sprite.color = Color.grey;

        // Verrouille le texte (fond gris)
        countText.color = Color.grey;
    }

    /// <summary>
    /// Déverrouillage d'un talent
    /// </summary>
    public void Unlock()
    {
        // Verrouille l'image (fond blanc)
        sprite.color = Color.white;

        // Verrouille le texte (fond blanc)
        countText.color = Color.white;
    }
}