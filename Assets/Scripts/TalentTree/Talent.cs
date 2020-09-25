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
    private Text countText = default;

    // Point(s) maximum sur le talent
    [SerializeField]
    private int maxCount;

    // Point(s) sur le talent
    private int currentCount;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        sprite = GetComponent<Image>();
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        countText.text = getCountText();
    }

    /// <summary>
    /// Texte des points du talent
    /// </summary>
    /// <returns></returns>
    public string getCountText()
    {
        return currentCount + "/" + maxCount;
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

    // Clic sur le talent
    public bool Click()
    {
        // Si on peut utiliser des points
        if (currentCount < maxCount)
        {
            // Ajoute un point au talent
            currentCount++;

            // Actualise le texte
            countText.text = getCountText();

            return true;
        }

        return false;
    }
}