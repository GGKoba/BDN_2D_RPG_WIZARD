using UnityEngine;



public class DeathBehaviour : StateMachineBehaviour
{
    // Temps écoulé depuis le début de l'animation
    private float timePassed;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Destruction du 1er enfant de l'objet (hitbox du personnage)
        Destroy(animator.transform.GetChild(0).gameObject);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MAJ du temps écoulé 
        timePassed += Time.deltaTime;

        // Détruit l'objet après 4s
        if (timePassed >= 4)
        {
            // Déclenche l'évènement de disparition du personnage
            animator.GetComponent<NPC>().OnCharacterRemoved();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
