using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	public enum statelist {neutral, moving, advancedmovement, advancedmovementinvuln, attacking, casting, castingunsafe, hit, grounded, recovering, recovered};
	public statelist state;

	public Vector3 translate;
	public float health=100f;
	public float direction=0;
	private Rigidbody bodyRigidbody;

	private GameObject lastCollider;

	// Use this for initialization
	void Start () {
		bodyRigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		translate = new Vector3 (0, 0, 0);
	}

	void FixedUpdate() {
		bodyRigidbody.MovePosition (transform.position + translate);
	}

	public bool CanAct() {
		if (state == statelist.neutral || state == statelist.moving || state == statelist.casting || state == statelist.recovered) {
			return true;
		} else {
			return false;
		}
	}

	public void TakeDamage(GameObject damageInstance,float damage,float knockbackIntensity, float knockbackDirection, float knockbackDecay, float knockbackLimit) {
		health = health - damage;
		lastCollider = damageInstance;
		StartCoroutine (Knockback(damageInstance, knockbackIntensity, knockbackDirection, knockbackDecay, knockbackLimit));
	}

	IEnumerator Knockback(GameObject damageInstance, float knockbackIntensity, float knockbackDirection, float knockbackDecay, float knockbackLimit) {
		float knockbackDelay = 0;
		Vector3 knockbackVector = knockbackIntensity * (Quaternion.AngleAxis(knockbackDirection*Mathf.Rad2Deg,Vector3.down) * Vector3.right);
		while (knockbackDelay < knockbackLimit && damageInstance==lastCollider) {
			translate = knockbackVector * Time.deltaTime;
			direction = (knockbackDirection + Mathf.PI) % (Mathf.PI*2) ;
			knockbackVector = knockbackVector * knockbackDecay;
			knockbackDelay = knockbackDelay + Time.deltaTime;
			yield return null;
		}
	}


}
