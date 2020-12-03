using System;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine.Callback {
	public class StateMachineUnityEvents : MonoBehaviour, IStateMachineCallback {

		[SerializeField]
		private string filter;
		[SerializeField]
		private int layerIndex;

		[SerializeField, Space]
		private UnityEvent onAnimationStart;
		[SerializeField]
		private UnityEvent onAnimationUpdate;
		[SerializeField]
		private UnityEvent onAnimationEnd;
		
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