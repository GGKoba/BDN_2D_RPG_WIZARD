using System;
using UnityEngine;



/// <summary>
/// Classe des sorts
/// </summary>
[Serializable]
public class Spell
{
    // Nom du sort
    [SerializeField]
    private string name = default;

    // Dégâts du sort
    [SerializeField]
    private int damage = default;

    // Icone du sort
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

    // Nom 
    public string SpellName { get => name; }

    // Dégâts
    public int SpellDamage { get => damage; }

    // Icone 
    public Sprite SpellIcon { get => icon; }

    // Vitesse 
    public float SpellSpeed { get => speed; }

    // Temps d'incantation 
    public float SpellCastTime { get => castTime; }

    // Prefab 
    public GameObject SpellPrefab { get => prefab; }

    // Couleur de la barre 
    public Color SpellBarColor { get => barColor; }
}