using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	private Body bodyScript;

	private int combo=5;
	private float[] comboDamage = new float[5] { 10f, 5f, 5f, 5f, 25f };
	private float[] comboMovement = new float[5] { 0.9f, 0.9f, 0.9f, 0.9f, 0.9f };
	private float[] comboKnockbackIntensity = new float[5] { 1f, 1f, 1f, 1f, 6f };
	private float[] comboKnockbackDecay = new float[5] { 12f, 12f, 12f, 12f, 6f };
	private float[] comboKnockbackLimit = new float[5] { 0.6f, 0.6f, 0.6f, 0.6f, 1f };
	private bool[] comboKnockbackGrounded = new bool[5] { false, false, false, false, true };
	private float[] comboDuration = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 1f };
	private float[] comboActiveStart = new float[5] { 0.1f, 0.1f, 0.1f, 0.1f, 0.1f };
	private float[] comboActiveDuration = new float[5] { 0.3f, 0.3f, 0.3f, 0.3f, 0.3f };
	private float[] comboStunDuration = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
	private bool[] comboAllDirection = new bool[5] { false, false, false, false, false };
	public GameObject[] comboCol = new GameObject[5];

	private int attackBuffer=0;
	private int comboCount=0; //represents the stats of the NEXT attack

	private float prevFire = 0f;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis ("Fire1") > 0 && prevFire == 0 && (bodyScript.CanAct () || bodyScript.state == Body.statelist.attacking)) {
			attackBuffer = Mathf.Min (combo - comboCount, attackBuffer + 1);
		}

		if (attackBuffer > 0 && bodyScript.CanAct()) {
			StartCoroutine (Strike (comboDamage [comboCount], comboMovement [comboCount], comboKnockbackIntensity [comboCount], comboKnockbackDecay [comboCount], comboKnockbackLimit [comboCount], comboKnockbackGrounded [comboCount], comboDuration [comboCount], comboActiveStart [comboCount], comboActiveDuration [comboCount], comboStunDuration [comboCount], comboAllDirection [comboCount], comboCol [comboCount]));
		}

		prevFire = Input.GetAxis ("Fire1");
	}
		
	IEnumerator Strike(float attackDamage, float attackMovement, float knockbackIntensity, float knockbackDecay, float knockbackLimit, bool knockbackGrounded, float attackDuration, float attackActiveStart, float attackActiveDuration, float attackStunDuration, bool attackAllDirection, GameObject attackCol) {
		bodyScript.state = Body.statelist.attacking;
		attackBuffer = attackBuffer - 1;
		comboCount = comboCount + 1;
		if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
			Vector3 v_move = Input.GetAxis ("Vertical") * new Vector3 (-1, 0, 1);
			Vector3 h_move = Input.GetAxis ("Horizontal") * new Vector3 (1, 0, 1);
			bodyScript.direction = Mathf.Atan2 ((v_move + h_move).z, (v_move + h_move).x);
		}
		float attackDelay = 0;
		bool active = false;
		while (attackDelay < attackDuration) {
			if (bodyScript.state == Body.statelist.attacking) {
				if (attackBuffer > 0 && comboCount < combo && attackDelay > attackActiveStart + attackActiveDuration) {
					StartCoroutine (Strike (comboDamage [comboCount], comboMovement [comboCount], comboKnockbackIntensity [comboCount], comboKnockbackDecay [comboCount], comboKnockbackLimit [comboCount], comboKnockbackGrounded [comboCount], comboDuration [comboCount], comboActiveStart [comboCount], comboActiveDuration [comboCount], comboStunDuration [comboCount], comboAllDirection [comboCount], comboCol [comboCount]));
					yield break;
				} else {
					if (Input.GetAxis ("Fire1") > 0 && comboCount < combo && attackDelay > attackActiveStart + attackActiveDuration) {
						attackBuffer = attackBuffer + 1;
						StartCoroutine (Strike (comboDamage [comboCount], comboMovement [comboCount], comboKnockbackIntensity [comboCount], comboKnockbackDecay [comboCount], comboKnockbackLimit [comboCount], comboKnockbackGrounded [comboCount], comboDuration [comboCount], comboActiveStart [comboCount], comboActiveDuration [comboCount], comboStunDuration [comboCount], comboAllDirection [comboCount], comboCol [comboCount]));
						yield break;
					} else {
						attackDelay = attackDelay + Time.deltaTime;
						if (attackDelay > attackActiveStart && active == false) {
							active = true;
							GameObject temp = Instantiate (attackCol, this.transform);
							foreach (var i in temp.GetComponentsInChildren<AttackCollider> ()) {
								i.attackDamage = attackDamage;
								i.knockbackIntensity = knockbackIntensity;
								i.knockbackDecay = knockbackDecay;
								i.knockbackLimit = knockbackLimit;
								i.knockbackGrounded = knockbackGrounded;
								i.attackActiveDuration = attackActiveDuration;
								i.attackStunDuration = attackStunDuration;
								i.attackAllDirection = attackAllDirection;
								i.attackSource = this.gameObject;
							}
							temp.transform.rotation = Quaternion.AngleAxis (Mathf.Rad2Deg * bodyScript.direction, Vector3.down);
						}
						if (attackDelay < attackActiveStart) {
							bodyScript.translate = attackMovement * Time.deltaTime / attackActiveStart * (Quaternion.AngleAxis (bodyScript.direction * Mathf.Rad2Deg, Vector3.down) * Vector3.right);
						}
						yield return null;
					}
				}
			} else {
				comboCount = 0;
				attackBuffer = 0;
				yield break;
			}
		}
		bodyScript.state = Body.statelist.neutral;
		comboCount = 0;
		attackBuffer = 0;
	}
}
