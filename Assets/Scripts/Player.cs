using System.Collections;
using UnityEngine;



/// <summary>
/// Classe contenant les fonctionnalités spécifiques au joueur
/// </summary>
public class Player : Character
{
    // Mana du joueur
    [SerializeField]
    private Stat mana = default;

    // Tableau des positions pour lancer les sorts
    [SerializeField]
    private Transform[] exitPoints = default;

    // Tableau des paires de blocs pour bloquer la ligne de mire
    [SerializeField]
    private Block[] blocks = default;

    // Mana initiale du joueur (readonly)
    private readonly float initMana = 50;
    
    // Index de la position d'attaque (2 = down)
    private int exitIndex = 2;

    // Bibliothèque des sorts
    private SpellBook spellBook;

    // Cible du joueur
    public Transform MyTarget { get; set; }


    /// <summary>
    /// Start
    /// </summary>
    protected override void Start()
    {
        // Initialise les barres
        mana.Initialize(initMana, initMana);

        // Référence sur la bibliothèque des sorts
        spellBook = gameObject.GetComponent<SpellBook>();

        // Appel Start sur la classe mère (abstraite)
        base.Start();
    }

    /// <summary>
    /// Update : Ecrase la fonction Update du script Character
    /// </summary>
    protected override void Update()
    {
        // Vérifie les interactions
        GetInput();

        // Appel Update sur la classe mère (abstraite)
        base.Update();
    }

    /// <summary>
    /// Interactions du joueur
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
    }

    /// <summary>
    /// Routine d'attaque
    /// </summary>
    /// <param name="spellIndex">Index du sort</param>
    private IEnumerator Attack(int spellIndex)
    {
        // La cible de l'attaque est la cible sélectionnée
        Transform attackTarget = MyTarget;

        // Récupére un sort avec ses propriétes depuis la bibliothèque des sorts 
        Spell mySpell = spellBook.CastSpell(spellIndex);

        // Indique que l'on attaque
        isAttacking = true;

        // Lance l'animation d'attaque
        animator.SetBool("attack", isAttacking);

        // Simule le temps d'incantation
        yield return new WaitForSeconds(mySpell.SpellCastTime);

        // Vérifie que la cible de l'attaque est toujours attaquable 
        if (attackTarget != null && InLineOfSight())
        {
            // Instantie le sort
            SpellManager spell = Instantiate(mySpell.SpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellManager>();

            // Affecte la cible et les dégâts du sort
            spell.Initialize(attackTarget, mySpell.SpellDamage);
        }

        // Termine l'attaque
        StopAttack();
    }

    /// <summary>
    /// Incante un sort
    /// </summary>
    /// <param name="spellIndex">Index du sort</param>
    public void CastSpell(int spellIndex)
    {
        // Actualise les blocs
        Block();

        // Vérifie si l'on peut attaquer
        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        }
    }

    /// <summary>
    /// Verifie si la cible est dans la ligne de mire
    /// </summary>
    private bool InLineOfSight()
    {
        // Si le joueur a une cible
        if (MyTarget != null)
        {
            // Calcule la direction de la cible
            Vector2 targetDirection = (MyTarget.position - transform.position).normalized;

            // Lance un raycast dans la direction de la cible  [Debug.DrawRay(transform.position, targetDirection, Color.red);]
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), LayerMask.GetMask("Block"));

            // S'il n'y a pas de collision, on peut lancer le sort
            if (hit.collider == null)
            {
                return true;
            }
        }

        // En cas de collision, on ne peut pas lancer le sort
        return false;
    }

    /// <summary>
    /// Change la paire de blocs en fonction de la direction du joueur
    /// </summary>
    private void Block()
    {
        foreach (Block bloc in blocks)
	    {
            // Désactive toutes les paires blocs
            bloc.Deactivate();
	    }

        // Active la paire de bloc correspondante à la direction du joueur
        blocks[exitIndex].Activate();
    }

    /// <summary>
    /// Fin de l'attaque : Ecrase la fonction StopAttack du script Character
    /// </summary>
    public override void StopAttack()
    {
        // Stoppe l'incantation du sort
        spellBook.StopCasting();

        // Appel StopAttack sur la classe abstraite pour mettre fin au statut d'attaque
        base.StopAttack();
    }
}