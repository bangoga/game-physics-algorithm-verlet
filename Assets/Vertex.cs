using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vertex : MonoBehaviour {
    public Vector3 oldposition;
    public Vector3 newposition;
    public Vector3 acc;

    void Start()
    {
        acc = Vector3.zero;
        oldposition = newposition = this.transform.position;
    }

    private void Update()
    {
        verlet();
    }
    public void applyforce(Vector3 force)
    {
        this.acc = force;
        print("hey force" + force);
    }

    public void verlet()
    {
        Vector3 Temp = this.transform.position;
        this.transform.position = this.transform.position + (this.transform.position - this.oldposition) + acc * Time.deltaTime * Time.deltaTime;
        this.oldposition = Temp;
        acc = Vector3.zero;
        
    }
    public void applyImpulse(Vector3 impulse)
    {
        this.acc = this.acc - impulse;
    }

}
