using System;
using System.Collections;
using UnityEngine;



/// <summary>
/// Classe de l'état "ATTACK"
/// </summary>
class AttackState : IState
{
    // Référence sur le script Enemy
    private Enemy enemy;

    // Cooldown d'attaque
    private float attackCooldown = 3;

    // Portée d'attaque
    private float extraRange = 0.1f;



    /// <summary>
    /// Entrée dans l'état "IDLE"
    /// </summary>
    public void Enter(Enemy enemyScript)
    {
        enemy = enemyScript;
    }

    /// <summary>
    /// Mise à jour dans l'état "IDLE"
    /// </summary>
    public void Exit()
    {
    }

    /// <summary>
    /// Sortie de l'état "IDLE"
    /// </summary>
    public void Update()
    {
        // Vérifie si l'on peut attaquer
        if (enemy.MyAttackTime >= attackCooldown && !enemy.IsAttacking)
        {
            // Reset du temps d'attaque
            enemy.MyAttackTime = 0;

            // Lancement de l'attaque
            enemy.StartCoroutine(Attack());
        }


        // S'il y a une cible
        if (enemy.MyTarget != null)
        {
            // Distance entre l'ennemi et la cible
            float distance = Vector2.Distance(enemy.MyTarget.position, enemy.transform.position);

            // Si la cible est à portée d'attaque
            if (distance >= enemy.MyAttackRange + extraRange && !enemy.IsAttacking)
            {
                // Passage à l'état de poursuite
                enemy.ChangeState(new FollowState());
            }
        }
        else
        {
            // Passage à l'état d'attente
            enemy.ChangeState(new IdleState());
        }
    }



    /// <summary>
    /// Routine d'attaque
    /// </summary>
    private IEnumerator Attack()
    {
        // La cible de l'attaque est la cible sélectionnée
        Transform attackTarget = enemy.MyTarget;

        // Récupére un sort avec ses propriétes depuis la bibliothèque des sorts 
        //Spell mySpell = spellBook.CastSpell(spellIndex);

        // Indique que l'on attaque
        enemy.IsAttacking = true;

        // Lance l'animation d'attaque
        enemy.MyAnimator.SetTrigger("attack");

        // Attente de la fin de l'animation
        yield return new WaitForSeconds(enemy.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        // Termine l'attaque
        enemy.IsAttacking = false;
    }
}