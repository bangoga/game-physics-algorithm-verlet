using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour {
    public GameObject ground;
    public GameObject Cannon_Prefab;
    public GameObject TxGen;

    public GameObject Cloud1;
    public GameObject Cloud2;

    public List<GameObject> mountain;
    Cannon Cannon;

    public GameObject chicken;
    public GameObject chicken2;
    public GameObject chicken3;
    public GameObject chicken4;
    public GameObject chicken5;

    public GameObject chickenButt;
    public GameObject chickenButt2;
    public GameObject chickenButt3;
    public GameObject chickenButt4;
    public GameObject chickenButt5;
    public GameObject wall;

    Edge[] all_chicken_egdges;
    Edge[] all_chicken_egdges2;
    Edge[] all_chicken_egdges3;
    Edge[] all_chicken_egdges4;
    Edge[] all_chicken_egdges5;
    public float windspeed {get;set;}
    // get windfactor, add wind to each ball and cloud

    void Start()
    {

        all_chicken_egdges = chicken.GetComponent<TurkeyAnimation>().all_edges;
        all_chicken_egdges2 = chicken2.GetComponent<TurkeyAnimation>().all_edges;
        all_chicken_egdges3 = chicken3.GetComponent<TurkeyAnimation>().all_edges;
        all_chicken_egdges4 = chicken4.GetComponent<TurkeyAnimation>().all_edges;
        all_chicken_egdges5 = chicken5.GetComponent<TurkeyAnimation>().all_edges;

        Cannon = Cannon_Prefab.GetComponent<Cannon>();
        mountain = TxGen.GetComponent<Generator>().all_blocks;
        InvokeRepeating("windEffect", 0.2f, 0.5f);
        InvokeRepeating("cleanup", 0.2f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate() {
        GroundCollisions();
        wallCollision();
    }


    // -----------[ These two functions in combination add random wind ] ----------// 
    void changeWindRandomly()
    {
       windspeed = UnityEngine.Random.Range(-0.15f, 0.15f);
    }

    void windEffect()
    {
        // after every time step 
        changeWindRandomly();

       // print("Wind is : " + windspeed);
        if (Cloud1 != null)
        {
            float cloudx = Cloud1.GetComponent<CustomRigidBody>().angVelocity.x;
            float cloudy = Cloud1.GetComponent<CustomRigidBody>().angVelocity.y;
            Cloud1.GetComponent<CustomRigidBody>().addforce(cloudx/1.2f,cloudy/2,0);
            Cloud1.GetComponent<CustomRigidBody>().setWind(windspeed/5);
        }

        if (Cloud2 != null)
        {
            float cloudx = Cloud2.GetComponent<CustomRigidBody>().angVelocity.x;
            float cloudy = Cloud2.GetComponent<CustomRigidBody>().angVelocity.y;
            Cloud2.GetComponent<CustomRigidBody>().addforce(cloudx/1.2f, cloudy / 2, 0);
            Cloud2.GetComponent<CustomRigidBody>().setWind(windspeed/5);
        }
        if (Cannon.CannonBalls.Count != 0)
        {
            for (int i = 0; i < Cannon.CannonBalls.Count; i++)
            {
                Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().setWind(windspeed);
            }
        }
     }

    // ------------------------------ [ Reaction and Collision of ball with wall ] --------------//
    void wallCollision()
    {
        if (Cannon.CannonBalls.Count != 0)
        {
            // Checking for collisions with the ground 
            for (int i = 0; i < Cannon.CannonBalls.Count; i++)
            {
                if (CheckCollision(wall, Cannon.CannonBalls[i]))
                {

                    // Create bounce on Collision. 
                    float newforcex = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().forcex;
                    float newforcey = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().forcey;
                    float newangle = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().angle;
                    Vector3 p1 = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().oldposition;
                    Vector3 p2 = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().newposition;

                    float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
                    float vel = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().velocity;
                    // restart gravity velocity
                    Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().velocity = 0;

                    newforcex = newforcex * 0.7f; // lower the x
                    newforcey = (newforcey * 0.6f); // lower the y

                    print(90-angle);
                    angle = (90 - angle) * Mathf.PI / 180;
                    //angle = angle * (Mathf.PI / 180);

                      Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().addforce(newforcex,newforcey, angle);
                }
            }
        }
    }

    // ------------------------------ [ Ground and Mountain with Ball Collision ] --------------//
    void GroundCollisions()
    {   
      
        if (Cannon.CannonBalls.Count != 0) { 
        // Checking for collisions with the ground 
        for(int i = 0; i < Cannon.CannonBalls.Count; i++)
        {
                if (CheckCollision(ground, Cannon.CannonBalls[i])) {
                    //print("Collided");

                    // Create bounce on Collision. 
                    float newforcex = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().forcex;
                    float newforcey = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().forcey;
                    float newangle = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().angle;
                    float vel = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().velocity;
                    Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().velocity = 0; // restart gravity velocity
                    Vector3 p1 = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().oldposition;
                    Vector3 p2 = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().newposition;


                    newforcex = newforcex * 0.8f; // lower the x
                    newforcey = (newforcey * 0.8f); // lower the y

                    float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
                   

                    print("Ground angle is " + angle);
                    angle = 180 - Mathf.Abs(angle);
                    
                    angle = angle * (Mathf.PI / 180);


                    if (newforcex < 0.01 && Cannon.CannonBalls[i].transform.position.y<-3.3)
                    {
                        Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().gravity = 0;
                        GameObject o = Cannon.CannonBalls[i];
                        Cannon.CannonBalls.Remove(o);
                        Destroy(o);
                        break;
                    }


                    Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().addforce(newforcex,newforcey, angle);
                }
                // detect cannon on the wall.
                for (int j = 0; j < mountain.Count; j++)
                {
                    if (CheckCollision(mountain[j], Cannon.CannonBalls[i]))
                    {

                        // Create bounce on Collision. 
                        float newforcex = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().forcex;
                        float newforcey = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().forcey;
                        float newangle = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().angle;
                        Vector3 p1 = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().oldposition;
                        Vector3 p2 = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().newposition;

                        float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
                        float vel = Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().velocity;
                        // restart gravity velocity
                        Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().velocity = 0;

                        newforcex = newforcex * 1f; // lower the x
                        newforcey = (newforcey * 1f); // lower the y

                        print("angle is going to be "+angle);
                        angle = (90 - angle) * Mathf.PI / 180;
                        //angle = angle * (Mathf.PI / 180);

                        Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().addforce(newforcex, newforcey, angle);
                    }

                }

            }
        }

        if (mountain.Count > 0)
        {
            for (int j = 0; j < mountain.Count; j++)
            {
                if (CheckCollisionChicken(mountain[j], chickenButt))
                {
                    chicken.GetComponent<TurkeyAnimation>().setAnimateLeft();
                    //print("collide");
                }
                if (CheckCollisionChicken(mountain[j], chickenButt2))
                {
                    chicken2.GetComponent<TurkeyAnimation>().setAnimateLeft();
                    //print("collide");
                }
                if (CheckCollisionChicken(mountain[j], chickenButt3))
                {
                    chicken3.GetComponent<TurkeyAnimation>().setAnimateLeft();
                    //print("collide");
                }
                if (CheckCollisionChicken(mountain[j], chickenButt4))
                {
                    chicken4.GetComponent<TurkeyAnimation>().setAnimateLeft();
                    //print("collide");
                }
                if (CheckCollisionChicken(mountain[j], chickenButt5))
                {
                    chicken5.GetComponent<TurkeyAnimation>().setAnimateLeft();
                    //print("collide");
                }
            }

        }

    }

    // customize collision to accomodate for vertex rather than box collider 
    // check collision between object and groud 

    bool CheckCollision(GameObject a, GameObject b) // AABB
    {

        // Checking left and right
        float d1x = b.GetComponent<CustomCollider>().getMin().x - a.GetComponent<CustomCollider>().getMax().x;
        float d2x = a.GetComponent<CustomCollider>().getMin().x - b.GetComponent<CustomCollider>().getMax().x;

        // Checking up and down 
        float d1y = b.GetComponent<CustomCollider>().getMin().y - a.GetComponent<CustomCollider>().getMax().y; // drop
        float d2y = a.GetComponent<CustomCollider>().getMin().y - b.GetComponent<CustomCollider>().getMax().y; // top
        
        
        if (d1x > 0.0f || d1y > 0.0f)
        {
            
            return false;
        }

        if (d2x > 0.0f || d2y > 0.0f)
        {
           
            return false;
        }

       // print("collide");
        return true;
    }

    //------------------------------------[All Chicken Related Collisions]--------------------///
    bool CheckCollisionChicken(GameObject a, GameObject b) // AABB
    {
        // Checking left and right
        float d1x = b.transform.position.x - a.GetComponent<CustomCollider>().getMax().x;
        float d2x = a.GetComponent<CustomCollider>().getMin().x - b.transform.position.x;

        // Checking up and down 
        float d1y = b.transform.position.y - a.GetComponent<CustomCollider>().getMax().y; // drop
        float d2y = a.GetComponent<CustomCollider>().getMin().y - b.transform.position.y; // top

        if (d1x > 0.0f || d1y > 0.0f)
        {

            return false;
        }

        if (d2x > 0.0f || d2y > 0.0f)
        {

            return false;
        }

       // print("collide");
        return true;
    }

    bool EdgeToBoxCollision(GameObject aO, GameObject bO, Vector2 c, Vector2 d)
    {

        Vector3 a = aO.transform.position;
        Vector3 b = bO.transform.position;

    /* for testing only
        ax = a.x;
        bx = b.x;
        ay = a.y;
        by = b.y;
    */
        float denominator = ((b.x - a.x) * (d.y - c.y)) - ((b.y - a.y) * (d.x - c.x));
        float numerator1 = ((a.y - c.y) * (d.x - c.x)) - ((a.x - c.x) * (d.y - c.y));
        float numerator2 = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));

        // Detect coincident lines (has a problem, read below)
        if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

        float r = numerator1 / denominator;
        float s = numerator2 / denominator;

        return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
    }

    // Clean up the balls in the given specifications
    void cleanup()
    {
        if (Cannon.CannonBalls.Count != 0)
        {
            for (int i = 0; i < Cannon.CannonBalls.Count; i++)
            {
                if (Cannon.CannonBalls[i].transform.position.x<-8.5 || Cannon.CannonBalls[i].transform.position.x > 8.5)
                {
                    GameObject o = Cannon.CannonBalls[i];
                    Cannon.CannonBalls.Remove(o);
                    Destroy(o);
                    break;
                }
                if (Cannon.CannonBalls[i].transform.position.x < -6 && (Cannon.CannonBalls[i].transform.position.x - Cannon.CannonBalls[i].GetComponent<CustomRigidBody>().oldposition.x >0))
                {
                    GameObject o = Cannon.CannonBalls[i];
                    Cannon.CannonBalls.Remove(o);
                    Destroy(o);
                    break;
                }
            }

        }

    }
}
