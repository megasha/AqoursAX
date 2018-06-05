using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientPenlight : MonoBehaviour {

    public string tag;
    public GameObject lookAtObject;

    public List<GameObject> objects;
    private List<float> lastVal;



    // Use this for initialization
    void Start () {
        lastVal = new List<float>();
        objects = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
        
        Debug.Log(objects.Count);

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LookAt(lookAtObject.transform, objects[i].transform.up);
            lastVal.Add(0.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        float val = Mathf.Sin(Time.time*14);
        val = val * 45;

        for (int i = 0; i < objects.Count; i++)
        {
            //Rotate 30 degrees every frame
            Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastVal[i], 0, 0));
            objects[i].transform.rotation = objects[i].transform.rotation * localRotation;
            lastVal[i] = val;
            //objects[i].transform.Rotate(new Vector3(1.0f, 0.0f,0.0f));
            //Debug.Log(objects[i].transform.localRotation.eulerAngles);
        }
    }
}
