using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe Player contenant les fonctionnalités spécifiques au joueur
/// </summary>
public class Player : Character
{
    // Vie du joueur
    [SerializeField]
    private Stat health;

    // Mana du joueur
    [SerializeField]
    private Stat mana;
    
    // Vie initiale du joueur
    private float initHealth = 100;

    // Mana initiale du joueur
    private float initMana = 50;


    /// <summary>
    /// Start
    /// </summary>
    protected override void Start()
    {
        // Initialise les barres
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    /// <summary>
    /// Update : Ecrase la fonction Update du script Character
    /// </summary>
    protected override void Update()
    {
        GetInput();

        // Appel Update sur la classe abstraite
        base.Update();
    }

    /// <summary>
    /// Déplacement du joueur
    /// </summary>
    private void GetInput()
    {
        direction = Vector2.zero;

        // TEST
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }


        // Déplacement en Haut
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector2.up;
        }

        // Déplacement en Bas
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector2.down;
        }

        // Déplacement à Gauche
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector2.left;
        }

        // Déplacement à Droite
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector2.right;
        }
    }
}
