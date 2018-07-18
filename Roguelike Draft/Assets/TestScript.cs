using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack {
	void Attack();
}

public class TestScript : MonoBehaviour {

	public GameObject targetPlayer;
	private Body bodyScript;
	private IAttack attackScript;
	private Navigation navScript;
	private Body.statelist prevState;
	private float targetDistance;
	public enum AIlist {roam1,roam2,closedistance,createdistance,attack};
	public AIlist AIstate;
	public float stateDuration;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();
		attackScript = GetComponent<IAttack> ();
		navScript = GetComponent<Navigation> ();
		FindClosestTarget ();
		prevState = bodyScript.state;
	}
	
	// Update is called once per frame
	void Update () {
		targetDistance = (targetPlayer.transform.position - transform.position).magnitude;

		if (prevState == Body.statelist.recovering && bodyScript.state != Body.statelist.recovering) {
			AIstate = EvaluateAI ();
		}
		prevState = bodyScript.state;

		switch (AIstate) {
		case AIlist.attack:
			AttackBehaviour ();
			break;
		case AIlist.roam1:
			Roam1 ();
			break;
		case AIlist.roam2:
			Roam2 ();
			break;
		case AIlist.closedistance:
			CloseDistance ();
			break;
		case AIlist.createdistance:
			CreateDistance ();
			break;
		}
	}

	void FindClosestTarget () {
		GameObject[] targetList = GameObject.FindGameObjectsWithTag ("Player");
		float targetDist = 999f;
		foreach (GameObject target in targetList) {
			if ((transform.position - target.transform.position).magnitude < targetDist) {
				targetDist = (transform.position - target.transform.position).magnitude;
				targetPlayer = target;
			}
		}
	}

	void CreateDistance () {
		stateDuration = stateDuration + Time.deltaTime;
		if (targetDistance < 6f & stateDuration < 1f) {
			navScript.goal = (transform.position - targetPlayer.transform.position).normalized * 6f + targetPlayer.transform.position;
		} else {
			AIstate = EvaluateAI ();
		}
	}

	void CloseDistance () {
		navScript.goal = targetPlayer.transform.position;
		stateDuration = stateDuration + Time.deltaTime;
		if (targetDistance < 3f) {
			if (targetPlayer.GetComponent<Body> ().IsVulnerable ()) {
				stateDuration = 0f;
				AIstate = AIlist.attack;
			} else {
				stateDuration = 0f;
				AIstate = AIlist.createdistance;
			}
		} else if (targetDistance < 6f || stateDuration > 2f) {
			AIstate = EvaluateAI ();
		}
	}

	void Roam1 () {
		stateDuration = stateDuration + Time.deltaTime;
		Vector3 tempOffset = transform.position - targetPlayer.transform.position;
		Vector3 tempPosition = Quaternion.AngleAxis (30, Vector3.down)*tempOffset;
		navScript.goal = targetPlayer.transform.position + tempPosition;
		if (targetDistance < 3f) {
			if (targetPlayer.GetComponent<Body> ().IsVulnerable ()) {
				stateDuration = 0f;
				AIstate = AIlist.attack;
			} else {
				stateDuration = 0f;
				AIstate = AIlist.createdistance;
			}
		}
		if (targetDistance > 10f) {
			AIstate = EvaluateAI ();
		}
		if (stateDuration > 3f) {
			float temp = Random.value;
			if (temp > 0.6f) {
				stateDuration = 0f;
				AIstate = AIlist.roam1;
			} else if (temp > 0.2f) {
				stateDuration = 0f;
				AIstate = AIlist.roam2;
			} else {
				stateDuration = 0f;
				AIstate = AIlist.closedistance;
			}
		}
	}

	void Roam2 () {
		stateDuration = stateDuration + Time.deltaTime;
		Vector3 tempOffset = transform.position - targetPlayer.transform.position;
		Vector3 tempPosition = Quaternion.AngleAxis (-30, Vector3.down)*tempOffset;
		navScript.goal = targetPlayer.transform.position + tempPosition;
		if (targetDistance < 3f) {
			if (targetPlayer.GetComponent<Body> ().IsVulnerable ()) {
				stateDuration = 0f;
				AIstate = AIlist.attack;
			} else {
				stateDuration = 0f;
				AIstate = AIlist.createdistance;
			}
		}
		if (targetDistance > 10f) {
			AIstate = EvaluateAI ();
		}
		if (stateDuration > 3f) {
			float temp = Random.value;
			if (temp > 0.6f) {
				stateDuration = 0f;
				AIstate = AIlist.roam1;
			} else if (temp > 0.2f) {
				stateDuration = 0f;
				AIstate = AIlist.roam2;
			} else {
				stateDuration = 0f;
				AIstate = AIlist.closedistance;
			}
		}
	}

	void AttackBehaviour () {
		stateDuration = stateDuration + Time.deltaTime;
		if (bodyScript.CanAct ()) {
			Vector3 temp = targetPlayer.transform.position - transform.position;
			bodyScript.direction = Mathf.Atan2 (temp.z, temp.x);
			attackScript.Attack ();
		} else
			AIstate = EvaluateAI ();
	}

	AIlist EvaluateAI() {
		stateDuration = 0;
		FindClosestTarget ();
		targetDistance = (targetPlayer.transform.position - transform.position).magnitude;
		if (targetDistance > 10f) {
			return AIlist.closedistance;
		} else if (targetDistance > 5f) {
			if (Random.value > 0.5f) {
				return AIlist.roam1;
			} else {
				return AIlist.roam2;
			}
		} else
			return AIlist.closedistance;
	}
}
