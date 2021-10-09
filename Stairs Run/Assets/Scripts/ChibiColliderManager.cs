using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiColliderManager : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Material playerMaterial;
    private Color playerDefaultColor;
    private float _howManySecondsPlayersTriggerStayedInsideObstacle;

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

        playerRigidbody.velocity = new Vector3(1.5f, 6, 0);
        playerMaterial.color = new Color(1f, 0.3f, 0.3f);

        yield return new WaitForSeconds(0.4f);

        playerRigidbody.velocity = new Vector3(-1.5f, 2, 0);
        playerMaterial.color = playerDefaultColor;
    }

    private void CheckForInput()
    {
        if(Input.GetKey(KeyCode.Space) || Input.touchCount > 0)
        {
            StopCoroutine(PushPlayerBackAndDropStairs());
            playerMaterial.color = playerDefaultColor;

            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            
        }
    }
}
