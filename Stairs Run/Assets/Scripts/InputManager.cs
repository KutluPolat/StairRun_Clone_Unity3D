using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        VerticalMovementControllerForUnityEditor();
#elif PLATFORM_ANDROID
        VerticalMovementControllerForAndroid();
#endif
    }

    private void VerticalMovementControllerForUnityEditor()
    {
        if (Input.GetKeyDown(KeyCode.W))
            GameManager.motionController._isGameStarted = true;

        if (GameManager.backpack.stairsInBackpackCounter == 0)
        {
            GameManager.motionController.FallDown();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.motionController.CloseGravityAndResetVelocity();
            GameManager.motionController.SetStairSpawnPositionUnderThePlayer();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            GameManager.motionController.ClimbLadder();
            GameManager.motionController.PlaceStairs();
        }

        if (Input.GetKeyUp(KeyCode.Space))
            GameManager.motionController.FallDown();
    }

    private void VerticalMovementControllerForAndroid()
    {
        if (Input.touchCount > 0)
        {
            GameManager.motionController._isGameStarted = true;

            if (GameManager.backpack.stairsInBackpackCounter == 0)
            {
                GameManager.motionController.FallDown();
                return;
            }

            if (Input.touches[0].phase == TouchPhase.Began)
            {
                GameManager.motionController.CloseGravityAndResetVelocity();
                GameManager.motionController.SetStairSpawnPositionUnderThePlayer();
            }

            GameManager.motionController.PlaceStairs();
            GameManager.motionController.ClimbLadder(); 
            

            if (Input.touches[0].phase == TouchPhase.Ended)
                GameManager.motionController.FallDown();
        }
    }
}
