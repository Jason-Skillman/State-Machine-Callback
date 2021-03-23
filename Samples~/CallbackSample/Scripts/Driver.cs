using UnityEngine;

public class Driver : MonoBehaviour {

	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	private void Update() {
		//Debug: Turn the sphere on and off with the number keys
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			animator.SetBool("isOn", true);
		} else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			animator.SetBool("isOn", false);
		}
	}
	
}