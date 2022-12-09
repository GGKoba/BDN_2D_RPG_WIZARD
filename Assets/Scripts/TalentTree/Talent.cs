using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des talents
/// </summary>
public class Talent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDescribable
{
    // Image du talent
    protected Image icon;

    // Texte du compteur
    [SerializeField]
    private Text countText = default;

    // Point(s) maximum sur le talent
    [SerializeField]
    private int maxCount = 0;

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
        icon = GetComponent<Image>();

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
        icon.color = Color.gray;

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
        icon.color = Color.white;

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

    /// <summary>
    /// Clic sur le talent : Virtual pour être écrasée pour les autres classes
    /// </summary>
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


    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Affiche le tooltip
        UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, this);
    }

    /// <summary>
    /// Sortie du curseur
    /// </summary>
    /// <param name="eventData">Evenement de sortie</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Masque le tooltip
        UIManager.MyInstance.HideTooltip();
    }

    /// <summary>
    /// Description du talent : Virtual pour être écrasée pour les autres classes
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        return string.Empty;
    }
}