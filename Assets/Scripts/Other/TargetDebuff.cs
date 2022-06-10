using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de l'affichage des d�buffs
/// </summary>
public class TargetDebuff : MonoBehaviour
{
    // Image du compteur de dur�e
    [SerializeField]
    private Image durationImage = default;

    // Image du d�buff
    [SerializeField]
    private Image icon = default;

    // Propri�t� d'acc�s au d�buff
    public Debuff MyDebuff { get; private set; }

    /// <summary>
    /// Initialise les informations
    /// </summary>
    /// <param name="debuff">Debuff courant</param>
    public void Initialize(Debuff debuff)
    {
        // Initialise le d�buff
        MyDebuff = debuff;

        // Initialise l'image
        icon.sprite = debuff.MyIcon.sprite;

        // Initialise l'image du compteur de dur�e
        durationImage.fillAmount = 0;
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        // Actualise l'image du compteur de dur�e (remplissage en fonction du temps : temps pass� sur temps total)
        durationImage.fillAmount = MyDebuff.MyElapsed / MyDebuff.MyDuration;
    }
}
