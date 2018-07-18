using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {

	public Vector3 goal;
	public Vector3 monitor;
	private Body bodyScript;
	private NavMeshPath movePath;
	private int trackPath;

	// Use this for initialization
	void Start () {
		movePath = new NavMeshPath ();
		bodyScript = GetComponent<Body> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (bodyScript.CanAct ()) {
			NavMeshHit goaladj = new NavMeshHit();
			bool temp=NavMesh.SamplePosition (goal, out goaladj, 3f, NavMesh.AllAreas);
			if (temp) {
				NavMesh.CalculatePath (transform.position, goaladj.position, NavMesh.AllAreas, movePath);
			} else {
				NavMesh.CalculatePath (transform.position, transform.position, NavMesh.AllAreas, movePath);
			}
			if (movePath.corners.Length > 1) {
				bodyScript.state = Body.statelist.moving;
				Vector3 positionAdjust = new Vector3 (this.transform.position.x, 0, this.transform.position.z);
				Vector3 targetAdjust = new Vector3 (movePath.corners [1].x, 0, movePath.corners [1].z);
				monitor = targetAdjust;
				Vector3 offset = targetAdjust - positionAdjust;
				if (offset.magnitude < (10f * Time.deltaTime)) {
					bodyScript.translate = offset;
				} else {
					bodyScript.translate = offset.normalized * 10f * Time.deltaTime;
				}
				bodyScript.direction = Mathf.Atan2 (offset.z, offset.x);
			} else {
				bodyScript.state = Body.statelist.neutral;
			}
		}
		for (int i = 0; i < movePath.corners.Length-1; i++)
			Debug.DrawLine(movePath.corners[i], movePath.corners[i+1], Color.red);	
	}
}
