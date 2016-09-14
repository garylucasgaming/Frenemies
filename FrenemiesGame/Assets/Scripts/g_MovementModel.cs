using UnityEngine;
using System.Collections;

public class g_MovementModel : MonoBehaviour {

    //public Initializers
    public float speed;   // speed of game object



    //private Initializers
    private Vector3 movementDirection; //direction object is moving
    private Vector3 facingDirection; //direction object is facing

    //                        0        1        2        3        4       5        6      7                       
    private enum Direction {North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }
    
    private g_Character character; //thiis objects character script
    private Rigidbody2D rigidBody; // rigidbody of this object

    //Sets components to this.component
    void Awake(){
        character = GetComponent<g_Character>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        UpdateDirection();
        
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

   

     void UpdateDirection()
    {
        //TODO: arrows == controller right stick.  change direction
    }

   void  UpdateMovement()
    {
        //TODO: wasd == left stick. tranform
    }

}
