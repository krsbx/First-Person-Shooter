using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    //Player Attributes
    [Range(0, 200)] public float MouseSensitivity = 100f, DefaultSpeed = 20f;
    [Range(0, 200)] public float CurrentSpeed = 20f, RunSpeed = 35f, CrouchSpeed = 15f;
    //Jump Attributes
    [Range(0.2f, 20)] public float DistCheck = 0.2f, GravityForces = 10f, JumpForces = 10f;
    //Special Keys
    public KeyCode Crouch = KeyCode.LeftControl, Run = KeyCode.LeftShift, Jump = KeyCode.Space;
    [HideInInspector] public Rigidbody rb; [HideInInspector] public CapsuleCollider cc;
    //Private Variables
    private Vector3 checkPos; //For Ground Checking
    private float rotationY; //For Mouse Look
    private void Start() {
        //Get All Component
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        //Freeze Rigidbody Rotation
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        //Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    bool IsGrounded(){
        RaycastHit hit;
        //Create a Sphere Cast
        //  Origin in checkPos (under the players)
        //  With size radius/4 with directions under the player
        //  Distance to check = DistCheck
        //  Cast to all layer (-1)
        if(Physics.SphereCast(checkPos, cc.radius/4, Vector3.down, out hit, DistCheck, -1, QueryTriggerInteraction.Ignore))
            return true;
        else
            return false;
    }

    private void Update() {
        //Jumping
        if(Input.GetKeyDown(Jump) && IsGrounded())
            rb.velocity = Vector3.up*JumpForces;
        
        //Crouching
        if(Input.GetKeyDown(Crouch)){
            cc.height /=2;
            CurrentSpeed = CrouchSpeed;
        }else if(Input.GetKeyUp(Crouch)){
            cc.height *= 2;
            CurrentSpeed = DefaultSpeed;
        }

        //Running
        if(Input.GetKeyDown(Run)){
            CurrentSpeed = RunSpeed;
        }else if(Input.GetKeyUp(Run)){
            CurrentSpeed = DefaultSpeed;
        }

        //Set checkPos to the center of the player
        //  Centered X dan Z
        //  The Y Axis => Centered Y -  Half Bounds in Y Axis + DistCheck
        checkPos = new Vector3(cc.bounds.center.x, cc.bounds.center.y-cc.bounds.extents.y+DistCheck, cc.bounds.center.z);
    }

    private void LateUpdate() {
        //Mouse Axis
        float MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        //Movement Axis
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Get All Movement Input
        Vector2 MoveX = new Vector2(horizontal*transform.right.x, horizontal*transform.right.z);
        Vector2 MoveY = new Vector2(vertical*transform.forward.x, vertical*transform.forward.z);

        //New Velocity
        var velocity = (MoveX+MoveY).normalized*Time.deltaTime*10;
        
        //Set Player Movement
        rb.velocity = new Vector3(velocity.x*CurrentSpeed, rb.velocity.y, velocity.y*CurrentSpeed);

        //Ground Forces
        if(rb.velocity.y < -2f)
            rb.velocity = Vector3.down*GravityForces;

        //Player Camera
        rotationY -= MouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); //Limit the player view
        
        //Up and Down
        Camera.main.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);

        //Left and Right
        rb.rotation *= Quaternion.Euler(0, MouseX, 0);
    }
}
