using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairManager : MonoBehaviour
{
    public static Backpack backpack;
    private void Start()
    {
        backpack = new Backpack();
    }
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
        if(StaticVariables._stairsInBackpackCounter == 0) // If the player doesn't have any stairs, return.
            return;

        // Resetting _boundsOfLatestSpawnedStair according to the player's position when every time player starts the action.
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            backpack.ResetBoundsOfLatestSpawnedStair();
        }

        if (Input.GetKey(KeyCode.Space)) 
        {
            backpack.PlaceStairs();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Stack")
        {
            Destroy(collider.gameObject);
            backpack.AddStairToBackpack();
        }
        if(collider.tag == "Obstacle")
        {
            backpack.Stumble();
        }
    }
}
