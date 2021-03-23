using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine {
	public class StateMachineCallback : MonoBehaviour, IStateMachineCallback {

		[Serializable]
		private struct Rule {
			public string filter;
			public int layerIndex;
			
			public UnityEvent onAnimationStart;
			public UnityEvent onAnimationUpdate;
			public UnityEvent onAnimationEnd;
		}

		[SerializeField]
		private List<Rule> rulesList;

		public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) {
			/*if(filter.Equals(string.Empty) || stateInfo.IsName(filter) && this.layerIndex == layerIndex) {
				onAnimationStart?.Invoke();
			}*/
		}

		public void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex) {
			/*if(filter.Equals(string.Empty) || stateInfo.IsName(filter) && this.layerIndex == layerIndex) {
				onAnimationUpdate?.Invoke();
			}*/
		}

		public void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex) {
			/*if(filter.Equals(string.Empty) || stateInfo.IsName(filter) && this.layerIndex == layerIndex) {
				onAnimationEnd?.Invoke();
			}*/
		}

	}
}