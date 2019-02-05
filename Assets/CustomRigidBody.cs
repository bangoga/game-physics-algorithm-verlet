using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRigidBody : MonoBehaviour
{

    public Vector3 position { get; set; }
    public Vector3 oldposition { get; set; }
    public Vector3 newposition { get; set; }
    public Vector3 Acceleration { get; set; }
    public Vector3 LinVelocity { get; set; }
    public Vector3 angVelocity { get; set; }

    public float angle { get; set; }
    public float forcex { get; set; }
    public float forcey { get; set; }


    public float gravity { get; set; }
    public float posit = 0;
    public float velocity = 0;

    public bool isGravity;
    public bool grounded { get; set; }
    public bool apex; // If vy = 0, then at apex 

    float windEffect;

    float lastMaxY;
    float CurrentMaxy;
   

    // Use this for initialization

    // time element has to be added too 

    void Start()
    {
        oldposition = newposition = this.transform.position;
        lastMaxY = this.transform.position.y;
        CurrentMaxy = this.transform.position.y;
       // print(CurrentMaxy);
        apex = false;
        position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        grounded = false;
        gravity = 40f;
    }

    // Gravity and the Euler act seperately Gravity based on velocity verlet and projectile on euler

    void Update()
    {
        lastMaxY = CurrentMaxy;
        CurrentMaxy = this.transform.position.y;

        oldposition = newposition;
        newposition = this.transform.position;
        if (CurrentMaxy - lastMaxY < 0.0f)
        {
            apex = true; // set apex to true, only after apex is true, wind acts
        }

        addWind();
        Euler();
        addGravity();

    }


   
    // Add forces to any given rigidbody, in my case used this for cannon
    public void addforce(float forcex, float forcey, float angle) // this is adding an acceleration to the object 
    {
        // - gravity * time 
        this.forcex = forcex;
        this.forcey = forcey;
        this.angle = angle;
        this.angVelocity = new Vector3(-forcex * Mathf.Cos(angle), forcey * Mathf.Sin(angle), -0.1f);
        // print(this.angVelocity);
    }

    // Projectile force is non verlet due to the fact, we are not acting acceleration to it, we want a steady speed. We will cause initial speed at first,
    // and let newtons 2nd law act for a steady speed with no acceleration aka no wind resistance 
    public void Euler()
    {
        CustomRigidBody P = this;
        float Vx = P.angVelocity.x;
        float Vz = P.angVelocity.z;
        float Vy = P.angVelocity.y; // slows down the velocity, but the gravity is still not actiing on it 
        Vector3 newAngVelocity = new Vector3(Vx, Vy, Vz);


        // distance = change in velocity * change in time, the distance is being travelled in linearly without any hinderance 
        P.position = P.position + P.angVelocity * Time.deltaTime; // assuming a constant velocity and no wind resistance 
        P.transform.position = (P.position); // use delta time for the gravity effect  

    }

        // Using velocity verlet for gravity 
    public void addGravity()
    {
        // --- Act gravity -- // 
        if (isGravity && !grounded)
        {

            posit += Time.deltaTime * (velocity + Time.deltaTime * gravity / 2);
            velocity += Time.deltaTime * gravity;
            this.transform.Translate(0, -posit, 0); // use delta time for the gravity effect  
        }
    }

    public void setWind(float wind)
    {
        windEffect = wind;
    }



    // Add in a certain direction w > 0 going to the left, w < 0, going to the right 
    // this is decided by the customphyiscs script
    public void addWind()
    {

        // as is called in timestep, is added again and again
        if (apex || this.transform.name == "Clouds" || this.transform.name == "Clouds (1)")
        {
            // or use the posit approach 
            
            this.angVelocity = new Vector3(this.angVelocity.x +windEffect, this.angVelocity.y, this.angVelocity.z);
        }
    }

}
