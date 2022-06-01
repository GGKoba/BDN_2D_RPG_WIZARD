using System;
using UnityEngine;



/// <summary>
/// Classe de l'objet "Sort"
/// </summary>
[Serializable]
public class Spell : IUseable, IMoveable, IDescribable, ICastable
{
    // Nom du sort
    [SerializeField]
    private string title = default;

    // Propri�t� d'acc�s au nom du sort 
    public string MyTitle { get => title; }

    // D�g�ts du sort
    [SerializeField]
    private float damage = default;

    // Propri�t� d'acc�s aux d�g�ts du sort
    public float MyDamage { get => Mathf.Ceil(damage); set => damage = value; }

    // Port�e du sort
    [SerializeField]
    private float range = default;

    // Propri�t� d'acc�s sur la port�e du sort
    public float MyRange { get => range; set => range = value; }

    // Description du sort
    [SerializeField]
    private string description = default;

    // Image du sort
    [SerializeField]
    private Sprite icon = default;

    // Propri�t� d'acc�s � l'image du sort 
    public Sprite MyIcon { get => icon; }

    // Vitesse du sort
    [SerializeField]
    private float speed = default;

    // Propri�t� d'acc�s � la vitesse du sort 
    public float MySpeed { get => speed; }

    // Temps d'incantation du sort
    [SerializeField]
    private float castTime = default;

    // Propri�t� d'acc�s au temps d'incantation du sort
    public float MyCastTime { get => castTime; set => castTime = value; }

    // Prefab du sort
    [SerializeField]
    private GameObject prefab = default;

    // Propri�t� d'acc�s � la prefab du sort 
    public GameObject MyPrefab { get => prefab; }

    // Couleur de la barre de sort
    [SerializeField]
    private Color barColor = default;

    // Propri�t� d'acc�s � la couleur de la barre 
    public Color MyBarColor { get => barColor; }

    // Propri�t� d'acc�s au d�buff du sort
    public Debuff MyDebuff { get; set; }


    /// <summary>
    /// Utilisation du sort
    /// </summary>
    public void Use()
    {
        Player.MyInstance.CastSpell(this);
    }

    /// <summary>
    /// Retourne la description du sort
    /// </summary>
    public string GetDescription()
    {
        string spellTitle = string.Format("<color=#FFD904><b>{0}</b></color>", title);
        string spellStats = string.Format("<color=#ECECEC>Incantation : {0}s</color>", castTime);
        string spellRange = string.Format("<color=#ECECEC>Port�e : {0}m</color>", range);
        string spellDescription = string.Format("<color=#E0D0AE>{0}\net cause <color=cyan>{1}</color> points de d�g�ts</color>", description, MyDamage);

        return string.Format("{0}\n\n{1}\n{2}\n\n{3}", spellTitle, spellStats, spellRange, spellDescription);
    }
}