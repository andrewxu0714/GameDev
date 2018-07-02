using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	public enum statelist {neutral, moving, advancedmovement, advancedmovementinvuln, attacking, casting, castingunsafe, hit, knocked, grounded, recovering};
	public statelist state;

	public Vector3 translate;
	public float health=100f;
	public float direction=0;
	private bool specialInvuln = false;
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
		transform.position = new Vector3 (transform.position.x, 0.5f, transform.position.z);
	}

	public bool CanAct() {
		return (state == statelist.neutral || state == statelist.moving || state == statelist.casting);
	}

	public bool IsVulnerable() {
		return !(state == statelist.advancedmovementinvuln || state == statelist.grounded || state == statelist.recovering || specialInvuln);
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
		Vector3 knockbackVector = (Quaternion.AngleAxis(knockbackDirection*Mathf.Rad2Deg,Vector3.down) * Vector3.right);
		while (knockbackDelay < knockbackLimit && damageInstance==lastCollider) {
			direction = (knockbackDirection + Mathf.PI) % (Mathf.PI*2) ;
			float knockbackMagnitude = knockbackIntensity*(Mathf.Exp (-knockbackDelay*knockbackDecay) - Mathf.Exp (-(knockbackDelay+Time.deltaTime)*knockbackDecay));
			knockbackVector = knockbackVector.normalized * knockbackMagnitude;
			knockbackDelay = knockbackDelay + Time.deltaTime;
			translate = knockbackVector;
			yield return null;
		}
		if (damageInstance == lastCollider) {
			if (knockbackGrounded) {
				StartCoroutine (Ground ());
			} else {
				state = statelist.neutral;
			}
		}
	}

	IEnumerator Ground() {
		state = statelist.grounded;
		yield return new WaitForSeconds (1f);
		state = statelist.recovering;
		yield return new WaitForSeconds (0.2f);
		state = statelist.neutral;
		specialInvuln = true;
		yield return new WaitForSeconds (0.5f);
		specialInvuln = false;
	}
}
