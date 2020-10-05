using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de l'arbre des talents
/// </summary>
public class TalentTree : Window
{
    // Tableau des talents
    [SerializeField]
    private Talent[] talents = default;

    // Tableau des talents déverouillés par défaut
    [SerializeField]
    private Talent[] unlockedByDefault = default;

    // Nombre de points de talent
    private int points = 10;

    // Propriété d'accès au tableau des talents
    public Talent[] MyTalents
    {
        get => talents;
    }

    // Propriété d'accès au nombre de points de talent
    public int MyPoints
    {
        get => points;
        set
        { 
            points = value;
            
            // Actualise le nombre de points
            UpdateTalentPointText();
        }
    }

    // Texte du total des points
    [SerializeField]
    private Text talentPointText;


    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        // Désactivation des talents
        ResetTalents();
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Réinitialisation des talents
    /// </summary>
    public void TryUseTalent(Talent talent)
    {
        // Si on a des points à utiliser
        if (MyPoints > 0 && talent.Click())
        {
            // Décrémentation du compteur
            MyPoints--;
        }

        // Si on n'a plus de points
        if (MyPoints == 0)
        {
            // Pour tous les talents
            foreach (Talent t in talents)
            {
                // Si le talent n'a pas de point
                if (t.MyCurrentCount == 0)
                {
                    // Verrouille du talent
                    t.Lock();
                }
            }

        }

    }

    /// <summary>
    /// Réinitialisation des talents
    /// </summary>
    private void ResetTalents()
    {
        // Actualise le compteur des points
        UpdateTalentPointText();

        // Pour tous les talents
        foreach (Talent talent in talents)
        {
            // Désactivation du talent
            talent.Lock();
        }

        // Pour tous les talents à déverouiller
        foreach (Talent talent in unlockedByDefault)
        {
            // Activation du talent
            talent.Unlock();
        }
    }

    /// <summary>
    /// Actualisation du nombre de points
    /// </summary>
    private void UpdateTalentPointText()
    {
        // MAJ du nombre de points
        talentPointText.text = MyPoints.ToString();
    }
}
