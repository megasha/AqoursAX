using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;

public class UOManager : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //-45 to 45 deg and back
        float val = Mathf.Sin(Time.time*14);
        val = val * 45;

        transform.eulerAngles = new Vector3(val, 0, 0);
	}
}
