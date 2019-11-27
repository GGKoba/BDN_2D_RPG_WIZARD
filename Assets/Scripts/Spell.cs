using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



/// <summary>
/// Classe des sorts
/// </summary>
[Serializable]
public class Spell
{
    // Nom du sort
    [SerializeField]
    private string name;

    // Dégats du sort
    [SerializeField]
    private int damage;

    // Icone du sort
    [SerializeField]
    private Sprite icon;

    // Vitesse du sort
    [SerializeField]
    private float speed;

    // Temps d'incantation du sort
    [SerializeField]
    private float castTime;

    // Prefab du sort
    [SerializeField]
    private GameObject prefab;

    // Couleur de la barre de sort
    [SerializeField]
    private Color barColor;


    /// <summary>
    /// Accesseurs sur les propriétés du sort
    /// </summary>

    // Nom 
    public string SpellName { get => name; }

    // Dégats 
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