using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine {
	public class StateMachineCallback : MonoBehaviour, IStateMachineCallback {

		[Serializable]
		public struct Group {
			public List<Card> cards;
		}

		[Serializable]
		public struct Card {
			public string filter;
			public int layerIndex;
			
			public UnityEvent onAnimationStart;
			public UnityEvent onAnimationUpdate;
			public UnityEvent onAnimationEnd;
		}

		public Group group;

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