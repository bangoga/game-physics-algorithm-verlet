using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    public GameObject Vertex_A;
    public GameObject Vertex_B;

    public Vector3 A { get; set; }
    public  Vector3 B { get; set; }

    public float angle;
    public float distance;

    private LineRenderer line;

    public Vector3 oldposition;
    public Vector3 newposition;
    public Vector3 acc;


    // Use this for initialization
    void Start () {
        acc = Vector3.zero;
        oldposition = newposition = this.transform.position;

        A = Vertex_A.transform.position;
        B = Vertex_B.transform.position;
        angle = Mathf.Atan2(B.y - A.y, B.x - A.x) * 180 / Mathf.PI;
        distance = Vector3.Distance(A, B);

        line = this.gameObject.GetComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.useWorldSpace = true;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;


    }

    void Update()
    {
        A = Vertex_A.transform.position;
        B = Vertex_B.transform.position;

        UpdateLine();
    }

    private void  UpdateLine()
    {
        if (Vertex_A != null && Vertex_A != null)
        {
            // Update position of the two vertex of the Line Renderer
            line.SetPosition(0, Vertex_B.transform.position);
            line.SetPosition(1, Vertex_A.transform.position);
        }
    }

    public Vector3 PositionA()
    {
        return A;
    }
    public Vector3 PositionB()
    {
        return B;
    }

    public void SetPositionA(Vector3 newA)
    {
        A = newA;
        Vertex_A.transform.position = A;
    }
    public void SetPositionB(Vector3 newB)
    {
        B = newB;
        Vertex_B.transform.position = B;
    }


    // unused. 
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
