using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float speed = 1.0f;
        float shake = 1.0f;

        //gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0, Mathf.Sin(Time.time * 30) * 15));

        gameObject.transform.rotation = Quaternion.Euler(0 , 0, Mathf.Sin(Time.time * 30) * 10);
	}
}
