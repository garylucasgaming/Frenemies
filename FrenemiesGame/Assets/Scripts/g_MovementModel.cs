using UnityEngine;
using System.Collections;

public class g_MovementModel : MonoBehaviour {

    //public Initializers
    public float speed;   // speed of game object



    //private Initializers
    private Vector3 movementDirection; //direction object is moving
    private Vector2 receivedDirection; //direction received from controller
    private Vector3 facingDirection; //direction object is facing
    private Rigidbody2D rigidBody; // rigidbody of this object

    //Sets components to this.component
    void Awake(){
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        UpdateDirection();
        ResetReceievedDirection();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

   

     void UpdateDirection()
    {
        //TODO: arrows == controller right stick.  change direction
        movementDirection = new Vector3(receivedDirection.x, receivedDirection.y, 0);
        if (receivedDirection != Vector2.zero) {
            Vector3 _facingDirection = movementDirection;

            if (_facingDirection.x != 0 && _facingDirection.y != 0)
            {
                if (_facingDirection.x == facingDirection.x)
                {
                    _facingDirection.y = 0;
                }
                else if (_facingDirection.y == facingDirection.y)
                {
                    _facingDirection.x = 0;
                }
                else
                {
                    _facingDirection.x = 0;
                }
            }
           facingDirection = _facingDirection;
           
       }
    }

   

   void  UpdateMovement()
    {
        //TODO: wasd == left stick. tranform

        rigidBody.velocity = movementDirection * speed;
    }

    void ResetReceievedDirection()
    {
        receivedDirection = Vector2.zero;
    }

    public void SetDirection(Vector2 direction) {
        if (direction == Vector2.zero) {
            return;
        }
        receivedDirection = direction;
       
    }

}
