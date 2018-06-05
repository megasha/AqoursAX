using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;

public class UOManager : MonoBehaviour {

    public string tag;
    public GameObject[] objects;

    // Use this for initialization
    void Start () {
        objects = GameObject.FindGameObjectsWithTag(tag);
	}
	
	// Update is called once per frame
	void Update () {
        //-45 to 45 deg and back
        float val = Mathf.Sin(Time.time * 14);
        val = val * 45;
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.eulerAngles = new Vector3(val, 0, 0);
        }
	}
}
