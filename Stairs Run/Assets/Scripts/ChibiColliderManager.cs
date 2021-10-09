using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiColliderManager : MonoBehaviour
{
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
        }
    }
}
