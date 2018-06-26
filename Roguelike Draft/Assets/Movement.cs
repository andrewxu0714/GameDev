using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	private Body bodyScript;

	public float speed=10;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();		
	}

	// Update is called once per frame
	void Update () {
		if (bodyScript.CanAct()) {
			Move ();
		}
	}

	void Move() {
		if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
			bodyScript.state = Body.statelist.moving;
			Vector3 v_move = Input.GetAxis ("Vertical") * new Vector3 (-1, 0, 1);
			Vector3 h_move = Input.GetAxis ("Horizontal") * new Vector3 (1, 0, 1);
			bodyScript.translate = (v_move + h_move).normalized * speed * Time.deltaTime;
			bodyScript.direction = Mathf.Atan2 (bodyScript.translate.z, bodyScript.translate.x);
		} else {
			bodyScript.state = Body.statelist.neutral;
		}
	}


}
