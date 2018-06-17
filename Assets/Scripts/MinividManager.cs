using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinividManager : MonoBehaviour {
	// Use this for initialization
	void Start () {
        GetComponent<RawImage>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GetComponent<RawImage>().enabled = !GetComponent<RawImage>().enabled;
        }
    }
}
