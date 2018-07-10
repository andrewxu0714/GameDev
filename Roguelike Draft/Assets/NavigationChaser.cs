using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationChaser : MonoBehaviour {

	public GameObject navLead;
	public Vector3 goal;
	private Body bodyScript;

	// Use this for initialization
	void Start () {
		bodyScript = GetComponent<Body> ();
		navLead = Instantiate (navLead, this.transform);
		navLead.GetComponent<NavigationLead> ().chaser = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//goal = this.transform.position + Vector3.right;
		goal = new Vector3(32,0,32);
		navLead.GetComponent<NavigationLead> ().goal = goal;
		if (bodyScript.CanAct ()) {
			bodyScript.translate = Mathf.Min(10f*Time.deltaTime,(navLead.transform.position - this.transform.position).magnitude)*(navLead.transform.position - this.transform.position).normalized;
		}
	}
}
