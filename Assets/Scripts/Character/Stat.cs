using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux barres de vie / mana
/// </summary>
public class Stat : MonoBehaviour
{
    // Vitesse de remplissage
    [SerializeField]
    private float lerpSpeed = default;

    // Référence au texte de la barre
    [SerializeField]
    private Text statValue = default;

    // L'image que l'on va remplir
    private Image content;

    // Propriété sur l'indicateur remplissage complet de la barre de stat 
    public bool IsFull { get => content.fillAmount == 1; }

    // Conserve la valeur de remplissage actuelle
    private float currentFill;

    // Surplus de valeur
    private float overflow;

    // Propriété d'accès à la valeur maximum
    public float MyOverflow
    {
        get
        {
            float tmp = overflow;

            // Réinitalise le surplus
            overflow = 0;

            return tmp;
        }
    }

    // Stat CurrentValue : vie/mana courant
    private float currentValue;

    // Propriété d'accès à la valeur maximum
    public float MyMaxValue { get; set; }

    /// <summary>
    /// Propriété pour renseigner la valeur courante, doit être utilisée chaque fois que que la currentValue change, de sorte que tout se mette à jour correctement
    /// </summary>
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            // Si la valeur dépasse 0, on renseigne 0
            if (value < 0)
            {
                currentValue = 0;
            }
            // Si la valeur dépasse le max, on renseigne le max
            else if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;

                // Actualise le "surplus"
                overflow = value - MyMaxValue;
            }
            // Si la valeur est comprise entre 0 et le max
            else
            {
                currentValue = value;
            }

            // Calcul le remplissage courante
            currentFill = currentValue / MyMaxValue;

            // S'il y a une valeur textuelle à mettre à jour
            if (statValue != null)
            {
                // Mise à jour de la valeur textuelle
                statValue.text = currentValue + " / " + MyMaxValue;
            }
        }
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence à l'image de la barre
        content = gameObject.GetComponent<Image>();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // Mise à jour des barres
        HandleBar();
    }

    /// <summary>
    /// Initialise les barres
    /// </summary>
    /// <param name="currentValue">Valeur courante de la barre</param>
    /// <param name="maxValue">Valeur max de la barre</param>
    public void Initialize(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;

        // Si l'image de la barre n'est pas référencée
        if (content == null)
        {
            // Référence à l'image de la barre
            content = gameObject.GetComponent<Image>();
        }

        // Valeur de remplissage de la barre
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    /// <summary>
    /// Mise à jour des barres
    /// </summary>
    private void HandleBar()
    {
        // Si on a une nouvelle valeur de remplissage, on doir mettre à jour les barres
        if (currentFill != content.fillAmount)
        {
            // Actualise le montant de remplissage afin d'obtenir un mouvement en douceur
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    /// <summary>
    /// Réinitialise le remplissage de la barre
    /// </summary>
    public void ResetBar()
    {
        // Vide le remplissage 
        content.fillAmount = 0;
    }
}
