using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerHeadBob : MonoBehaviour
{
    PlayerMovement movement; //Used for player data
    private void Start() {
        //Get All Component
        movement = GetComponent<PlayerMovement>();
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
