using UnityEngine;
using System.Collections;

public class g_textFollow : MonoBehaviour {

    private GameObject other;


    void Awake() {
        other = GameObject.FindGameObjectWithTag("infoBuddy");
    }

	void Update () {
       transform.position = other.transform.position;
	}
}
