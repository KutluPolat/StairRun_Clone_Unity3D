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
        if (StaticVariables.backpack.stairsInBackpackCounter == 0 && StaticVariables._isGameStarted)
        {
            FallDown();
            return;
        }

        if (Input.touchCount > 0)
        {
            StaticVariables._isGameStarted = true;

            if (Input.touches[0].phase == TouchPhase.Began)
            {
                StartToClimbLadder();
                StaticVariables.stairSpawnManager.ResetStairSpawnStartingPosition();
            }

            StaticVariables.stairSpawnManager.PlaceStairs();
            ClimbLadder(); 
            

            if (Input.touches[0].phase == TouchPhase.Canceled)
                FallDown();
        }
    }
    private void VerticalMovementControllerForUnityEditor()
    {
        if (Input.GetKeyDown(KeyCode.W))
            StaticVariables._isGameStarted = true;

        if (StaticVariables.backpack.stairsInBackpackCounter == 0 && StaticVariables._isGameStarted)
        {
            FallDown();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartToClimbLadder();
            StaticVariables.stairSpawnManager.ResetStairSpawnStartingPosition();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ClimbLadder();
            StaticVariables.stairSpawnManager.PlaceStairs();
        }

        if (Input.GetKeyUp(KeyCode.Space))
            FallDown();
    }
    private void StartToClimbLadder()
    {
        playerRigidbody.useGravity = false;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }
    private void FallDown()
    {
        // Set useGravity to true so chibi can fall down
        playerRigidbody.useGravity = true;

        // Add rigidbody to placed stairs and destroy them after one second
        foreach (GameObject spawnedStair in GameObject.FindGameObjectsWithTag("SpawnedStair"))
        {
            if (spawnedStair.GetComponent<Rigidbody>() == null)
                spawnedStair.AddComponent<Rigidbody>();

            spawnedStair.GetComponent<BoxCollider>().isTrigger = false; // Closing is trigger so stairs can stop when they hit platform.
            StartCoroutine(DestroyAfterOneSecond(spawnedStair));
        }
    }
    private IEnumerator DestroyAfterOneSecond(GameObject objectThatWillDestroyed)
    {
        yield return new WaitForSeconds(1f);
        Destroy(objectThatWillDestroyed);
    }
    private void ClimbLadder() => Move(0, playerVerticalSpeed * Time.deltaTime);
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
