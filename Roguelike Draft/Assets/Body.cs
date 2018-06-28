using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	public enum statelist {neutral, moving, advancedmovement, advancedmovementinvuln, attacking, casting, castingunsafe, hit, knocked, grounded, recovering, recovered};
	public statelist state;

	public Vector3 translate;
	public float health=100f;
	public float direction=0;
	private CharacterController bodyController;

	private GameObject lastCollider;

	// Use this for initialization
	void Start () {
		bodyController = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		bodyController.Move (translate);
		translate = new Vector3 (0, 0, 0);
	}

	public bool CanAct() {
		return (state == statelist.neutral || state == statelist.moving || state == statelist.casting || state == statelist.recovered);
	}

	public bool IsVulnerable() {
		return !(state == statelist.advancedmovementinvuln || state == statelist.grounded || state == statelist.recovering || state==statelist.recovered);
	}

	public void TakeDamage(GameObject damageInstance,float damage,float knockbackIntensity, float knockbackDirection, float knockbackDecay, float knockbackLimit, bool knockbackGrounded) {
		health = health - damage;
		lastCollider = damageInstance;
		StartCoroutine (Knockback(damageInstance, knockbackIntensity, knockbackDirection, knockbackDecay, knockbackLimit, knockbackGrounded));
	}

	IEnumerator Knockback(GameObject damageInstance, float knockbackIntensity, float knockbackDirection, float knockbackDecay, float knockbackLimit, bool knockbackGrounded) {
		float knockbackDelay = 0;
		if (knockbackGrounded) {
			state = statelist.knocked;
		} else {
			state = statelist.hit;
		}
		Vector3 knockbackVector = knockbackIntensity * (Quaternion.AngleAxis(knockbackDirection*Mathf.Rad2Deg,Vector3.down) * Vector3.right);
		while (knockbackDelay < knockbackLimit && damageInstance==lastCollider) {
			translate = knockbackVector * Time.deltaTime;
			direction = (knockbackDirection + Mathf.PI) % (Mathf.PI*2) ;
			knockbackVector = knockbackVector * knockbackDecay;
			knockbackDelay = knockbackDelay + Time.deltaTime;
			yield return null;
		}
		if (knockbackGrounded) {
			StartCoroutine(Ground());
		}
	}

	IEnumerator Ground() {
		state = statelist.grounded;
		yield return new WaitForSeconds (1f);
		state = statelist.recovering;
		yield return new WaitForSeconds (0.1f);
		state = statelist.recovered;
		yield return new WaitForSeconds (0.2f);
		if (state == statelist.recovered) {
			state = statelist.neutral;
		}
	}
}
