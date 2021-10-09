using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairManager : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        StairPlacementControllerForUnityEditor();
#elif PLATFORM_ANDROID
        StairPlacementControllerForAndroid();
#endif
    }
    private void StairPlacementControllerForAndroid()
    {
        if (Input.touchCount > 0)
        {

        }
    }
    private void StairPlacementControllerForUnityEditor()
    {
        if(StaticVariables.backpack.stairsInBackpackCounter == 0) // If the player doesn't have any stairs, return.
            return;

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StaticVariables.stairSpawnManager.ResetStairSpawnStartingPosition();
        }

        if (Input.GetKey(KeyCode.Space)) 
        {
            StaticVariables.stairSpawnManager.PlaceStairs();
        }
    }
}
