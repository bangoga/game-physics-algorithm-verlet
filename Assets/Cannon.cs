using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    public GameObject CannonBall_Prefab;
    GameObject CannanBall;
    public float rotation = 20.0F;

    public List<GameObject> CannonBalls = new List<GameObject>();
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            shootCannon();
        }

        if (Input.GetKey(KeyCode.W) && this.transform.rotation.x > -0.48)
        {
            print(this.transform.eulerAngles.x);
            this.transform.Rotate(-Vector3.right * rotation);
        }

        if (Input.GetKey(KeyCode.S) && this.transform.rotation.x < -0.02)
        {
            this.transform.Rotate(Vector3.right * rotation);
        }
    }

    // Shooting mechanic
    // Provide the projectile angle and force of spawn 

    void shootCannon()
    {
        Vector2 theta = this.transform.rotation.eulerAngles; // [Theta x is angle of shooting] 
        Vector3 CannonPosition = GameObject.FindGameObjectWithTag("CannonSpawn").transform.position; // Position of nuzzle 
        CannanBall = Instantiate(CannonBall_Prefab, new Vector3(CannonPosition.x , CannonPosition.y,-0.2f),Quaternion.identity);

        CannonBalls.Add(CannanBall);
        float force = 26f; // change it to 15 later 
        float angle = this.transform.eulerAngles.x;

        // ------------------------------------[Handling limits between 45 degrees only ]--------------------- // 
        // let all angles be positive only 
        print("Angle is " + angle);

        angle = (-angle+360); 
        angle = angle * (Mathf.PI / 180);
        CannanBall.GetComponent<CustomRigidBody>().addforce(force, force,angle);
    }
}
