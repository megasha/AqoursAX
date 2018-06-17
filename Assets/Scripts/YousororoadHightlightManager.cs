using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YousororoadHightlightManager : MonoBehaviour {
    public GameObject pit;
    public GameObject orchestra;
    public GameObject lodge;


	// Use this for initialization
	void Start () {
        pit.GetComponent<Renderer>().enabled = false;
        orchestra.GetComponent<Renderer>().enabled = false;
        lodge.GetComponent<Renderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            pit.GetComponent<Renderer>().enabled = !pit.GetComponent<Renderer>().enabled;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            orchestra.GetComponent<Renderer>().enabled = !orchestra.GetComponent<Renderer>().enabled;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            lodge.GetComponent<Renderer>().enabled = !lodge.GetComponent<Renderer>().enabled;
        }
    }
}
