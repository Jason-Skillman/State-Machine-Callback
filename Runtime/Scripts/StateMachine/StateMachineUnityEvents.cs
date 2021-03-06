using System;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine {
	public class StateMachineUnityEvents : MonoBehaviour, IStateMachineCallback {

		[SerializeField]
		private string filter = default;
		[SerializeField]
		private int layerIndex = default;

		[SerializeField, Space]
		private UnityEvent onAnimationStart = default;
		[SerializeField]
		private UnityEvent onAnimationUpdate = default;
		[SerializeField]
		private UnityEvent onAnimationEnd = default;

		public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) {
			if(filter.Equals(string.Empty) || stateInfo.IsName(filter) && this.layerIndex == layerIndex) {
				onAnimationStart?.Invoke();
			}
		}

		public void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex) {
			if(filter.Equals(string.Empty) || stateInfo.IsName(filter) && this.layerIndex == layerIndex) {
				onAnimationUpdate?.Invoke();
			}
		}

		public void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex) {
			if(filter.Equals(string.Empty) || stateInfo.IsName(filter) && this.layerIndex == layerIndex) {
				onAnimationEnd?.Invoke();
			}
		}

	}
}