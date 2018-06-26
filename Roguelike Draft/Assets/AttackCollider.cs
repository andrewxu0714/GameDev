using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

	public float attackDamage;
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
		if (target == attackSource) {
			return;
		}
		if (attackHistory.Contains (target)) {
			return;
		}
		attackHistory.Add (target);
		if (attackAllDirection) {
			target.GetComponent<Body> ().TakeDamage (this.gameObject,10f, 100f, Mathf.Atan2 (target.transform.position.z-transform.position.z, target.transform.position.x-transform.position.x), 0.5f, 0.5f);
		} else {
			target.GetComponent<Body> ().TakeDamage (this.gameObject,10f, 100f, Mathf.Atan2 ((transform.rotation * Vector3.right).z, (transform.rotation * Vector3.right).x), 0.5f, 0.5f);
		}
	}
}
