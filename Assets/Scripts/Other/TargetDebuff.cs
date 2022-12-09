using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de l'affichage des débuffs
/// </summary>
public class TargetDebuff : MonoBehaviour
{
    // Image du compteur de durée
    [SerializeField]
    private Image durationImage = default;

    // Image du débuff
    [SerializeField]
    private Image icon = default;

    // Image du débuff
    [SerializeField]
    private Text timeText = default;



    // Propriété d'accès au débuff
    public Debuff MyDebuff { get; private set; }

    /// <summary>
    /// Initialise les informations
    /// </summary>
    /// <param name="debuff">Debuff courant</param>
    public void Initialize(Debuff debuff)
    {
        // Initialise le débuff
        MyDebuff = debuff;

        // Initialise l'image
        icon.sprite = debuff.MyIcon.sprite;

        // Initialise l'image du compteur de durée
        durationImage.fillAmount = 0;

        // Initialise la durée du débuff
        timeText.text = $"{string.Format("{0:0.0}", MyDebuff.MyDuration)}s";
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        // Actualise l'image du compteur de durée (remplissage en fonction du temps : temps passé sur temps total)
        durationImage.fillAmount = MyDebuff.MyElapsed / MyDebuff.MyDuration;

        // Actualise la durée du débuff
        timeText.text = $"{string.Format("{0:0.0}", MyDebuff.MyDuration - MyDebuff.MyElapsed)}s";
    }
}
