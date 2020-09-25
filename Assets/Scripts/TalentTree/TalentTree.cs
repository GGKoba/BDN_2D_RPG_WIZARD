using UnityEngine;



/// <summary>
/// Classe de gestion de l'arbre des talents
/// </summary>
public class TalentTree : MonoBehaviour
{
    // Tableau des talents
    [SerializeField]
    private Talent[] talents = default;

    // Tableau des talents déverouillés par défaut
    [SerializeField]
    private Talent[] unlockedByDefault = default;


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
    private void ResetTalents()
    {
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
}
