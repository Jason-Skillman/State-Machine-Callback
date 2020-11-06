using UnityEngine;

namespace StateMachine.Callback {
	public class StateMachineEvent : StateMachineBehaviour {

		private IStateMachineCallback stateMachineCallback;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if(stateMachineCallback == null)
				stateMachineCallback = animator.GetComponent<IStateMachineCallback>();

			stateMachineCallback?.OnAnimationStart(stateInfo, layerIndex);
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			stateMachineCallback?.OnAnimationUpdate(stateInfo, layerIndex);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			stateMachineCallback?.OnAnimationEnd(stateInfo, layerIndex);
		}

	}
}