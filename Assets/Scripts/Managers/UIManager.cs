using System;
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

    // Menu des raccourcis
    [SerializeField]
    private CanvasGroup keyBindMenu = default;

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

    // Le menu est-il ouvert ?
    private bool isOpenMenu;

    // Tableau des boutons des touches
    private GameObject[] keyBindButtons;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur les boutons des touches
        keyBindButtons = GameObject.FindGameObjectsWithTag("KeyBind");
    }

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

        //Masque le menu
        CloseMenu();
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

        // Si on presse la touche Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpenMenu)
            {
                // Fermeture du menu
                CloseMenu();
            }
            else
            {
                // Ouverture du menu
                OpenMenu();
            }
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

    /// <summary>
    /// Ouverture du menu
    /// </summary>
    public void OpenMenu()
    {
        // Affiche le menu
        keyBindMenu.alpha = 1;

        // Bloque les interactions
        keyBindMenu.blocksRaycasts = true;

        // Début de pause
        Time.timeScale = 0;

        // Définit le menu comme ouvert
        isOpenMenu = true;
    }

    /// <summary>
    /// Fermeture du menu
    /// </summary>
    public void CloseMenu()
    {
        // Masque le menu
        keyBindMenu.alpha = 0;

        // Débloque les interactions
        keyBindMenu.blocksRaycasts = false;

        // Fin de pause
        Time.timeScale = 1;

        // Définit le menu comme fermé
        isOpenMenu = false;
    }

    /// <summary>
    /// Mise à jour du texte des touches
    /// </summary>
    /// <param name="key">Identifiant de la touche</param>
    /// <param name="code">Touche</param>
    public void UpdateKeyText(string key, KeyCode code)
    {
        // Retourne le texte du bouton qui a le même nom que la clé
        Text buttonText = Array.Find(keyBindButtons, button => button.name == key + "_Button").GetComponentInChildren<Text>();

        // Retourne le texte du bouton qui a le même nom que la clé
        buttonText.text = code.ToString();
    }
}
