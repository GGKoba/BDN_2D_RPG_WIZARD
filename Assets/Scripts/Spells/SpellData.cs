using System;
using UnityEngine;



/// <summary>
/// Classe des sorts
/// </summary>
[Serializable]
public class SpellData : IUseable
{
    // Nom du sort
    [SerializeField]
    private string name = default;

    // Dégâts du sort
    [SerializeField]
    private int damage = default;

    // Icône du sort
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

    // Propriété d'accès à l'icône du sort 
    public Sprite MyIcon { get => icon; }

    // Propriété d'accès à la vitesse du sort 
    public float MySpeed { get => speed; }

    // Propriété d'accès au temps d'incantation du sort
    public float MyCastTime { get => castTime; }

    // Propriété d'accès à la prefab du sort 
    public GameObject MyPrefab { get => prefab; }

    // Propriété d'accès à la couleur de la barre 
    public Color MyBarColor { get => barColor; }


    // Utilisation du sort
    public void Use()
    {
        Player.MyInstance.CastSpell(MyName);
    }
}