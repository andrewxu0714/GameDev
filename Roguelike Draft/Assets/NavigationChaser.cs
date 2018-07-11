using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationChaser : MonoBehaviour {

	public Vector3 goal;
	private Body bodyScript;
	private NavMeshPath movePath;
	private int trackPath;

	// Use this for initialization
	void Start () {
		movePath = new NavMeshPath ();
		bodyScript = GetComponent<Body> ();
		goal = new Vector3(29,0.5f,29);
		trackPath = 0;
	}
	
	// Update is called once per frame
	void Update () {
		NavMesh.CalculatePath (transform.position, goal, NavMesh.AllAreas, movePath);
		if (bodyScript.CanAct () && movePath.corners.Length>1) {
			Vector3 positionAdjust = new Vector3 (this.transform.position.x, 0, this.transform.position.z);
			Vector3 targetAdjust = new Vector3 (movePath.corners [1].x, 0, movePath.corners [1].z);
			Debug.Log ((targetAdjust - positionAdjust).magnitude);
			if ((targetAdjust - positionAdjust).magnitude < (10f * Time.deltaTime)) {
				bodyScript.translate = targetAdjust - positionAdjust;
			} else {
				bodyScript.translate = (targetAdjust - positionAdjust).normalized * 10f * Time.deltaTime;
			}
		}
		for (int i = 0; i < movePath.corners.Length-1; i++)
			Debug.DrawLine(movePath.corners[i], movePath.corners[i+1], Color.red);	
	}
}
