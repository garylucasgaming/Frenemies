using UnityEngine;
using System.Collections;

public class g_InfoBuddyController : MonoBehaviour {

    //Public members

    //private members
    private g_MovementModel movement;



    void Awake()
    {
        movement = GetComponent<g_MovementModel>();

    }

    void Start()
    {
        SetDirection(new Vector2(0, -1));
    }

    void Update()
    {
        UpdateDirection();
       
    }

    //send data to movement listener
    void SetDirection(Vector2 direction)
    {
        if (movement == null)
        {
            return;
        }
        movement.SetDirection(direction);

    }

   


    //update direction
    void UpdateDirection()
    {
        Vector2 newDirection = Vector2.zero;

        if (Input.GetAxisRaw("RightHandHorizontal") < 0)
        {
            newDirection.x = -1;
        }

        if (Input.GetAxisRaw("RightHandHorizontal") > 0)
        {
            newDirection.x = 1;
        }

        if (Input.GetAxisRaw("RightHandVertical") < 0)
        {
            newDirection.y = -1;
        }

        if (Input.GetAxisRaw("RightHandVertical") > 0)
        {
            newDirection.y = 1;
           
        }

        SetDirection(newDirection);

    }

  

}


