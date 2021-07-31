using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerRotator : MonoBehaviour
{

    public Camera mainCamera;
    /*Vector3 moveInput, moveVelocity;*/
    [SerializeField] float moveSpeed;

    [HideInInspector] public Vector3 PointPos;    
    void Update()
    {
/*        moveInput = new Vector3(Input.GetAxisRaw("Horizontal")* moveSpeed*Time.deltaTime, 0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
        moveVelocity = moveInput * moveSpeed;*/

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(-Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            Vector3 look = new Vector3(pointToLook.x * moveSpeed, transform.position.y, pointToLook.z * moveSpeed);
            
            Vector3 look1 = 2 * transform.position - look;
            PointPos = look1;
            transform.LookAt(look1);
        }
    }



}
