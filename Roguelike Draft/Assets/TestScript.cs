using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack {
	void Attack();
}

public class TestScript : MonoBehaviour {

	public float attackInterval = 1.0f;
	public float attackTimer = 0f;
	private Body bodyScript;
	private IAttack attackScript;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();
		attackScript = GetComponent<IAttack> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (attackTimer > attackInterval) {
			attackTimer = 0;
			attackScript.Attack ();
		}
		attackTimer = attackTimer + Time.deltaTime;
	}
}
