using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KananrailHighlightManager : MonoBehaviour {
    public GameObject leftPit;
    public GameObject rightPit;
    public GameObject leftOrchestra;
    public GameObject rightOrchestra;
    public GameObject leftLodge;
    public GameObject rightLodge;

    // Use this for initialization
    void Start () {
        leftPit.GetComponent<Renderer>().enabled = false;
        rightPit.GetComponent<Renderer>().enabled = false;
        leftOrchestra.GetComponent<Renderer>().enabled = false;
        rightOrchestra.GetComponent<Renderer>().enabled = false;
        leftLodge.GetComponent<Renderer>().enabled = false;
        rightLodge.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            leftPit.GetComponent<Renderer>().enabled = !leftPit.GetComponent<Renderer>().enabled;
            rightPit.GetComponent<Renderer>().enabled = !rightPit.GetComponent<Renderer>().enabled;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            leftOrchestra.GetComponent<Renderer>().enabled = !leftOrchestra.GetComponent<Renderer>().enabled;
            rightOrchestra.GetComponent<Renderer>().enabled = !rightOrchestra.GetComponent<Renderer>().enabled;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            leftLodge.GetComponent<Renderer>().enabled = !leftLodge.GetComponent<Renderer>().enabled;
            rightLodge.GetComponent<Renderer>().enabled = !rightLodge.GetComponent<Renderer>().enabled;
        }
    }
}
