using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour {

    public bool beingCarried = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (beingCarried)
            transform.localPosition = new Vector3(0, 0, 0);
	}
}
