using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiColliderManager : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Material playerMaterial;
    private Color playerDefaultColor;

    private void Start() => playerMaterial.color = playerDefaultColor = new Color(0.25f, 0.25f, 1);

    private void Update() => PlayerColorController();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Stack")
        {
            Destroy(collider.gameObject);
            StaticVariables.backpack.AddStairToBackpack();
        }

        if (collider.tag == "Obstacle")
        {
            StaticVariables.backpack.DropStairsFromBackPack();
            StartCoroutine(PushPlayerBack());
        }
    }
    private IEnumerator PushPlayerBack()
    {
        playerRigidbody.velocity = new Vector3(1.5f, 6, 0);
        playerMaterial.color = new Color(1f, 0.3f, 0.3f);

        yield return new WaitForSeconds(0.5f);

        // If player didn't already started to place & climb ladder, give a little help to player.
        // If player did started, playerRigidbody.velocity.magnitude will equal to be 0.
        if (playerRigidbody.velocity.magnitude != 0) 
            playerRigidbody.velocity = new Vector3(-1.5f, 2, 0);

        playerMaterial.color = playerDefaultColor;
    }

    private void PlayerColorController()
    {
        if (playerRigidbody.velocity.y > 2f && StaticVariables._isGameStarted)
            playerMaterial.color = new Color(1f, 0.3f, 0.3f);

        else if (StaticVariables._isGameStarted)
            playerMaterial.color = playerDefaultColor;
    }
}
