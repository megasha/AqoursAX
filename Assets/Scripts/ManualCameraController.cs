using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCameraController : MonoBehaviour {

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    public float moveSpeed = 10.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float shiftMod = 1.0f;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift))
            shiftMod = 3.0f;
        else
            shiftMod = 1.0f;

        //Forward
		if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * shiftMod);
        //Backward
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed * shiftMod);
        //Left
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed * shiftMod);
        //Backward
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * shiftMod);

        //Up
        if (Input.GetKey(KeyCode.Space))
            transform.position += Vector3.up * Time.deltaTime * moveSpeed * shiftMod;

        //Down
        else if (Input.GetKey(KeyCode.Z))
            transform.position += Vector3.down * Time.deltaTime * moveSpeed * shiftMod;

        //Orientation
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }
}
