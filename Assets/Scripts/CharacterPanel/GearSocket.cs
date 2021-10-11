using UnityEngine;



/// <summary>
/// Classe de gestion des emplacements des équipements du personnage
/// </summary>
public class GearSocket : MonoBehaviour
{
    // Référence sur l'Animator
    public Animator MyAnimator { get; set; }

    // [TODO : useless ?] - Référence sur l'Animator du personnage
    //private Animator parentAnimator;

    // Référence sur le SpriteRenderer
    protected SpriteRenderer spriteRenderer;

    // Référence sur le controller
    private AnimatorOverrideController animatorOverrideController;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // [TODO : useless ?] - Référence sur l'Animator du personnage
        //parentAnimator = GetComponentInParent<Animator>();

        // Référence sur l'Animator
        MyAnimator = GetComponent<Animator>();

        // Référence sur le controller
        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);

        // Surcharge le controller pour écraser les animations utilisées
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }

    /// <summary>
    /// Actualise les paramètres de l'animation  : Virtual pour être écrasée pour les autres classes
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    public virtual void SetDirection(float x, float y)
    {
        MyAnimator.SetFloat("x", x);
        MyAnimator.SetFloat("y", y);
    }

    /// <summary>
    /// [TODO : refactoring Duplicated CODE] Active un layer d'animation (Idle/Walk/Attack)
    /// </summary>
    /// <param name="layerName">Nom du layer à activer</param>
    public void ActivateLayer(string layerName)
    {
        // Boucle sur les layers d'animations
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            // Réinitialise le layer courant
            MyAnimator.SetLayerWeight(i, 0);
        }

        // Active le layer correspond au nom passé en paramètre
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    /// <summary>
    /// Ecrase les animations d'un équipement
    /// </summary>
    /// <param name="animations">Tableau des animations de l'équipement</param>
    public void Equip(AnimationClip[] animations)
    {
        // Actualise la couleur du spriteRenderer
        spriteRenderer.color = Color.white;

        // Animations ATTACK
        animatorOverrideController["Attack_Back"] = animations[0];
        animatorOverrideController["Attack_Front"] = animations[1];
        animatorOverrideController["Attack_Left"] = animations[2];
        animatorOverrideController["Attack_Right"] = animations[3];

        // Animations IDLE
        animatorOverrideController["Idle_Back"] = animations[4];
        animatorOverrideController["Idle_Front"] = animations[5];
        animatorOverrideController["Idle_Left"] = animations[6];
        animatorOverrideController["Idle_Right"] = animations[7];

        // Animations WALK
        animatorOverrideController["Walk_Back"] = animations[8];
        animatorOverrideController["Walk_Front"] = animations[9];
        animatorOverrideController["Walk_Left"] = animations[10];
        animatorOverrideController["Walk_Right"] = animations[11];
    }


    /// <summary>
    /// Réinitialise les animations d'un équipement
    /// </summary>
    public void Unequip()
    {
        // Animations ATTACK
        animatorOverrideController["Attack_Back"] = null;
        animatorOverrideController["Attack_Front"] = null;
        animatorOverrideController["Attack_Left"] = null;
        animatorOverrideController["Attack_Right"] = null;

        // Animations IDLE
        animatorOverrideController["Idle_Back"] = null;
        animatorOverrideController["Idle_Front"] = null;
        animatorOverrideController["Idle_Left"] = null;
        animatorOverrideController["Idle_Right"] = null;

        // Animations WALK
        animatorOverrideController["Walk_Back"] = null;
        animatorOverrideController["Walk_Front"] = null;
        animatorOverrideController["Walk_Left"] = null;
        animatorOverrideController["Walk_Right"] = null;

        // Couleur du spriteRenderer
        Color spriteColor = spriteRenderer.color;

        // Ajoute la transparence
        spriteColor.a = 0;

        // Actualise la couleur du spriteRenderer
        spriteRenderer.color = spriteColor;
    }
}