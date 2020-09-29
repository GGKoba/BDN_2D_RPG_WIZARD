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

    // Propriété d'accès au nom du sort 
    public string MyTitle { get => title; }

    // Dégâts du sort
    [SerializeField]
    private int damage = default;

    // Propriété d'accès aux dégâts du sort
    public int MyDamage { get => damage; }

    // Description du sort
    [SerializeField]
    private string description = default;

    // Image du sort
    [SerializeField]
    private Sprite icon = default;

    // Propriété d'accès à l'image du sort 
    public Sprite MyIcon { get => icon; }

    // Vitesse du sort
    [SerializeField]
    private float speed = default;

    // Propriété d'accès à la vitesse du sort 
    public float MySpeed { get => speed; }

    // Temps d'incantation du sort
    [SerializeField]
    private float castTime = default;

    // Propriété d'accès au temps d'incantation du sort
    public float MyCastTime { get => castTime; set => castTime = value; }

    // Prefab du sort
    [SerializeField]
    private GameObject prefab = default;

    // Propriété d'accès à la prefab du sort 
    public GameObject MyPrefab { get => prefab; }

    // Couleur de la barre de sort
    [SerializeField]
    private Color barColor = default;

    // Propriété d'accès à la couleur de la barre 
    public Color MyBarColor { get => barColor; }


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
        string spellDescription = string.Format("<color=#E0D0AE>{0}\net cause <color=cyan>{1}</color> points de dégâts</color>", description, damage);

        return string.Format("{0}\n\n{1}\n\n{2}", spellTitle, spellStats, spellDescription);
    }
}