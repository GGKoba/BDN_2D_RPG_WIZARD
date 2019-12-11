using UnityEngine;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux ennemis
/// </summary>
public class Enemy : NPC
{
    // Canvas de la barre de vie
    [SerializeField]
    private CanvasGroup healthGroup = default;

    // Etat courant de l'ennemi
    private IState currentState;

    // Cible de l'ennemi
    private Transform target;

    // Propriété d'accès à la cible de l'ennemi
    public Transform MyTarget { get => target; set => target = value; }


    /// <summary>
    /// Awake
    /// </summary>
    protected void Awake()
    {
        ChangeState(new IdleState());
    }
    
    /// <summary>
    /// Start : Surcharge la fonction Start du script NPC
    /// </summary>
    protected override void Start()
    {
        // Référence sur CanvasGroup de la barre de cast du joueur
        healthGroup.alpha = 0;

        // Appelle Start sur la classe mère
        base.Start();
    }

    /// <summary>
    /// Update : Surcharge la fonction Update du script NPC
    /// </summary>
    protected override void Update()
    {
        // Appelle Update sur l'état courant
        currentState.Update();

        // Appelle Update sur la classe mère
        base.Update();
    }

    /// <summary>
    /// Désélection d'un ennemi : Surcharge la fonction Select du script NPC
    /// </summary>
    public override void DeSelect()
    {
        // Masque la barre de vie
        healthGroup.alpha = 0;

        // Appelle DeSelect sur la classe mère
        base.DeSelect();    
    }

    /// <summary>
    /// Sélection d'un ennemi : Surcharge la fonction Select du script NPC
    /// </summary>
    public override Transform Select()
    {
        // Affiche la barre de vie
        healthGroup.alpha = 1;

        // Appelle Select sur la classe mère
        return base.Select();
    }

    /// <summary>
    /// Dégâts liée à une attaque : Surcharge la fonction TakeDamage du script Character
    /// </summary>
    /// <param name="damage">Montant des dégâts</param>
    public override void TakeDamage(float damage)
    {
        // Appelle TakeDamage sur la classe mère
        base.TakeDamage(damage);

        // Déclenche l'évènement de changement de la vie
        OnHealthChanged(health.MyCurrentValue);
    }

    /// <summary>
    /// Change l'état de l'ennemi
    /// </summary>
    /// <param name="state">Etat de l'ennemi</param>
    public void ChangeState(IState state)
    {
        // S'il y a un état;
        if (currentState != null)
        {
            // Sortie de l'ancien état
            currentState.Exit();
        }

        // Mise à jour de l'état courant
        currentState = state;

        // Entrée dans le nouvel état
        currentState.Enter(this);
    }
}