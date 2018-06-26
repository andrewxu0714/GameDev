using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float x_offset;
	public float y_offset;
	public float z_offset;
	public GameObject followTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = followTarget.transform.position + new Vector3 (x_offset, y_offset, z_offset);
	}
}
