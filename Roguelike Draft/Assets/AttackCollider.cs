using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

	public float attackDamage;
	public float knockbackIntensity;
	public float knockbackDecay;
	public float knockbackLimit;
	public bool knockbackGrounded;
	public float attackActiveDuration;
	public float attackStunDuration;
	public bool attackAllDirection;
	public GameObject attackSource;
	private List<GameObject> attackHistory = new List<GameObject>();

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
		if (attackSource.GetComponent<Body>().state != Body.statelist.attacking || attackActiveDuration<0) {
			Destroy (this.transform.parent.gameObject);
			Destroy (this.gameObject);
		}
		attackActiveDuration = attackActiveDuration - Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		Damage (other.gameObject);
	}

	void Damage (GameObject target) {
		if (target == attackSource || attackHistory.Contains (target) || !target.GetComponent<Body> ().IsVulnerable()) {
			return;
		}
		attackHistory.Add (target);
		if (attackAllDirection) {
			target.GetComponent<Body> ().TakeDamage (this.gameObject,attackDamage, knockbackIntensity, Mathf.Atan2 (target.transform.position.z-transform.position.z, target.transform.position.x-transform.position.x), knockbackDecay, knockbackLimit, knockbackGrounded);
		} else {
			target.GetComponent<Body> ().TakeDamage (this.gameObject,attackDamage, knockbackIntensity, Mathf.Atan2 ((transform.rotation * Vector3.right).z, (transform.rotation * Vector3.right).x), knockbackDecay, knockbackLimit, knockbackGrounded);
		}
	}
}
