using UnityEngine;
using System.Collections;

public class g_PlayerController : MonoBehaviour {


    //Public members

    //private members
    private g_MovementModel movement;
    private g_CharacterView view;
    private g_InteractionModel interaction;


    void Awake() {
        movement = GetComponent<g_MovementModel>();
        view = GetComponent<g_CharacterView>();
        interaction = GetComponent<g_InteractionModel>();
    }

    void Start() {
        SetDirection(new Vector2(0, -1));
    }

    void Update()
    {
        UpdateDirection();
        UpdateAction();
        UpdateAttack();
    }

    //send data to movement listener
    void SetDirection(Vector2 direction) {
        if (movement == null) {
            return;
        }
        movement.SetDirection(direction);
        Debug.Log("Sending Direction " + direction);
    }

    //send data to action listener
    void OnActionPressed() {
        if (interaction == null) {
            return;
        }

        interaction.OnInteract();
    }

    //send data to attack listener
    void OnAttackPressed()
    {
        if (interaction == null)
        {
            return;
        }

        interaction.OnAttack();
    }

    //update direction
    void UpdateDirection()
    {
        Vector2 newDirection = Vector2.zero;

        if (Input.GetAxisRaw("LeftHandHorizontal") < 0) {
            newDirection.x = -1;
        }

        if (Input.GetAxisRaw("LeftHandHorizontal") > 0)
        {
            newDirection.x = 1;
        }

        if (Input.GetAxisRaw("LeftHandVertical") < 0)
        {
            newDirection.y = 1;
        }

        if (Input.GetAxisRaw("LeftHandVertical") > 0)
        {
            newDirection.y = -1;
            Debug.Log("DirectionSet" + newDirection);
        }

        SetDirection(newDirection);

    }
    
    //update attack
    void UpdateAttack()
    {
        if (Input.GetButtonDown("Fire1")) {
            OnAttackPressed();
        }
    }

    //update action
    void UpdateAction()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            OnActionPressed();
        }
    }

}
