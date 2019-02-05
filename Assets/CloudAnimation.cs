using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnimation : MonoBehaviour {

    public GameObject left_screen_boundry;
    public GameObject right_screen_boundry;

    public GameObject leftline;
    public GameObject rightline;

    Vector3 left_screen;
    Vector3 right_screen;

    Vector3 left_cloud;
    Vector3 right_cloud;

    Vector3 CurrentCloudsPosition;

    Vector3 movement_speed;

    public string animate_direction = "left";
	// Use this for initialization
	void Start () {

        movement_speed = new Vector3(0.02f, 0, 0);
        // Get bounds of screen;
        left_screen  = left_screen_boundry.transform.position;
        right_screen = right_screen_boundry.transform.position;

        // Get bounds of clouds
        left_cloud = leftline.transform.position;
        right_cloud = rightline.transform.position;

       // print(left_cloud);
       // print(left_screen);
    }
	
	// Update is called once per frame
	void Update () {
        updatePositions();
        checkBoundries(animate_direction);
        animateCloud();
	}

    void updatePositions()
    {
        left_screen = left_screen_boundry.transform.position;
        right_screen = right_screen_boundry.transform.position;

        // Get bounds of clouds
        left_cloud = leftline.transform.position;
        right_cloud = rightline.transform.position;

    }

    public void animateCloud()
    {

        CurrentCloudsPosition = this.transform.position;
        if (animate_direction == "left")
        {

            this.CurrentCloudsPosition = this.CurrentCloudsPosition - this.movement_speed * Time.deltaTime;
            this.transform.position = (this.CurrentCloudsPosition); // use delta time for nice movemement 
 
        }

        if(animate_direction == "right")
        {
            this.CurrentCloudsPosition = this.CurrentCloudsPosition + this.movement_speed * Time.deltaTime;
            this.transform.position = (this.CurrentCloudsPosition); // use delta time for nice movemement 
        }
    }

    public void setAnimateLeft()
    {
        
        animate_direction = "left";
    }

    public void setAnimateRight()
    {
        animate_direction = "right";
    }

    void checkBoundries(string side)
    {

        if (side == "left")
        {
            //print(left_cloud.x);
            float d1x = left_screen.x - left_cloud.x ;
            float d2x = right_screen.x - right_cloud.x;

            if (d1x > 0.0f && d2x > 0.0f)
            {
               // print("d1x : "+d1x + " d2x : " + d2x);
                setAnimateRight();
            }

        }

        if (side == "right")
        {
            float d1x = left_screen.x - left_cloud.x;
            float d2x = right_screen.x - right_cloud.x;
 

            if (d2x  < 0.0f )
            {
                //print("collider rightt");
                setAnimateLeft();
            }

        }

    }

}
