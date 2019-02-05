using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurkeyAnimation : MonoBehaviour {

    public GameObject left_screen_boundry;
    public GameObject right_screen_boundry;
    public GameObject beak;
    public GameObject ass;
    public GameObject leg1;
    public GameObject leg2;
    public GameObject Center;

    bool hit;

    public float gravity { get; set; }
    public float posit = 0;
    public float velocity = 0;

    Vector3 oldposition;
    // public GameObject c;
    Vector3 left_screen;
    Vector3 right_screen;
    Vector3 beakPosition;
    Vector3 assPosition;

    Vector3 centerOfRotation;

    Vector3 TurkeyPosition;

    Vector3 movement_speed;

    public GameObject Cannon_Prefab;

    Cannon cannon;
    public string animate_direction;
    public string leg1rotaiton = "left";
    public string leg2rotation = "right";

    // Adding constraints

    //public GameObject TxGen;
    //public List<GameObject> mountain;

    public Edge[] all_edges; // save all the edges 
    public Vertex[] vertices;
    private LineRenderer line;
    public Vector3 acc;
    bool j;


    void Start()
    {

        float r = Random.Range(0, 10);
        if (r > 5)
        {
            animate_direction = "left";
        }
    
        if (r <= 5)
        {
            animate_direction = "right";
        }

        hit = false;
        cannon = Cannon_Prefab.GetComponent<Cannon>();
        oldposition = this.transform.position;
        gravity = 1f;
        //acc = new Vector3(0, 0.5f, 0);
        vertices = GameObject.FindObjectsOfType<Vertex>();
        all_edges = GameObject.FindObjectsOfType<Edge>();
      //  mountain = TxGen.GetComponent<Generator>().all_blocks;
        movement_speed = new Vector3(2f, 0, 0);
        // Get bounds of screen;
        left_screen = left_screen_boundry.transform.position;
        right_screen = right_screen_boundry.transform.position;

        // Get beak position agian
        beakPosition = beak.transform.position;
        assPosition = ass.transform.position;
        centerOfRotation = Center.transform.position;
        float ranJump = Random.Range(1.0f, 4.0f);
        float ranStart = Random.Range(1.0f, 4.0f);
        InvokeRepeating("jump", ranStart, ranJump);


    }

    void FixedUpdate()
    {
        updatePositions();
        checkBoundries(animate_direction);
        
        animate();
        reanimate();

        

        if (this.transform.position.y < -2.3)
        {
            // add negative gravity 
            applyforce(new Vector3(0,gravity,0))  ;
            //transform.position = new Vector3(transform.position.x,- 2.3f, transform.position.y);
            if (j == true) // if jump, pushes the player model slightly up to allow for the verlet to do its thing
            {
                this.transform.position = new Vector3(transform.position.x, -2.2f, transform.position.y);
            }
            else // if exactly at ground, then stick to the ground. 
            {
                this.transform.position = new Vector3(transform.position.x, -2.3f, transform.position.y);
            }
        }
        else
        {
            addGravity();
        }


        Hitme();
        chickenVerlet();

    }

    void updatePositions()
    {
        left_screen = left_screen_boundry.transform.position;
        right_screen = right_screen_boundry.transform.position;

        // Get beak position agian
        beakPosition = beak.transform.position;
        assPosition = ass.transform.position;
        centerOfRotation = Center.transform.position;
    }

    public void setAnimateLeft()
    {

        animate_direction = "left";
    }

    public void setAnimateRight()
    {
        animate_direction = "right";
    }

    // animate the movement of chicken
    public void animate()
    {

        TurkeyPosition = this.transform.position;
        if (animate_direction == "left")
        {
            if (!hit)
            {
                this.TurkeyPosition = this.TurkeyPosition - this.movement_speed * Time.deltaTime;
                this.transform.position = (this.TurkeyPosition); // use delta time for nice movemement 
            }
        }

        if (animate_direction == "right")
        {
            if (!hit)
            {
                this.TurkeyPosition = this.TurkeyPosition + this.movement_speed * Time.deltaTime;
                this.transform.position = (this.TurkeyPosition); // use delta time for nice movemement 
            }

        }

     
        if(leg1.transform.rotation.z >0.5)
        {
            //print(leg1.transform.rotation.z);
            leg1rotaiton = "left";
        }

        if(leg1.transform.rotation.z < -0.3)
        {
            leg1rotaiton = "right";
        }

        // Leg1 animate
        if (leg1rotaiton == "right")
        {
            leg1.transform.RotateAround(centerOfRotation, Vector3.forward, 100 * Time.deltaTime);
        }
        else
        {
            leg1.transform.RotateAround(centerOfRotation, -Vector3.forward, 100 * Time.deltaTime);
        }

        // Leg2

        if (leg2.transform.rotation.z > 0.3)
        {
            //print(leg2.transform.rotation.z);
            leg2rotation = "left";
        }

        if (leg2.transform.rotation.z < -0.5)
        {
            leg2rotation = "right";
        }

        if (leg2rotation == "right")
        {
            leg2.transform.RotateAround(centerOfRotation, Vector3.forward, 100 * Time.deltaTime);
        }
        else
        {
            leg2.transform.RotateAround(centerOfRotation, -Vector3.forward, 100 * Time.deltaTime);
        }

 
       // leg2.transform.RotateAround(centerOfRotation, -Vector3.forward, 20 * Time.deltaTime);
        // random time jump and no animation 
    }

    // Responsible for semi automated movement
    void checkBoundries(string side)
    {

        if (side == "left")
        {
          //  print(beakPosition.x);
            float d1x = left_screen.x - beakPosition.x;
            float d2x = right_screen.x - assPosition.x;

            if (d1x > 0.0f && d2x > 0.0f)
            {
                // print("d1x : " + d1x + " d2x : " + d2x);
                //print("peak collide");
                setAnimateRight();
            }

        }

        if (side == "right")
        {
            float d1x = left_screen.x - beakPosition.x;
            float d2x = right_screen.x - assPosition.x;


            if (d2x < 0.0f)
            {
                //print("collider rightt");
                setAnimateLeft();
            }

        }

    }

    // unused, adding contraint solution interfered with other physics, decided to make it more rigid

    void solveConstraints()
    {
        for (int I = 0; I < all_edges.Length; I++)
        {
            Edge E = all_edges[I];
            Vector3 V1V2 = E.PositionB() - E.PositionA();



            //Calculate the current distance
            float V1V2Length = Vector3.Distance(E.PositionA(), E.PositionB());

            //Calculate the difference from the original length
           
            float Diff = V1V2Length - E.distance;

            V1V2.Normalize();

            //Push both vertices apart by half of the difference respectively 

            //so the distance between them equals the original length

            Vector3 newA = E.PositionA() + V1V2 * Diff * 0.5f ;
            Vector3 newB = E.PositionB() - V1V2 * Diff * 0.5f;

            E.Vertex_A.transform.position = newA;
            E.Vertex_B.transform.position = newB;

        }

    }

   
    // This is the collision detection for the ball and chicken, if chicken hits the ball, ball dissappears and the force of it is added to the chicken 

    void Hitme()
    {
        if (cannon.CannonBalls.Count != 0)
        {
            // Checking for collisions with the ground 
            for (int i = 0; i < cannon.CannonBalls.Count; i++)
            {
                foreach (Edge e in all_edges)
                {
                        if (EdgeToBoxCollision(e.Vertex_A, e.Vertex_B, cannon.CannonBalls[i].GetComponent<CustomCollider>().NW, cannon.CannonBalls[i].GetComponent<CustomCollider>().SW))
                        {
                        addballforce(cannon.CannonBalls[i].GetComponent<CustomRigidBody>());
                        GameObject o = cannon.CannonBalls[i];
                        hit = true;
                            cannon.CannonBalls.Remove(o);
                            Destroy(o);
                            break;
                        }
                        else if (EdgeToBoxCollision(e.Vertex_A, e.Vertex_B, cannon.CannonBalls[i].GetComponent<CustomCollider>().NW, cannon.CannonBalls[i].GetComponent<CustomCollider>().NE))
                        {
                        addballforce(cannon.CannonBalls[i].GetComponent<CustomRigidBody>());
                        GameObject o = cannon.CannonBalls[i];
                        hit = true;
                        cannon.CannonBalls.Remove(o);
                            Destroy(o);
                            break;
                        }

                        else if (EdgeToBoxCollision(e.Vertex_A, e.Vertex_B, cannon.CannonBalls[i].GetComponent<CustomCollider>().NE, cannon.CannonBalls[i].GetComponent<CustomCollider>().SE))
                        {
                        hit = true;
                        addballforce(cannon.CannonBalls[i].GetComponent<CustomRigidBody>());
                        GameObject o = cannon.CannonBalls[i];
                            cannon.CannonBalls.Remove(o);
                            Destroy(o);
                            break;
                        }

                        else if (EdgeToBoxCollision(e.Vertex_A, e.Vertex_B, cannon.CannonBalls[i].GetComponent<CustomCollider>().SW, cannon.CannonBalls[i].GetComponent<CustomCollider>().SE))
                        {
                        hit = true;
                        addballforce(cannon.CannonBalls[i].GetComponent<CustomRigidBody>());
                            GameObject o = cannon.CannonBalls[i];
                            cannon.CannonBalls.Remove(o);
                            Destroy(o);
                            break;
                        }
                }
            }

        }
    }
    
    // To detect if the edges of the chicken intersect with any of the box edges on the cannon
    bool EdgeToBoxCollision(GameObject aO, GameObject bO, Vector2 c, Vector2 d)
    {

        Vector3 a = aO.transform.position;
        Vector3 b = bO.transform.position;

        float denominator = ((b.x - a.x) * (d.y - c.y)) - ((b.y - a.y) * (d.x - c.x));
        float numerator1 = ((a.y - c.y) * (d.x - c.x)) - ((a.x - c.x) * (d.y - c.y));
        float numerator2 = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));

        // Detect coincident lines (has a problem, read below)
        if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

        float r = numerator1 / denominator;
        float s = numerator2 / denominator;

        return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
    }

    void jump()
    {
        bool j = true;
        this.applyforce(new Vector3(0, 28f, 0));
        print("jump up ");
    }

    public void addGravity()
    {
        // --- Act gravity -- // 

        applyforce( new Vector3(0,-gravity,0));
    }


    // Add reaction the the turkey on the ball. increase the newforcey to see better results
    public void addballforce(CustomRigidBody c)
    {
        float newforcex = c.forcex;
        float newforcey = c.forcey;
        newforcex = newforcex * 5f; // lower the x
        newforcey = newforcey * 5f; ;

        float f = (c.oldposition.y / (c.transform.position.y + c.oldposition.y));

        // if falling from above 
        // if (c.transform.position.y - c.oldposition.y <=0)
        // {
        //applyforce(new Vector3(-newforcex, -newforcey, 0));
        //   }


        // if (c.transform.position.y - c.oldposition.y > 0)
        // {

        acc = new Vector3(acc.x, 0, 0); 
            applyforce(new Vector3(-newforcex,- newforcey, 0));
       // }

    }

    // reaction is - force 
    public void applyforce(Vector3 force)
    {
        this.acc += force;
    }

    // This is both the vertical and horizontal verlet, on application of force, this updates. 
    public void chickenVerlet()
    {
        Vector3 Temp = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        float oldx = this.transform.position.x;
        float oldz= this.transform.position.z;
        // The 0.5 is to only lower the gravity, can be removed
        this.transform.position = new Vector3(oldx, this.transform.position.y, oldz) + (new Vector3(oldx, this.transform.position.y, 0) - new Vector3(oldx, oldposition.y, 0)) + 0.5f*acc * Time.deltaTime * Time.deltaTime;

        this.oldposition = Temp;
    }


    // to make sure the chickens don't run out of bounds
    public void reanimate()
    {
        //print(this.transform.position.x - oldposition.x);

        if((this.transform.position.x - oldposition.x)<0f && animate_direction == "right") 
        {
            this.transform.position = new Vector3(oldposition.x+0.1f, this.transform.position.y, this.transform.position.z);
            
            setAnimateRight();
        }
        if ((this.transform.position.x - oldposition.x) > 0f && animate_direction == "left")
        {
            this.transform.position = new Vector3(oldposition.x-0.1f, this.transform.position.y, this.transform.position.z);
            setAnimateRight();
           
        }


    }
}
