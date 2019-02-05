using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {
    GameObject ball;
    public Vector2 Min;
    public Vector2 Max;
    float widthmax;
    float heightmax;
    float widthmin;
    float heightmin;

    public float mass;
    public float intertia;
    float position = 0;
    float acceleration = 9.81f/Screen.height;
    float velocity = 0; 


    Bounds b;
    // Use this for initialization
    void Start () {

        // --- Handling the bounds -----//

        ball = this.gameObject;
        b = ball.GetComponent<Collider2D>().bounds;
        float xaxis=ball.transform.position.x;
        float yaxis=ball.transform.position.y;
        widthmax = xaxis + b.extents.x;
        widthmin = xaxis - b.extents.x;
        heightmax = yaxis + b.extents.y;
        heightmin = yaxis - b.extents.y;

        Min = new Vector2(widthmin, heightmin);
        Max = new Vector2(widthmax, heightmax);
        ///print("Extends : " + b.extents);
        //print("center : " + b.center);
        print(Time.deltaTime);


        //        print("Min Width : " + widthmin + "| Height : " + heightmin);
    }
	
    void calculateIntertia()
    {
        float m = mass;
        float h = heightmax - heightmin;
        float w = widthmax - widthmin;
        intertia = m * (w * w + h * h) / 12;
    }


	// Update is called once per frame
	void Update () {
        UpdatePosistion(); // updates the position of the box colliders 
        //print(Time.deltaTime);
    }

    void UpdatePosistion()
    {
        b = ball.GetComponent<Collider2D>().bounds;
        float xaxis = ball.transform.position.x;
        float yaxis = ball.transform.position.y;
        widthmax = xaxis + b.extents.x;
        widthmin = xaxis - b.extents.x;
        heightmax = yaxis + b.extents.y;
        heightmin = yaxis - b.extents.y;

        Min = new Vector2(widthmin, heightmin);
        Max = new Vector2(widthmax, heightmax);
    }

    public Vector2 getMin()
    {
        return Min;
    }

    public Vector2 getMax()
    {
        return Max;
    }
}
