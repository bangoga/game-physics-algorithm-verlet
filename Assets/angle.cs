using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angle : MonoBehaviour {
    public GameObject b;
    Vector3 p1;
    Vector3 p2;
	// Use this for initialization
	void Start () {
        p1 = this.transform.position;
        p2 = b.transform.position;

        float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;

        print("angle is"+angle);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
