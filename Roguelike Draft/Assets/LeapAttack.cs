using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapAttack : MonoBehaviour, IAttack {

	private Body bodyScript;

	private int combo=1;
	private float[] comboDamage = new float[1] { 30f };
	private float[] comboMovement = new float[1] { 3f };
	private float[] comboKnockbackIntensity = new float[1] { 6f };
	private float[] comboKnockbackDecay = new float[1] { 2f };
	private float[] comboKnockbackLimit = new float[1] { 1f };
	private bool[] comboKnockbackGrounded = new bool[1] { true };
	private float[] comboDuration = new float[1] { 0.1f };
	private float[] comboActiveStart = new float[1] { 0.01f };
	private float[] comboActiveDuration = new float[1] { 0.09f };
	private float[] comboStunDuration = new float[1] { 1f };
	private bool[] comboAllDirection = new bool[1] { false };
	private bool[] comboPositionTrack = new bool[1] { true };
	public GameObject[] comboCol = new GameObject[1];

	private int comboCount=0; //represents the stats of the NEXT attack

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();		
	}

	// Update is called once per frame
	void Update () {

	}

	public void Attack() {
		StartCoroutine (Strike (comboDamage [comboCount], comboMovement [comboCount], comboKnockbackIntensity [comboCount], comboKnockbackDecay [comboCount], comboKnockbackLimit [comboCount], comboKnockbackGrounded [comboCount], comboDuration [comboCount], comboActiveStart [comboCount], comboActiveDuration [comboCount], comboStunDuration [comboCount], comboAllDirection [comboCount], comboPositionTrack [comboCount], comboCol [comboCount]));
	}

	IEnumerator Strike (float attackDamage, float attackMovement, float knockbackIntensity, float knockbackDecay, float knockbackLimit, bool knockbackGrounded, float attackDuration, float attackActiveStart, float attackActiveDuration, float attackStunDuration, bool attackAllDirection, bool attackPositionTrack, GameObject attackCol)
	{
		bodyScript.state = Body.statelist.attacking;
		comboCount = comboCount + 1;
		float attackDelay = 0;
		bool active = false;
		while (attackDelay < attackDuration) {
			if (bodyScript.state == Body.statelist.attacking) {
				attackDelay = attackDelay + Time.deltaTime;
				if (attackDelay > attackActiveStart && active == false) {
					active = true;
					GameObject temp = Instantiate (attackCol,this.transform.position,Quaternion.AngleAxis (bodyScript.direction * Mathf.Rad2Deg, Vector3.down));
					foreach (var i in temp.GetComponentsInChildren<AttackCollider> ()) {
						i.attackDamage = attackDamage;
						i.knockbackIntensity = knockbackIntensity;
						i.knockbackDecay = knockbackDecay;
						i.knockbackLimit = knockbackLimit;
						i.knockbackGrounded = knockbackGrounded;
						i.attackActiveDuration = attackActiveDuration;
						i.attackStunDuration = attackStunDuration;
						i.attackAllDirection = attackAllDirection;
						i.attackPositionTrack = attackPositionTrack;
						i.attackSource = this.gameObject;
					}
				}
				bodyScript.translate = attackMovement * Time.deltaTime / attackDuration * (Quaternion.AngleAxis (bodyScript.direction * Mathf.Rad2Deg, Vector3.down) * Vector3.right);
				yield return null;
			} else {
				comboCount = 0;
				yield break;
			}
		}
		bodyScript.state = Body.statelist.neutral;
		comboCount = 0;
	}
}
