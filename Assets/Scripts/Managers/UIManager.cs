using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de l'interface
/// </summary>
public class UIManager : MonoBehaviour
{
    // Instance de classe (singleton)
    private static UIManager instance;

    // Propriété d'accès à l'instance
    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type UIManager (doit être unique)
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    // Frame de la cible
    [SerializeField]
    private GameObject targetFrame = default;

    // Image de la cible
    [SerializeField]
    private Image targetPortrait = default;

    // Tableau des boutons d'action
    [SerializeField]
    private Button[] actionButtons = default;

    // Liste des touches liées aux actions
    private KeyCode action1, action2, action3;

    // Barre de vie de la cible
    private Stat healthStat;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Binding des touches
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;

        // Référence sur la barre de vie de la cible
        healthStat = targetFrame.GetComponentInChildren<Stat>();

        // Masque la frame de la cible
        HideTargetFrame();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // Si on presse la touche d'action 1 (touche 1)
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }

        // Si on presse la touche d'action 2 (touche 2)
        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }

        // Si on presse la touche d'action 3 (touche 3)
        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
    }

    /// <summary>
    /// Clic sur un bouton d'action
    /// </summary>
    /// <param name="buttonIndex">Index du bouton d'action</param>
    private void ActionButtonOnClick(int buttonIndex)
    {
        // Appelle la fonction Onclick du bouton lié à l'action
        actionButtons[buttonIndex].onClick.Invoke();
    }

    /// <summary>
    /// Affichage la frame de la cible
    /// </summary>
    /// <param name="target">Cible de type NPC</param>
    public void ShowTargetFrame(NPC target)
    {
        // Active la frame de la cible
        targetFrame.SetActive(true);

        // Initialise la barre de vie de la cible
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        // Mise à jour de l'image de la cible (1er enfant de l'objet TargetFrame => face) ==> targetFrame.transform.GetChild(0).GetComponent<Image>();
        targetPortrait.sprite = target.MyPortrait;

        // Abonnement sur l'évènement de changement de vie
        target.HealthChanged += new HealthChanged(UpdateTargetFrame);

        // Abonnement sur l'évènement de disparition du personnage
        target.CharacterRemoved += new CharacterRemoved(HideTargetFrame);
    }

    /// <summary>
    /// Masquage la frame de la cible
    /// </summary>
    public void HideTargetFrame()
    {
        // Désactive la frame de la cible
        targetFrame.SetActive(false);
    }

    /// <summary>
    /// Mise à jour de la frame de la cible
    /// </summary>
    /// <param name="health">Vie de la cible</param>
    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }
}
