using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	private Body bodyScript;

	private int combo=5;
	private float[] comboDamage = new float[5] {10f,5f,5f,5f,25f};
	private float[] comboDuration = new float[5] {0.5f,0.5f,0.5f,0.5f,3f};
	private float[] comboActiveStart = new float[5] {0.1f,0.1f,0.1f,0.1f,0.1f}; 
	private float[] comboActiveDuration = new float[5] {0.3f,0.3f,0.3f,0.3f,0.3f};
	private float[] comboStunDuration = new float[5] {0.5f,0.5f,0.5f,0.5f,0.5f};
	private bool[] comboAllDirection = new bool[5] {false,false,false,false,false};
	public GameObject[] comboCol = new GameObject[5];

	private int attackBuffer=0;
	private int comboCount=0; //represents the stats of the NEXT attack

	private float inputDuration=0.2f;
	private float inputDelay=0f;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();		
	}
	
	// Update is called once per frame
	void Update () {

		inputDelay = Mathf.Max (inputDelay - Time.deltaTime, 0);

		if (Input.GetAxis ("Fire1") > 0 && inputDelay <= 0 && (bodyScript.CanAct () || bodyScript.state == Body.statelist.attacking)) {
			attackBuffer = Mathf.Min (combo - comboCount, attackBuffer + 1);
			inputDelay = inputDelay + inputDuration;
		}

		if (attackBuffer > 0 && bodyScript.CanAct()) {
			StartCoroutine (Strike(comboDamage[comboCount],comboDuration[comboCount],comboActiveStart[comboCount],comboActiveDuration[comboCount],comboStunDuration[comboCount],comboAllDirection[comboCount],comboCol[comboCount]));
		}
	}
		
	IEnumerator Strike(float attackDamage, float attackDuration, float attackActiveStart, float attackActiveDuration, float attackStunDuration, bool attackAllDirection, GameObject attackCol) {
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
				if (attackBuffer > 0 && comboCount <= combo && attackDelay > attackActiveStart + attackActiveDuration) {
					StartCoroutine (Strike(comboDamage[comboCount],comboDuration[comboCount],comboActiveStart[comboCount],comboActiveDuration[comboCount],comboStunDuration[comboCount],comboAllDirection[comboCount],comboCol[comboCount]));
					yield break;
				} else {
					attackDelay = attackDelay + Time.deltaTime;
					if (attackDelay > attackActiveStart && active == false) {
						active = true;
						GameObject temp = Instantiate (attackCol, this.transform);
						foreach (var i in temp.GetComponentsInChildren<AttackCollider> ()) {
							i.attackDamage = attackDamage;
							i.attackActiveDuration = attackActiveDuration;
							i.attackStunDuration = attackStunDuration;
							i.attackAllDirection = attackAllDirection;
							i.attackSource = this.gameObject;
						}
						temp.transform.rotation = Quaternion.AngleAxis (Mathf.Rad2Deg * bodyScript.direction, Vector3.down);
					}
					yield return null;
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
