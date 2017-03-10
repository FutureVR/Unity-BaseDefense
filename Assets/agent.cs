using UnityEngine;
using System.Collections;

public class agent : MonoBehaviour {

    NavMeshAgent navAgent;

	void Start ()
    {
        navAgent = GetComponent<NavMeshAgent>();
	}
	
	void Update ()
    {
        navAgent.destination = new Vector3(0, 0, 0);
	}
}
