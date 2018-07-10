using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationLead : MonoBehaviour {

	public Vector3 goal;
	public GameObject chaser;
	public NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position - chaser.transform.position).magnitude > 1) {
			transform.position = chaser.transform.position+(transform.position - chaser.transform.position).normalized*0.5f;
		}
		agent.destination = goal;		
	}
}
