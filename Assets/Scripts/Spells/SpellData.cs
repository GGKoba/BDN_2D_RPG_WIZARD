using System;
using UnityEngine;



/// <summary>
/// Classe des sorts
/// </summary>
[Serializable]
public class SpellData : IUseable, IMoveable, IDescribable
{
    // Nom du sort
    [SerializeField]
    private string name = default;

    // Dégâts du sort
    [SerializeField]
    private int damage = default;

    // Description du sort
    [SerializeField]
    private string description = default;

    // Image du sort
    [SerializeField]
    private Sprite icon = default;

    // Vitesse du sort
    [SerializeField]
    private float speed = default;

    // Temps d'incantation du sort
    [SerializeField]
    private float castTime = default;

    // Prefab du sort
    [SerializeField]
    private GameObject prefab = default;

    // Couleur de la barre de sort
    [SerializeField]
    private Color barColor = default;


    /// <summary>
    /// Accesseurs sur les propriétés du sort
    /// </summary>

    // Propriété d'accès au nom du sort 
    public string MyName { get => name; }

    // Propriété d'accès aux dégâts du sort
    public int MyDamage { get => damage; }

    // Propriété d'accès à l'image du sort 
    public Sprite MyIcon { get => icon; }

    // Propriété d'accès à la vitesse du sort 
    public float MySpeed { get => speed; }

    // Propriété d'accès au temps d'incantation du sort
    public float MyCastTime { get => castTime; }

    // Propriété d'accès à la prefab du sort 
    public GameObject MyPrefab { get => prefab; }

    // Propriété d'accès à la couleur de la barre 
    public Color MyBarColor { get => barColor; }


    /// <summary>
    /// Utilisation du sort
    /// </summary>
    public void Use()
    {
        Player.MyInstance.CastSpell(MyName);
    }

    /// <summary>
    /// Retourne la description du sort
    /// </summary>
    public string GetDescription()
    {
        string spellTitle = string.Format("<color=#FFD904><b>{0}</b></color>", name);
        string spellStats = string.Format("<color=#ECECEC>Incantation : {0}s</color>", castTime);
        string spellDescription = string.Format("<color=#E0D0AE>{0}\net cause <color=cyan>{1}</color> points de dégâts</color>", description, damage);

        return string.Format("{0}\n\n{1}\n\n{2}", spellTitle, spellStats, spellDescription);
    }
}