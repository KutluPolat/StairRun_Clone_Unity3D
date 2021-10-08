using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator playerAnimator;
    public Rigidbody playerRigidbody;
    public float playerHorizontalSpeed = 2f, playerVerticalSpeed = 3f;

    private void Update()
    {
        AnimationController();
        MoveForward();

#if UNITY_EDITOR
        VerticalMovementControllerForUnityEditor();
#elif PLATFORM_ANDROID
        VerticalMovementControllerForAndroid();
#endif
    }
   
    private void VerticalMovementControllerForAndroid()
    {
        if (Input.touchCount > 0)
        {
            StaticVariables._isGameStarted = true;
            Move(0, playerVerticalSpeed * Time.deltaTime);
        }
    }
    private void VerticalMovementControllerForUnityEditor()
    {
        if (Input.GetKeyDown(KeyCode.W))
            StaticVariables._isGameStarted = true;

        if(StaticVariables._stairsInBackpackCounter == 0 && StaticVariables._isGameStarted)
        {
            playerRigidbody.useGravity = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Space))
            Move(0, playerVerticalSpeed * Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerRigidbody.useGravity = true;
        } 
    }
    private void MoveForward() => Move(-playerHorizontalSpeed * Time.deltaTime);
    private void Move(float directionX, float directionY = 0)
    {
        if (StaticVariables._isGameStarted && !StaticVariables._isGameEnded)
        {
            transform.position = new Vector3(transform.position.x + directionX, transform.position.y + directionY, transform.position.z);
        }
    }
    private void AnimationController()
    {
        if(playerRigidbody.velocity.magnitude > 0.5f && StaticVariables._isGameStarted)
            playerAnimator.SetTrigger("Fall");

        else if(StaticVariables._isGameStarted)
            playerAnimator.SetTrigger("Run");
    }
}
