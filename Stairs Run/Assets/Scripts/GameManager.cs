using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static MotionControlClass motionController;
    public static BackpackClass backpack;
    public float playerHorizontalSpeed = 4f, playerVerticalSpeed = 4f;

    public Rigidbody playerRigidbody;
    public Material playerMaterial;
    private readonly Color playerDefaultColor = new Color(0.25f, 0.25f, 1);
    private Coroutine pushPlayerBackAndDropStairsCoroutine;
    private bool _isInputInsideThisScriptDisabled;

    public TextMeshProUGUI txt_GameEnded;
    private void Start() 
    {
        motionController = new MotionControlClass(playerVerticalSpeed, playerHorizontalSpeed);
        backpack = new BackpackClass();

        playerMaterial.color = playerDefaultColor;
    }

    private void Update() 
    {
        motionController.AnimationController();
        motionController.MovePlayerForward();

        CheckForInputToStopCoroutine();
    } 


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack"))
        {
            Destroy(other.gameObject);
            backpack.AddStairToBackpack();
        }

        if (other.CompareTag("Obstacle"))
        {
            if(backpack.stairsInBackpackCounter == 0)
            {
                txt_GameEnded.text = "FAILED";
                motionController.DisableAnimator();
                _isInputInsideThisScriptDisabled = true;
                motionController._isMovementDisabled = true;
                GameObject.Find("Chibi").GetComponent<InputManager>().enabled = false; // Disabling input manager will block any input about movement.
                return;
            }
            // In order to stop this coroutine, I have to use a Coroutine variable like pushPlayerBackAndDropStairsCoroutine.
            pushPlayerBackAndDropStairsCoroutine = StartCoroutine(PushPlayerBackAndDropStairs());
        }

        if (other.CompareTag("EndOfThePlatform"))
        {
            if(backpack.stairsInBackpackCounter <= 10)
            {
                LevelPassed(other.gameObject);
                return;
            }

            GameObject.Find("Chibi").GetComponent<InputManager>().enabled = false; // Disabling input manager will block any input about movement.
            motionController.CloseGravityAndResetVelocity();
            motionController.SetStairSpawnPositionUnderThePlayer();
            InvokeRepeating("PlaceStairsAsFarAsPlayerCan", 0f, 0.02f);
        }

        if (other.CompareTag("FinishLine")) 
        {
            LevelPassed(other.gameObject);
        }
    }

    private IEnumerator PushPlayerBackAndDropStairs()
    {
        backpack.DropStairsFromBackPack();

        yield return new WaitForSeconds(0.1f);

        // Push player back, change model color to red
        playerRigidbody.velocity = new Vector3(3f, 6, 0);
        playerMaterial.color = new Color(1f, 0.3f, 0.3f);
        motionController._isMovementDisabled = true;

        yield return new WaitForSeconds(0.5f);

        // A little help for player to start to move, and change model color to default
        playerRigidbody.velocity = new Vector3(-2f, 0, 0); // A little help for the player.
        playerMaterial.color = playerDefaultColor;
        motionController._isMovementDisabled = false;

        // I want stairs to start to spawn under the player. 
        // And spawning algorithm checks the latest stair and spawns a new stair right next to the latest spawned one. 
        // Also, we pushed back the player so now, the player is farther back than the latest spawned stair. 
        // So we have to reset the stair spawn starting position in order for stairs to start to spawn under the player.
        motionController.SetStairSpawnPositionUnderThePlayer();
    }

    private void CheckForInputToStopCoroutine()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space) && !_isInputInsideThisScriptDisabled)
            StopCoroutine_SetPlayerColorToDefault_EnableMovement();
#elif PLATFORM_ANDROID
        if (Input.touchCount > 0 && !motionController._isInputsDisabled)
            if (Input.touches[0].phase == TouchPhase.Began)
                StopCoroutine_SetPlayerColorToDefault_EnableMovement();
#endif
    }
    private void StopCoroutine_SetPlayerColorToDefault_EnableMovement()
    {
        if (pushPlayerBackAndDropStairsCoroutine != null) // If there's any coroutine working inside of pushPlayerBackAndDropStairsCoroutine.
        {
            StopCoroutine(pushPlayerBackAndDropStairsCoroutine);
            playerMaterial.color = playerDefaultColor;
            motionController._isMovementDisabled = false;
        }
    }
    private void PlaceStairsAsFarAsPlayerCan()
    {
        motionController.PlaceStairs();
        motionController.ClimbLadder();

        if(backpack.stairsInBackpackCounter == 0)
            CancelInvoke();
    }
    private void LevelPassed(GameObject other)
    {
        int rewardMultiplier = int.Parse(other.GetComponent<TextMeshPro>().text);
        txt_GameEnded.text = $"Passed. \n Points: {100 * rewardMultiplier}";

        GameObject.Find("Chibi").GetComponent<InputManager>().enabled = false; // Disabling input manager will block any input about movement.
        motionController.TurnChibisFaceTowardsPlayer();
        motionController.FallDown();
        motionController._isWinConditionTrue = true;
        backpack.DestroyBackpack();
        motionController.AnimationController(); // Setting animation to dance because isWinConditionTrue is true
    }

    public void Restart() => SceneManager.LoadScene("Scene_01");
}
