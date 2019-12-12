using System.Collections;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe pour la bibiothèque des sorts
/// </summary>
public class SpellBook : MonoBehaviour
{
    // Tableau des sorts
    [SerializeField]
    private Spell[] spells = default;

    [Header("Barre de cast")]
    // Référence à la barre de cast du joueur
    [SerializeField]
    public Image castingBar;

    // Référence à l'icone du sort de la barre de cast du joueur
    [SerializeField]
    private Image icon = default;

    // Référence au texte du nom du sort de la barre de cast du joueur
    [SerializeField]
    private Text spellName = default;

    // Référence au texte du temps d'incantation du sort de la barre de cast du joueur
    [SerializeField]
    private Text castTime = default;

    // Groupe des éléments de la barre de cast du joueur
    [SerializeField]
    private CanvasGroup canvasGroup = default;

    // Routine de remplissage de la barre de cast du joueur
    private Coroutine spellRoutine;

    // Routine d'apparition de la barre de cast du joueur
    private Coroutine fadeRoutine;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence sur CanvasGroup de la barre de cast du joueur
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// Retourne un sort avec ses propriétés
    /// </summary>
    /// <param name="spellIndex">Index du sort</param>
    public Spell CastSpell(int spellIndex)
    {
        // Sort correspondant à l'index dans le tableau
        Spell spell = spells[spellIndex];

        // Initialise le remplissage de la barre à vide
        castingBar.fillAmount = 0;

        // Adapte la couleur de la barre avec celle qui est liée au sort
        castingBar.color = spell.SpellBarColor;

        // Adapte l'icone de la barre avec celle qui est liée au sort
        icon.sprite = spell.SpellIcon;

        // Adapte le nom du sort de la barre avec celui qui est liée au sort
        spellName.text = spell.SpellName;

        // Démarre la routine d'incantation du sort
        spellRoutine = StartCoroutine(Progress(spell.SpellCastTime));

        // Démarre la routine d'apparition de la barre de cast
        fadeRoutine = StartCoroutine(FadeBar());

        // Retourne le sort à incanter
        return spell;
    }

    /// <summary>
    /// Routine de remplissage de la barre de cast en fonction de la progression du temps d'incantation
    /// </summary>
    /// <param name="spellCastTime">Temps d'incantation du sort</param>
    private IEnumerator Progress(float spellCastTime)
    {
        // Temps écoulé depuis le début de l'incantation du sort
        float timePassed = Time.deltaTime;

        // Vitesse de remplissage de la barre de cast
        float rate = 1.0f / spellCastTime;

        // Progression courante de l'incantation du sort
        float progress = 0.0f;

        // Tant que l'on a pas terminé l'incantation du sort
        while (progress <= 1.0)
        {
            // Mise à jour du remplissage de la barre suivant la progression
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            // Mise à jour de la progression
            progress += rate * Time.deltaTime;

            // Mise à jour du temps écoulé
            timePassed += Time.deltaTime;

            // Mise à jour du texte du temps d'incantation du sort (F2 => 0,00)
            castTime.text = (spellCastTime - timePassed).ToString("F2");

            // Vérification que le temps d'incantation ne passe pas au-dessous de 0
            if (spellCastTime - timePassed < 0)
            {
                castTime.text = "0.00";
            }

            yield return null;
        }

        // Termine l'incantion du sort
        StopCasting();
    }

    /// <summary>
    /// Routine d'apparation de la barre de cast
    /// </summary>
    private IEnumerator FadeBar()
    {
        // Vitesse d'apparition de la barre de cast
        float rate = 1.0f / 0.50f;

        // Progression courante de l'apparition de la barre de cast
        float progress = 0.0f;

        // Tant que l'apparition de la barre de cast n'est pas terminée
        while (progress <= 1.0)
        {
            // Mise à jour de l'apparition
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            // Mise à jour de la progression
            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// Termine l'incantion du sort
    /// </summary>
    public void StopCasting()
    {
        // Vérifie qu'il existe une référence à la routine de remplissage de la barre de cast
        if (spellRoutine != null)
        {
            // Arrête la routine de remplissage
            StopCoroutine(spellRoutine);

            // Réinitialise la routine de remplissage
            spellRoutine = null;
        }

        // Vérifie qu'il existe une référence à la routine d'apparition de la barre de cast
        if (fadeRoutine != null)
        {
            // Arrête la routine d'apparition
            StopCoroutine(fadeRoutine);

            // Réinitialise la transparence du canvas
            canvasGroup.alpha = 0;

            // Réinitialise la routine d'apparition
            fadeRoutine = null;
        }
    }
}