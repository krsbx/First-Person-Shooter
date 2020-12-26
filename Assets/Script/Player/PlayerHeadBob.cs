using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerHeadBob : MonoBehaviour
{
    [Range(0, 30)] public float DefaultBobSpeed = 10f, BobRunSpeed = 15f, BobCrouchSpeed = 5f, BobCurrentSpeed;
    [Range(0, 1)] public float DefaultBobAmount = 0.1f, RunBobAmount = 0.15f, CrouchBobAmount = 0.08f, CurrentBobAmount;
    PlayerMovement movement; //Used for player data
    private float bobDelay, DefaultPos; //Will be used to changes Camera Local Position
    private void Start() {
        //Get All Component
        movement = GetComponent<PlayerMovement>();
        //Set Head Bob Attributes
        BobCurrentSpeed = DefaultBobSpeed;
        CurrentBobAmount = DefaultBobAmount;
        DefaultPos = localPos.y;
    }

    private void Update() {
        //Crouching
        if(Input.GetKeyDown(Crouch)){
            BobCurrentSpeed = BobCrouchSpeed;
            CurrentBobAmount = CrouchBobAmount;
        }else if(Input.GetKeyUp(Crouch)){
            SetDefaultHeadBob();
        }

        //Running
        if(Input.GetKeyDown(Run)){
            BobCurrentSpeed = BobRunSpeed;
            CurrentBobAmount = RunBobAmount;
        }else if(Input.GetKeyUp(Run)){
            SetDefaultHeadBob();
        }

        //Use Head Bob
        if(UseHeadBob()){
            bobDelay += Time.deltaTime * BobCurrentSpeed;
            localPos = new Vector3(localPos.x, DefaultPos+Mathf.Sin(bobDelay)*CurrentBobAmount, localPos.z);
        }else{
            bobDelay = 0;
            localPos = new Vector3(localPos.x, Mathf.Lerp(localPos.y, DefaultPos, Time.deltaTime*BobCurrentSpeed), localPos.z);
        }
    }

    void SetDefaultHeadBob(){
        BobCurrentSpeed = DefaultBobSpeed;
        CurrentBobAmount = DefaultBobAmount;
    }

    bool UseHeadBob(){
        return (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.z) > 0.1f);
    }

    //Get All Player Movement Key From PlayerMovement
    public KeyCode Run {get{return movement.Run;}}
    public KeyCode Crouch {get{return movement.Crouch;}}
    //Get All Player Component
    public Rigidbody rb {get{return movement.rb;}}
    public Vector3 localPos{
        get{ return Camera.main.transform.localPosition; }
        set{ Camera.main.transform.localPosition = value; }
    }

}
