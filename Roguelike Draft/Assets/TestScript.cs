using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	public float attackInterval = 1.0f;
	public float attackTimer = 0f;
	public GameObject attackCol;
	private Body bodyScript;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (attackTimer > attackInterval) {
			attackTimer = 0;
			GameObject temp = Instantiate (attackCol, this.transform);
			foreach (var i in temp.GetComponentsInChildren<AttackCollider> ()) {
				i.attackDamage = 10f;
				i.knockbackIntensity = 6f;
				i.knockbackDecay = 6f;
				i.knockbackLimit = 0.5f;
				i.knockbackGrounded = false;
				i.attackActiveDuration = 0.2f;
				i.attackStunDuration = 0.25f;
				i.attackAllDirection = false;
				i.attackSource = this.gameObject;
			}
			temp.transform.rotation = Quaternion.AngleAxis (Mathf.Rad2Deg * bodyScript.direction, Vector3.down);			
		}
		attackTimer = attackTimer + Time.deltaTime;
	}
}
