using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject playerObject;
    public float cameraSpeed = 5f;

    private void LateUpdate()
    {
        var targetPosition = new Vector3(playerObject.transform.position.x + 5f, playerObject.transform.position.y + 4f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    }
}
