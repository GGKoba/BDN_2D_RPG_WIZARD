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

    // propriété d'accès sur le nombre de points du talent
    public int MyCurrentCount { get => currentCount; set => currentCount = value; }

    // Talent débloqué ou non
    private bool unlocked;

    // Talent enfant
    [SerializeField]
    private Talent childTalent = default;

    // Image de la flèche
    [SerializeField]
    private Image arrowImage = default;

    // Sprite de la flèche inactive
    [SerializeField]
    private Sprite arrowSpriteLocked = default;

    // Sprite de la flèche active
    [SerializeField]
    private Sprite arrowSpriteUnlocked = default;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur l'image du talent
        sprite = GetComponent<Image>();

        // Actualise le texte du compteur
        countText.text = getCountText();

        // Déverouille le talent
        if (unlocked)
        {
            Unlock();
        }
    }

    /// <summary>
    /// Texte des points du talent
    /// </summary>
    /// <returns></returns>
    public string getCountText()
    {
        return MyCurrentCount + "/" + maxCount;
    } 

    /// <summary>
    /// Verrouillage d'un talent
    /// </summary>
    public void Lock()
    {
        // Verrouille l'image (fond gris)
        sprite.color = Color.gray;

        // Verrouille le texte (fond gris)
        if (countText != null)
        {
            countText.color = Color.gray;
        }

        // S'il y a une jonction avec un talent enfant
        if (arrowImage != null)
        {
            arrowImage.sprite = arrowSpriteLocked;
        }
    }

    /// <summary>
    /// Déverrouillage d'un talent
    /// </summary>
    public void Unlock()
    {
        // Déverrouille l'image (fond blanc)
        sprite.color = Color.white;

        // Déverrouille le texte (fond blanc)
        if (countText != null)
        {
            countText.color = Color.white;
        }

        // S'il y a une jonction avec un talent enfant
        if (arrowImage != null)
        {
            arrowImage.sprite = arrowSpriteUnlocked;
        }

        // Flag le talent comme "Déverrouillé"
        unlocked = true;
    }

    // Clic sur le talent : Virtual pour être écrasée pour les autres classes
    public virtual bool Click()
    {
        // Si on peut utiliser des points
        if (MyCurrentCount < maxCount && unlocked)
        {
            // Ajoute un point au talent
            MyCurrentCount++;

            // Actualise le texte
            countText.text = getCountText();

            // Si on atteint le nombre de points max pour le talent
            if (MyCurrentCount == maxCount)
            {
                // Si le talent à un enfant
                if (childTalent != null)
                {
                    // Déverrouille le talent enfant
                    childTalent.Unlock();
                }
            }

            // Clic possible
            return true;
        }

        // Clic impossible
        return false;
    }
}