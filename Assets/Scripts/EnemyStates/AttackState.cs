using System;
using System.Collections;
using UnityEngine;



/// <summary>
/// Classe de l'état "ATTACK"
/// </summary>
class AttackState : IState
{
    // Référence sur le script Enemy
    private Enemy parent;

    // Cooldown d'attaque
    private readonly float attackCooldown = 3;

    // Portée d'attaque
    private readonly float extraRange = 0.1f;



    /// <summary>
    /// Entrée dans l'état "IDLE"
    /// </summary>
    /// <param name="enemyScript">le parent de l'ennemi</param>
    public void Enter(Enemy enemyScript)
    {
        parent = enemyScript;

        // Réinitialise la direction/mouvement
        parent.MyDirection = Vector2.zero;
        parent.MyRigidbodyCharacter.velocity = Vector2.zero;
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
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            // Réinitialise le temps d'attaque
            parent.MyAttackTime = 0;

            // Démarre la routine d'attaque
            parent.StartCoroutine(Attack());
        }


        // S'il y a une cible
        if (parent.MyTarget != null)
        {
            // Distance entre l'ennemi et la cible
            float distance = Vector2.Distance(parent.MyTarget.transform.parent.position, parent.transform.parent.position);

            // Si la cible est à portée d'attaque
            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking)
            {
                // Si c'est un ennemi distant
                if (parent is EnemyRanged)
                {
                    // Passage à l'état de recherche
                    parent.ChangeState(new PathState());
                }
                else
                {
                    // Passage à l'état de poursuite
                    parent.ChangeState(new FollowState());
                }
            }
        }
        else
        {
            // Passage à l'état d'attente
            parent.ChangeState(new IdleState());
        }
    }



    /// <summary>
    /// Routine d'attaque
    /// </summary>
    private IEnumerator Attack()
    {
        // Indique que l'on attaque
        parent.IsAttacking = true;

        // Lance l'animation d'attaque
        parent.MyAnimator.SetTrigger("attack");

        // Attente de la fin de l'animation
        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        // Termine l'attaque
        parent.IsAttacking = false;
    }
}