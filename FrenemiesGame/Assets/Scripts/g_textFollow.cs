using UnityEngine;
using System.Collections;

public class g_textFollow : MonoBehaviour {

    public GameObject other;

	
	

	void Update () {
        this.transform.position = other.transform.position;
	}
}
