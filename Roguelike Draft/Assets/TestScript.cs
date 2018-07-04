using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	public float attackInterval = 1.0f;
	public float attackTimer = 0f;
	private Body bodyScript;
	private LeapAttack attackScript;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();
		attackScript = GetComponent<LeapAttack> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (attackTimer > attackInterval) {
			attackTimer = 0;
			attackScript.CommandAttack ();		
		}
		attackTimer = attackTimer + Time.deltaTime;
	}
}
