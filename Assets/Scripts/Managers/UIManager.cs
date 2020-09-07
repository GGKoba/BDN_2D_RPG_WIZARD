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

    // Tableau des menus
    [SerializeField]
    private CanvasGroup[] menus = default;
/*
    [Header("Menu")]
    // Menu des raccourcis
    [SerializeField]
    private CanvasGroup keyBindMenu = default;

    // Menu des sorts
    [SerializeField]
    private CanvasGroup spellBookMenu = default;

    // Feuille du personnage
    [SerializeField]
    private CharacterPanel characterPanel = default;
*/
    [Header("Target")]
    // Frame de la cible
    [SerializeField]
    private GameObject targetFrame = default;

    // Image de la cible
    [SerializeField]
    private Image targetPortrait = default;

    // Texte du niveau de la cible
    [SerializeField]
    private Text levelText = default;

    [Header("Tooltip")]
    // Tooltip
    [SerializeField]
    private GameObject tooltip = default;

    // Référence sur le RectTransform du tooltip
    [SerializeField]
    private RectTransform tooltipRect = default;

    // Texte du tooltip
    private Text tooltipText;

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

        // Référence sur le texte du tooltip
        tooltipText = tooltip.GetComponentInChildren<Text>();
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

        // Masque les menus
        foreach (CanvasGroup menu in menus)
        {
            CloseMenu(menu);
        }

    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // [Esc] : Menu Raccourci
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Ouverture/Fermeture du menu principal
            OpenClose(menus[0]);
        }
        
        // [P] : Menu Sorts
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Ouverture/Fermeture du menu des sorts
            OpenClose(menus[1]);
        }

        // [C] : Profil
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Ouverture/Fermeture de la feuille du personnage
            OpenClose(menus[2]);
        }

        // [N] : Quêtes
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Ouverture/Fermeture de la fenêtre des quêtes
            OpenClose(menus[3]);
        }

        // [B] : Sacs
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Ouverture/Fermeture de tous les sacs de l'inventaire
            InventoryScript.MyInstance.OpenClose();
        }

        // [V] : Craft
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Ouverture/Fermeture de la fenêtre de fabrication
            OpenClose(menus[6]);
        }

    }

    /// <summary>
    /// Affichage la frame de la cible
    /// </summary>
    /// <param name="target">Cible de type NPC</param>
    public void ShowTargetFrame(Enemy target)
    {
        // Active la frame de la cible
        targetFrame.SetActive(true);

        // Initialise la barre de vie de la cible
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        // Mise à jour de l'image de la cible (1er enfant de l'objet TargetFrame => face) ==> targetFrame.transform.GetChild(0).GetComponent<Image>();
        targetPortrait.sprite = target.MyPortrait;

        // Actualise le texte du niveau de la cible
        levelText.text = target.MyLevel.ToString();

        // Abonnement sur l'évènement de changement de vie
        target.HealthChangedEvent += new HealthChanged(UpdateTargetFrame);

        // Abonnement sur l'évènement de disparition du personnage
        target.CharacterRemovedEvent += new CharacterRemoved(HideTargetFrame);

        // Adapte la couleur du niveau en fonction de celui du joueur
        SetColorLevelText(target);
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
    /// Ouverture du menu
    /// </summary>
    /// <param name="menu">Menu à ouvrir</param>
    public void OpenMenu(CanvasGroup menu)
    {
        // Pour tous les menus
        foreach (CanvasGroup canvas in menus)
        {
            // Fermeture du menu
            CloseMenu(canvas);
        }

        //Ouverture / Fermeture du menu
        OpenClose(menu);
    }

    /// <summary>
    /// Fermeture du menu
    /// </summary>
    /// <param name="menu">Menu à fermer</param>
    public void CloseMenu(CanvasGroup menu)
    {
        // Masque le menu
        menu.alpha = 0;

        // Débloque les interactions
        menu.blocksRaycasts = false;
    }

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

            // Affiche le compteur sur l'image
            clickable.MyStackText.enabled = true;

            // Affiche l'image
            clickable.MyIcon.enabled = true;
        }
        else
        {
            // Réinitialise le texte du nombre d'éléments de l'item
            ClearStackCount(clickable);
        }

        // S'il n'y a plus d'élément
        if (clickable.MyCount == 0)
        {
            // Masque le compteur sur l'image
            clickable.MyStackText.enabled = false;

            // Masque l'image
            clickable.MyIcon.enabled = false;
        }
    }

    /// <summary>
    /// Réinitialise le texte du nombre d'éléments de l'item
    /// </summary>
    public void ClearStackCount(IClickable clickable)
    {
        // Réinitialisation du texte du nombre d'éléments de l'item
        clickable.MyStackText.text = null;

        // Masque le compteur sur l'image
        clickable.MyStackText.enabled = false;

        // Affiche l'image
        clickable.MyIcon.enabled = true;
    }


    /// <summary>
    /// Affiche le tooltip
    /// </summary>
    /// <param name="pivot">Pivot du tooltip</param>
    /// <param name="position">Position du tooltip</param>
    /// <param name="itemDescription">Description du tooltip</param>
    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable itemDescription)
    {
        // Pivot du tooltip
        tooltipRect.pivot = pivot;

        // Position du tooltip
        tooltip.transform.position = position;

        // Description du tooltip
        tooltipText.text = itemDescription.GetDescription();

        // Affichage du tooltip
        tooltip.SetActive(true);
    }

    /// <summary>
    /// Masque le tooltip
    /// </summary>
    public void HideTooltip()
    {
        // Masquage du tooltip
        tooltip.SetActive(false);
    }

    /// <summary>
    /// Actualise le tooltip
    /// </summary>
    /// <param name="itemDescription">Description du tooltip</param>
    public void RefreshTooltip(IDescribable itemDescription)
    {
        // Actualise le text du tooltîp
        tooltipText.text = itemDescription.GetDescription();
    }

    /// <summary>
    /// Adapte la couleur du niveau en fonction de celui du joueur
    /// </summary>
    /// <param name="target">Cible du joueur</param>
    private void SetColorLevelText(Enemy target)
    {
        // Adapte la couleur du niveau en fonction de celui du joueur
        if (target.MyLevel >= Player.MyInstance.MyLevel + 5)
        {
            levelText.color = Color.red;
        }
        else if (target.MyLevel == Player.MyInstance.MyLevel + 3 || target.MyLevel == Player.MyInstance.MyLevel + 4)
        {
            levelText.color = new Color32(255, 160, 0, 255);
        }
        else if (target.MyLevel >= Player.MyInstance.MyLevel - 2 && target.MyLevel <= Player.MyInstance.MyLevel + 2)
        {
            levelText.color = Color.yellow;
        }
        else if (target.MyLevel <= Player.MyInstance.MyLevel - 3 && target.MyLevel > XPManager.CalculateGrayLevel())
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.grey;
        }
    }
}