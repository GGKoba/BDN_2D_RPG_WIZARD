using System.Collections;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe des textes de combats
/// </summary>
public class CombatText : MonoBehaviour
{
    // Vitesse des textes
    [SerializeField]
    private float speed = default;

    // Temps d'affichage du texte
    [SerializeField]
    private float lifeTime = default;

    // Référence sur le conteneur du texte
    // [SerializeField]
    private Text text;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence sur le conteneur du texte
        text = GetComponent<Text>();

        // Démarre la routine de disparition du texte
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Déplacement des textes
    /// </summary>
    private void Move()
    {
        // Déplace le texte
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    /// <summary>
    /// Routine de disparition du texte
    /// </summary>
    private IEnumerator FadeOut()
    {
        // Transparence initiale du texte
        float startAlpha = text.color.a;

        // Vitesse de disparition du texte
        float rate = 1.0f / lifeTime;

        // Progression courante de la disparition du texte
        float progress = 0.0f;

        // Tant que la disparition du texte n'est pas terminée
        while (progress <= 1.0)
        {
            Color tmpColor = text.color;

            // Mise à jour transparence du texte
            tmpColor.a = Mathf.Lerp(startAlpha, 0, progress);

            // Rafrachissement de la transparence du texte
            text.color = tmpColor;

            // Mise à jour de la progression
            progress += rate * Time.deltaTime;

            yield return null;
        }

        // Destruction de l'objet
        Destroy(gameObject);
    }
}