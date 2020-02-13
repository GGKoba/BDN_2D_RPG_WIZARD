using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des sauvegardes
/// </summary>
public class SavedGame : MonoBehaviour
{
    // Date de la sauvegarde
    [SerializeField]
    private Text dateTime = default;

    // Image de la barre de vie
    [SerializeField]
    private Image health = default;

    // Image de la barre de mana
    [SerializeField]
    private Image mana = default;

    // Image de la barre d'expérience
    [SerializeField]
    private Image xp = default;

    // Texte de la barre de vie
    [SerializeField]
    private Text healthText = default;

    // Texte de la barre de mana
    [SerializeField]
    private Text manaText = default;

    // Texte de la barre d'expérience
    [SerializeField]
    private Text xpText = default;

    // Texte du niveau
    [SerializeField]
    private Text levelText = default;

    // Visuels à masquer
    [SerializeField]
    private GameObject visuals = default;

    // Index de la sauvegarde
    [SerializeField]
    private int index = default;

    // Propriété d'accès à l'index de la sauvegarde
    public int MyIndex { get => index; }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        HideVisuals();
    }

    /// <summary>
    /// Affiche les informations de la sauvegarde
    /// </summary>
    public void ShowInfo(SaveData saveData)
    {
        // Affiche les visuels
        visuals.SetActive(true);

        // Informations sur la date de sauvegarde
        dateTime.text = "Date : " + saveData.MyDateTime.ToString("dd/MM/yyyy") + " - " + saveData.MyDateTime.ToString("HH:mm");

        // Informations sur la vie du joueur
        healthText.text = saveData.MyPlayerData.MyHealth + "/" + saveData.MyPlayerData.MyMaxHealth;

        // Remplissage de la barre de vie
        health.fillAmount = saveData.MyPlayerData.MyHealth / saveData.MyPlayerData.MyMaxHealth;

        // Informations sur la mana du joueur
        manaText.text = saveData.MyPlayerData.MyMana + "/" + saveData.MyPlayerData.MyMaxMana;

        // Remplissage de la barre de mana
        mana.fillAmount = saveData.MyPlayerData.MyMana / saveData.MyPlayerData.MyMaxMana;

        // Informations sur l'expérience du joueur
        xpText.text = saveData.MyPlayerData.MyXp + "/" + saveData.MyPlayerData.MyMaxXp;

        // Remplissage de la barre d'expérience
        xp.fillAmount = saveData.MyPlayerData.MyXp / saveData.MyPlayerData.MyMaxXp;

        // Informations sur le niveau du joueur
        levelText.text = saveData.MyPlayerData.MyLevel.ToString();
    }

    public void HideVisuals()
    {
        // Masque les visuels
        visuals.SetActive(false);
    }

}
