using UnityEngine;

public class RandomIdle : StateMachineBehaviour {

	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		animator.SetInteger("idle", Random.Range(0, 4)); // Create a random index for the idle animation
	}
}
