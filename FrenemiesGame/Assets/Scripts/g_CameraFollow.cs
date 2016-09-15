using UnityEngine;
using System.Collections;

public class g_CameraFollow : MonoBehaviour {

    private Transform player;
    

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
       
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = player.position;
	
	}
}
