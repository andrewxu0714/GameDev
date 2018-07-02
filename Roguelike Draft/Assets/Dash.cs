using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

	private Body bodyScript;
	public string triggerAxis = "Fire2";
	private float triggerCooldown = 0f;
	private float abilityCooldown = 1f;
	private float abilityDuration = 0.1f;
	private float abilityDistance = 8f;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis (triggerAxis) > 0 && bodyScript.CanAct () && triggerCooldown == 0) {
			StartCoroutine (abilityExecute ());
		}
		triggerCooldown = Mathf.Max (0, triggerCooldown - Time.deltaTime);
	}

	IEnumerator abilityExecute() {
		bodyScript.state = Body.statelist.advancedmovement;
		triggerCooldown = abilityCooldown;
		if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
			Vector3 v_move = Input.GetAxis ("Vertical") * new Vector3 (-1, 0, 1);
			Vector3 h_move = Input.GetAxis ("Horizontal") * new Vector3 (1, 0, 1);
			bodyScript.direction = Mathf.Atan2 ((v_move + h_move).z, (v_move + h_move).x);
		}
		float triggerDistance = 0f;
		while (triggerDistance < abilityDistance) {
			if (bodyScript.state != Body.statelist.advancedmovement) {
				yield break;
			}
			float tempMagnitude = Time.deltaTime * abilityDistance / abilityDuration;
			tempMagnitude = Mathf.Min (abilityDistance - triggerDistance, tempMagnitude);
			bodyScript.translate= tempMagnitude*(Quaternion.AngleAxis (bodyScript.direction * Mathf.Rad2Deg, Vector3.down) * Vector3.right).normalized;
			triggerDistance = triggerDistance + tempMagnitude;
			yield return null;
		}
		bodyScript.state = Body.statelist.neutral;
	}
}
