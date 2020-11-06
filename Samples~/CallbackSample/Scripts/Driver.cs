using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine.Callback;
using UnityEngine;

public class Driver : MonoBehaviour, IStateMachineCallback {

	[SerializeField]
	private Material matOff, matOn;

	private Animator animator;
	private MeshRenderer meshRenderer;

	private void Awake() {
		animator = GetComponent<Animator>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Update() {
		//Debug: Turn the sphere on and off with the number keys
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			animator.SetBool("isOn", true);
		} else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			animator.SetBool("isOn", false);
		}
	}

	public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) {
		//Use stateInfo to get the correct animation state name
		if(stateInfo.IsName("TurnOn")) {
			Debug.Log("Turn on start callback");
		} else if(stateInfo.IsName("TurnOff")) {
			Debug.Log("Turn off start callback");
		}
	}

	public void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex) { }

	public void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex) {
		//Use stateInfo to get the correct animation state name
		if(stateInfo.IsName("TurnOn")) {
			Debug.Log("Turn on end callback");

			//Use the callback to turn the sphere green when the animation has stopped playing
			meshRenderer.material = matOn;
		} else if(stateInfo.IsName("TurnOff")) {
			Debug.Log("Turn off end callback");

			//Use the callback to turn the sphere white when the animation has stopped playing
			meshRenderer.material = matOff;
		}
	}
}