using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe abstraite dont tous les débuffs héritent
/// </summary>
public abstract class Debuff
{
    // Propriété d'accès à durée du débuff
    public float MyDuration { get; set; }

    // Propriété d'accès à la chance de proc du débuff
    public float MyProcChance { get; set; }

    // Propriété d'accès à l'image du débuff
    public Image MyIcon { get; set; }


    // Propriété d'accès au nom du débuff : Abstraite pour être réécrite par ses enfants
    public abstract string MyName { get; }

    // Propriété d'accès au yemps d'application du débuff
    public float MyElapsed { get; set; }

    // Personnage portant le débuff
    protected Character character;


    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="image">Image du débuff</param>
    public Debuff(Image image)
    {
        // Mise à jour de l'image du débuff
        MyIcon = image;
    }

    /// <summary>
    /// Update : Virtual pour être écrasée pour les autres classes
    /// </summary>
    public virtual void Update()
    {
        // Actualise le temps d'application du débuff
        MyElapsed += Time.deltaTime;

        // Si le temps du débuff est écoulé
        if (MyElapsed >= MyDuration)
        {
            // Retire le debuff
            Remove();
        }
    }

    /// <summary>
    /// Apply : Virtual pour être écrasée pour les autres classes (Applique le debuff)
    /// </summary>
    /// <param name="characterDebuffed">Personnage affecté par le débuff</param>
    public virtual void Apply(Character characterDebuffed)
    {
        // Actualise le personnage portant le débuff
        character = characterDebuffed;

        // Applique le débuff au personnage
        characterDebuffed.ApplyDebuff(this);

        // Actualise la frame de la cible
        UIManager.MyInstance.AddDebuffToTargetFrame(this);
    }


    /// <summary>
    /// Remove : Virtual pour être écrasée pour les autres classes (Retire le debuff)
    /// </summary>
    public virtual void Remove()
    {
        // Retire le débuff au personnage
        character.RemoveDebuff(this);

        // Réinitialise le temps d'application du débuff
        MyElapsed = 0;
    }

    /// <summary>
    /// Clone le debuff actuel : Abstraite pour être réécrite par ses enfants
    /// </summary>
    /// <returns></returns>
    public abstract Debuff Clone();
}