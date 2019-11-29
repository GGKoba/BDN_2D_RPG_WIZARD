using UnityEngine;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux ennemis
/// </summary>
public class Enemy : NPC
{
    // Canvas de la barre de vie
    [SerializeField]
    private CanvasGroup healthGroup = default;


    /// <summary>
    /// Start
    /// </summary>
    protected override void Start()
    {
        // Référence sur CanvasGroup de la barre de cast du joueur
        healthGroup.alpha = 0;

        // Appel Start sur la classe mère
        base.Start();
    }

    /// <summary>
    /// Désélection d'un ennemi : Ecrase la fonction Select du script NPC
    /// </summary>
    public override void DeSelect()
    {
        // Masque la barre de vie
        healthGroup.alpha = 0;

        // Appel DeSelect sur la classe mère
        base.DeSelect();    
    }

    /// <summary>
    /// Sélection d'un ennemi : Ecrase la fonction Select du script NPC
    /// </summary>
    public override Transform Select()
    {
        // Affiche la barre de vie
        healthGroup.alpha = 1;

        // Appel Select sur la classe mère
        return base.Select();
    }
}