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
    private Stat health = default;

    // Mana du joueur
    [SerializeField]
    private Stat mana = default;
    
    // Préfab des sorts
    [SerializeField]
    private GameObject[] spellPrefab = default;

    /// Positions pour lancer les sorts
    [SerializeField]
    private Transform[] exitPoints = default;

    /// Index de la position d'attaque (2 = down)
    private int exitIndex = 2;

    // Vie initiale du joueur (readonly)
    private readonly float initHealth = 100;

    // Mana initiale du joueur (readonly)
    private readonly float initMana = 50;


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

        // [DEBUG] : Test Vie/Mana
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
            exitIndex = 0;
            direction += Vector2.up;
        }

        // Déplacement en Bas
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }

        // Déplacement à Gauche
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }

        // Déplacement à Droite
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }

        // Attaque du joueur
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Vérifie si l'on peut attaquer
            if (!isAttacking && !IsMoving)
            {
                attackRoutine = StartCoroutine(Attack());
            }
        }
    }

    /// <summary>
    /// Routine d'attaque
    /// </summary>
    private IEnumerator Attack()
    {
        // Indique que l'on attaque
        isAttacking = true;

        // Lance l'animation d'attaque
        animator.SetBool("attack", isAttacking);

        // [DEBUG] : Durée de cast
        yield return new WaitForSeconds(1);

        // Lance le sort
        CastSpell();

        Debug.Log("Cast terminé");

        // Termine l'attaque
        StopAttack();
    }

    /// <summary>
    /// Incante un sort
    /// </summary>
    public void CastSpell()
    {
        // Instantie le sort
        Instantiate(spellPrefab[0], exitPoints[exitIndex].position, Quaternion.identity);
    }
}