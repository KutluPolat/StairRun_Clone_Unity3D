using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiColliderManager : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Material playerMaterial;
    private Color playerDefaultColor;

    private void Start() => playerMaterial.color = playerDefaultColor = new Color(0.25f, 0.25f, 1);

    private void Update() => CheckForInput();


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stack")
        {
            Destroy(other.gameObject);
            StaticVariables.backpack.AddStairToBackpack();
        }

        if (other.tag == "Obstacle")
        {
            StartCoroutine(PushPlayerBackAndDropStairs());
        }
    }

    private IEnumerator PushPlayerBackAndDropStairs()
    {
        StaticVariables.backpack.DropStairsFromBackPack();

        yield return new WaitForSeconds(0.1f);

        playerRigidbody.velocity = new Vector3(3f, 6, 0);
        playerMaterial.color = new Color(1f, 0.3f, 0.3f);
        StaticVariables._isMovementDisabled = true;

        yield return new WaitForSeconds(0.5f);

        playerRigidbody.velocity = new Vector3(-2f, 0, 0);
        playerMaterial.color = playerDefaultColor;
        StaticVariables._isMovementDisabled = false;

        // I want stairs to start to spawn under the player. 
        // And spawning algorithm checks the latest stair and spawns a new stair right next to the latest spawned one. 
        // Also, we pushed back the player so now, the player is farther back than the latest spawned stair. 
        // So we have to reset the stair spawn starting position in order for stairs to start to spawn under the player.
        StaticVariables.stairSpawnManager.ResetStairSpawnStartingPosition();
    }

    private void CheckForInput()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
            ResetEverything();
#elif PLATFORM_ANDROID
        if (Input.touchCount > 0)
            if (Input.touches[0].phase == TouchPhase.Began)
                ResetEverything();
#endif
    }
    private void ResetEverything()
    {
        StopCoroutine(PushPlayerBackAndDropStairs());
        playerMaterial.color = playerDefaultColor;
        StaticVariables._isMovementDisabled = false;
    }
}
