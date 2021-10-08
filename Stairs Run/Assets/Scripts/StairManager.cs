using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairManager : MonoBehaviour
{
    public GameObject miniStairPrefab, backpack, stairPrefab;
    public BoxCollider boxColliderOfPlayer;
    public float YMarginOfStairs = 0.2f;

    private GameObject[,] _stairsInBackpack = new GameObject[50, 3];
    private Bounds _boundsOfLatestSpawnedStair;

    private void Update()
    {
        StairPlacementControllerForAndroid();
        StairPlacementControllerForUnityEditor();
    }
    private void StairPlacementControllerForAndroid()
    {
#if PLATFORM_ANDROID
        if (Input.touchCount > 0)
        {
        }
#endif
    }
    private void StairPlacementControllerForUnityEditor()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _boundsOfLatestSpawnedStair.center = boxColliderOfPlayer.bounds.center;
            _boundsOfLatestSpawnedStair.min = boxColliderOfPlayer.bounds.max;
            _boundsOfLatestSpawnedStair.max = boxColliderOfPlayer.bounds.min;
        }
        if (Input.GetKey(KeyCode.Space))
        { 
            if (_boundsOfLatestSpawnedStair == null || boxColliderOfPlayer.bounds.max.x < _boundsOfLatestSpawnedStair.min.x)
            {
                DeleteStairFromBackpack();

                var spawnPosition = new Vector3(
                    boxColliderOfPlayer.bounds.center.x,
                    boxColliderOfPlayer.bounds.min.y,
                    boxColliderOfPlayer.bounds.center.z);
                
                _boundsOfLatestSpawnedStair = Instantiate(stairPrefab, spawnPosition, stairPrefab.transform.rotation).GetComponent<BoxCollider>().bounds;
            }
        }
#endif
    }
    /// <summary>
    /// <para>Checks _stairsInBackpack array from first element to last.<br/>
    /// Finds closest null element to first element, instantiates a stair in to it.</para>
    /// Algorithm;<br/>
    /// 1-) Instantiates a miniStairPrefab on Vector3.zero with default rotation, and assigns it in to relevant _stairsInBackpack array.<br/>
    /// 2-) Calculates local Z and Y axis values that should miniStairPrefab have, and assigns it to a variable.<br/>
    /// 3-) Sets the local rotation value that should miniStairPrefab have, and assigns it to a variable.<br/>
    /// 4-) Changes the localPosition and localRotation values of the relevant array by using these variables.
    /// </summary>
    private void AddStairToBackpack()
    {
        for(int vertical = 0; vertical < 50; vertical++) // Vertical elements in _stairsInBackpack, first parameter.
        {
            for(int horizontal = 0; horizontal < 3; horizontal++) // Horizontal elements in _stairsInBackpack, second parameter.
            {
                if(_stairsInBackpack[vertical,horizontal] == null)
                {
                    _stairsInBackpack[vertical, horizontal] = Instantiate(miniStairPrefab, Vector3.zero, Quaternion.identity, backpack.transform);

                    float localZValue = horizontal == 0 ? -0.1f : horizontal == 1 ? 0 : 0.1f;
                    float localYValue = vertical == 0 ? -0.03f : vertical == 1 ? 0 : 0.03f * (vertical - 1);
                    var spawnPosition = new Vector3(0, localYValue, localZValue);
                    var rotation = Quaternion.Euler(0, 90, 0);

                    _stairsInBackpack[vertical, horizontal].transform.localPosition = spawnPosition;
                    _stairsInBackpack[vertical, horizontal].transform.localRotation = rotation;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// <para>Checks _stairsInBackpack array from last element to first.<br/>
    /// Finds closest valid element to last element, and destroys it.</para>
    /// </summary>
    private void DeleteStairFromBackpack()
    {
        for (int localY = 49; localY >= 0; localY--)
        {
            for (int localZ = 2; localZ >= 0; localZ--)
            {
                if (_stairsInBackpack[localY, localZ] != null)
                {
                    Destroy(_stairsInBackpack[localY, localZ]);
                    return;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Stack")
        {
            Destroy(collider.gameObject);
            AddStairToBackpack();
        }
    }
}
