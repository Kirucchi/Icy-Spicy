using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed;
    private float angle;
    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        angle += Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, 0, angle);
	}
}
