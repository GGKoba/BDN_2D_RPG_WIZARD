using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe Stat contenant les fonctionnalités spécifiques aux barres de vie / mana
/// </summary>
public class Stat : MonoBehaviour
{
    // Vitesse de remplissage
    [SerializeField]
    private float lerpSpeed;

    // Référence au texte de la barre
    [SerializeField]
    private Text statValue;
    
    // L'image que l'on va remplir
    private Image content;

    // Conserve la valeur de remplissage actuelle
    private float currentFill;

    // Stat CurrentValue : vie/mana courant
    private float currentValue;

    // Stat MaxValue : vie/mana max
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
            }
            // Si la valeur est comprise entre 0 et le max
            else
            {
                currentValue = value;
            }

            // Calcul le remplissage courante
            currentFill = currentValue / MyMaxValue;

            // Mise à jour de la valeur textuelle
            statValue.text = currentValue + " / " + MyMaxValue; 
        }
    }

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        // Référence à l'image de la barre
        content = gameObject.GetComponent<Image>();
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
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
    }

    /// <summary>
    /// Mise à jour des barres
    /// </summary>
    private void HandleBar()
    {
        // Si on a une nouvelle valeur de remplissage, on doir mettre à jour les barres
        if (currentFill != content.fillAmount)
        {
            // Renseigne le montant de remplissage afin d'obtenir un mouvement en douceur
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

}
