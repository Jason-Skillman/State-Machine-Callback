using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine {
	public class StateMachineCallbackEvents : MonoBehaviour, IStateMachineCallback {

		[Serializable]
		private struct Rule {
			public string animationStateName;
			public int layerIndex;
			
			public UnityEvent onAnimationStart;
			public UnityEvent onAnimationUpdate;
			public UnityEvent onAnimationEnd;
		}

		[SerializeField]
		private List<Rule> rulesList = default;

		public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) {
			foreach(Rule rule in rulesList) {
				if(rule.animationStateName.Equals(string.Empty) || stateInfo.IsName(rule.animationStateName) && rule.layerIndex == layerIndex) {
					rule.onAnimationStart?.Invoke();
				}
			}
		}

		public void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex) {
			foreach(Rule rule in rulesList) {
				if(rule.animationStateName.Equals(string.Empty) || stateInfo.IsName(rule.animationStateName) && rule.layerIndex == layerIndex) {
					rule.onAnimationUpdate?.Invoke();
				}
			}
		}

		public void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex) {
			foreach(Rule rule in rulesList) {
				if(rule.animationStateName.Equals(string.Empty) || stateInfo.IsName(rule.animationStateName) && rule.layerIndex == layerIndex) {
					rule.onAnimationEnd?.Invoke();
				}
			}
		}

	}
}