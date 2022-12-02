namespace JasonSkillman.StateMachine {
	using UnityEngine;
	
	public interface IStateMachineCallback {
		/// <summary>
		/// Called once when the animation state starts.
		/// </summary>
		/// <param name="stateInfo"></param>
		/// <param name="layerIndex"></param>
		void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex);

		/// <summary>
		/// Called every frame while animating.
		/// </summary>
		/// <param name="stateInfo"></param>
		/// <param name="layerIndex"></param>
		void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex);

		/// <summary>
		/// Called once when the animation state ends.
		/// </summary>
		/// <param name="stateInfo"></param>
		/// <param name="layerIndex"></param>
		void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex);
	}
}