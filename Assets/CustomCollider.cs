using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollider : MonoBehaviour {
    public Vector2 Min;
    public Vector2 Max;
    float widthmax;
    float heightmax;
    float widthmin;
    float heightmin;

    // We can also make it show as points on the corner 
    public Vector2 NW;
    public Vector2 SW;
    public Vector2 NE;
    public Vector2 SE;


    Bounds b;

    void Start () {
        decideBounds();
    }
	
	// Update is called once per frame
	void Update () {
        decideBounds();   
    }

    void decideBounds()
    {
        b = this.GetComponent<Collider2D>().bounds;
        float xaxis = this.transform.position.x;
        float yaxis = this.transform.position.y;
        widthmax = xaxis + b.extents.x;
        widthmin = xaxis - b.extents.x;
        heightmax = yaxis + b.extents.y;
        heightmin = yaxis - b.extents.y;

        Min = new Vector2(widthmin, heightmin);
        Max = new Vector2(widthmax, heightmax);

        SW = Min;
        NE = Max;
        NW = new Vector2(widthmin, heightmax);
        SE = new Vector2(widthmax, heightmin);

    }

    public Vector2 getMin()
    {
        return Min;
    }

    public Vector2 getMax()
    {
        return Max;
    }

    public void calculateCorners()
    {
        b = this.GetComponent<Collider2D>().bounds;
        float xaxis = this.transform.position.x;
        float yaxis = this.transform.position.y;

        
    }
}
