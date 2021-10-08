using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator playerAnimator;
    public Rigidbody playerRigidbody;
    public float playerHorizontalSpeed = 2f, playerVerticalSpeed = 3f;
    private bool _isGameStarted, _isGameEnded;

    private void Update()
    {
        MoveForward();
        VerticalMovementControllerForAndroid();
        VerticalMovementControllerForUnityEditor();
    }
   
    private void VerticalMovementControllerForAndroid()
    {
#if PLATFORM_ANDROID
        if (Input.touchCount > 0)
        {
            _isGameStarted = true;
            Move(0, playerVerticalSpeed * Time.deltaTime);
        }
#endif
    }
    private void VerticalMovementControllerForUnityEditor()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
            _isGameStarted = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Space))
            Move(0, playerVerticalSpeed * Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.Space))
            playerRigidbody.useGravity = true;
#endif
    }
    private void MoveForward() => Move(-playerHorizontalSpeed * Time.deltaTime);
    private void Move(float directionX, float directionY = 0)
    {
        if (_isGameStarted && !_isGameEnded)
        {
            playerAnimator.SetTrigger("Run");
            transform.position = new Vector3(transform.position.x + directionX, transform.position.y + directionY, transform.position.z);
        }  
    }
}
