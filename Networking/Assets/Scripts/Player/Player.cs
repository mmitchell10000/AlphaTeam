
using UnityEngine;
using System.Collections;

//Requires PlayerstateMachine to go along with Player Script
[RequireComponent(typeof(PlayerStateMachine))]
public class Player : MonoBehaviour
{

    public float moveSpeed;
    Camera viewCamera;
    PlayerStateMachine playerState;
 

    void Start()
    {

  
        //Attached to same GameObject as Player script
        playerState = GetComponent<PlayerStateMachine>();

        viewCamera = Camera.main;
    }


    void Update()
    {

            //Movement based on Input of Horizontal and Vertical Axis
            Vector3 playerMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            //normalized gives Vector 3 a magnitude of 1
            Vector3 moveVelocity = playerMovement.normalized * moveSpeed;
            playerState.Move(moveVelocity);

            //Look Input
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            float rayDistance;

            //if ray intersects with groundPlane
            //We get ray length
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                //Debug.DrawLine(ray.origin, point, Color.red);

                playerState.LookAt(point);
            }
        }

    
}
