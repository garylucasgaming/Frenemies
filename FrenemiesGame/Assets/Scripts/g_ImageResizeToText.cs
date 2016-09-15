using UnityEngine;
using System.Collections;

public class g_ImageResizeToText : MonoBehaviour {

    private GameObject text;

    void Awake() {
        text = GameObject.FindGameObjectWithTag("infoBuddyText");
    }

    void updateSize() {
        Vector3 targetSize = text.GetComponent<Renderer>().bounds.size;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
