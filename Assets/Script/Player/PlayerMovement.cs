using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    //Player Attributes
    [Range(0, 200)] public float MouseSensitivity = 100f, CurrentSpeed = 20f;
    [HideInInspector] public Rigidbody rb; [HideInInspector] public CapsuleCollider cc;
    private void Start() {
        //Get All Component
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        //Freeze Rigidbody Rotation
        rb.constraints = RigidbodyConstraints.FreezeRotation;
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
    }
}
