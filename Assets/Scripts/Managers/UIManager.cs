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
       
    // Tableau des boutons d'action
    [SerializeField]
    private ActionButton[] actionButtons = default;

    [Header("Menu")]
    // Menu des raccourcis
    [SerializeField]
    private CanvasGroup keyBindMenu = default;

    // Menu des sorts
    [SerializeField]
    private CanvasGroup spellBookMenu = default;

    [Header("Target")]
    // Frame de la cible
    [SerializeField]
    private GameObject targetFrame = default;

    // Image de la cible
    [SerializeField]
    private Image targetPortrait = default;

    // Barre de vie de la cible
    private Stat healthStat;

    // Le menu est-il ouvert ?
    // private bool isOpenMenu;

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
        // Référence sur la barre de vie de la cible
        healthStat = targetFrame.GetComponentInChildren<Stat>();

        // Masque la frame de la cible
        HideTargetFrame();

        // Masque le menu
        OpenClose(keyBindMenu);
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // [Esc] : Menu Raccourci
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Ouverture/Fermeture du menu des raccourcis
            OpenClose(keyBindMenu);
            /*
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
            */
        }

        // [P] : Menu Sorts
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Ouverture/Fermeture du menu des sorts
            OpenClose(spellBookMenu);
        }

        // [B] : Sacs
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Ouverture/Fermeture de tous les sacs de l'inventaire
            InventoryScript.MyInstance.OpenClose();
        }
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
        target.HealthChangedEvent += new HealthChanged(UpdateTargetFrame);

        // Abonnement sur l'évènement de disparition du personnage
        target.CharacterRemovedEvent += new CharacterRemoved(HideTargetFrame);
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
    /*
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
    */

    /// <summary>
    /// Mise à jour du texte des touches
    /// </summary>
    /// <param name="key">Identifiant de la touche</param>
    /// <param name="code">Touche</param>
    public void UpdateKeyText(string key, KeyCode code)
    {
        // Retourne le texte du bouton qui a le même nom que la clé
        Text buttonText = Array.Find(keyBindButtons, button => button.name.ToUpper() == (key + "_Button").ToUpper()).GetComponentInChildren<Text>();

        // Retourne le texte du bouton qui a le même nom que la clé
        buttonText.text = (code == KeyCode.None ? "" : code.ToString());
    }

    /// <summary>
    /// Clic sur un bouton d'action
    /// </summary>
    /// <param name="buttonName">Nom du bouton</param>
    public void ClickActionButton(string buttonName)
    {
        // Déclenchement du clic sur le bouton d'action
        Array.Find(actionButtons, actionButton => actionButton.gameObject.name.ToUpper() == (buttonName + "_Button").ToUpper()).MyActionButton.onClick.Invoke();
    }

    /// <summary>
    /// Ouverture/Fermeture d'un menu
    /// </summary>
    /// <param name="canvasGroup"></param>
    public void OpenClose(CanvasGroup canvasGroup)
    {
        // Bloque/débloque les interactions
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;

        // Masque(0) /Affiche(1) le menu
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        // Début(0)/fin(1) de pause
        //Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    /// <summary>
    /// Mise à jour du nombre d'éléments de l'emplacement de l'item cliquable
    /// </summary>
    /// <param name="clickable"></param>
    public void UpdateStackSize(IClickable clickable)
    {
        // S'il y a plus d'un élément
        if (clickable.MyCount > 1)
        {
            // Actualise le texte du nombre d'éléments de l'item
            clickable.MyStackText.text = clickable.MyCount.ToString();

            // Actualise la couleur du texte
            clickable.MyStackText.color = Color.white;

            // Actualise la couleur de l'image
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            // Réinitialisation du texte du nombre d'éléments de l'item
            clickable.MyStackText.text = null;

            // Réinitialisation de la couleur du texte du nombre d'éléments de l'item
            clickable.MyStackText.color = new Color(0, 0, 0, 0);

            // Actualise la couleur de l'image
            clickable.MyIcon.color = Color.white;
        }

        // S'il n'y a plus d'élément
        if (clickable.MyCount == 0)
        {
            // Réinitialisation de l'image de l'emplacement
            clickable.MyIcon.sprite = null;

            // Réinitialisation de la couleur de l'emplacement
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
        }
    }

}