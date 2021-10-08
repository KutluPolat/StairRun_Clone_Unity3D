using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack
{
    private GameObject[,] _stairsInBackpack = new GameObject[50, 3];
    private GameObject _miniStairPrefab, _backpack, _stairPrefab;
    private float _yMarginOfStairs = 0.2f;
    
    private BoxCollider _boxColliderOfPlayer;
    private Bounds _boundsOfLatestSpawnedStair;

    public Backpack()
    {
        _miniStairPrefab = Resources.Load<GameObject>("MiniStack");
        _stairPrefab = Resources.Load<GameObject>("Stair");
        _backpack = GameObject.Find("BackPack");
        _boxColliderOfPlayer = GameObject.Find("Chibi").GetComponent<BoxCollider>();
    }

    /// <summary><para>Checks _stairsInBackpack array from first element to last.<br/>
    /// Finds closest null element to first element, instantiates a stair in to it. </para>
    /// Algorithm;<br/>
    /// 1-) Instantiates a miniStairPrefab on Vector3.zero with default rotation, and assigns it in to relevant _stairsInBackpack array.<br/>
    /// 2-) Calculates local Z and Y axis values that should miniStairPrefab have, and assigns it to a variable.<br/>
    /// 3-) Sets the local rotation value that should miniStairPrefab have, and assigns it to a variable.<br/>
    /// 4-) Changes the localPosition and localRotation values of the relevant array by using these variables.
    /// </summary>
    public void AddStairToBackpack()
    {
        for (int vertical = 0; vertical < 50; vertical++) // Vertical elements in _stairsInBackpack, first parameter.
        {
            for (int horizontal = 0; horizontal < 3; horizontal++) // Horizontal elements in _stairsInBackpack, second parameter.
            {
                if (_stairsInBackpack[vertical, horizontal] == null)
                {
                    _stairsInBackpack[vertical, horizontal] = MonoBehaviour.Instantiate(_miniStairPrefab, Vector3.zero, Quaternion.identity, _backpack.transform);

                    float localZValue = horizontal == 0 ? -0.1f : horizontal == 1 ? 0 : 0.1f;
                    float localYValue = vertical == 0 ? -0.05f : vertical == 1 ? 0 : 0.05f * (vertical - 1);
                    var spawnPosition = new Vector3(0, localYValue, localZValue);
                    var rotation = Quaternion.Euler(0, 90, 0);

                    _stairsInBackpack[vertical, horizontal].transform.localPosition = spawnPosition;
                    _stairsInBackpack[vertical, horizontal].transform.localRotation = rotation;

                    StaticVariables._stairsInBackpackCounter++;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// <para>Checks _stairsInBackpack array from last element to first.<br/>
    /// Finds closest valid element to last element, and destroys it.</para>
    /// </summary>
    public void DeleteStairFromBackpack()
    {
        for (int localY = 49; localY >= 0; localY--)
        {
            for (int localZ = 2; localZ >= 0; localZ--)
            {
                if (_stairsInBackpack[localY, localZ] != null)
                {
                    MonoBehaviour.Destroy(_stairsInBackpack[localY, localZ]);
                    StaticVariables._stairsInBackpackCounter--;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Push player back and drop 10 stairs.
    /// </summary>
    public void Stumble()
    {
        int droppedStairCounter = 0;
        for (int localY = 49; localY >= 0; localY--)
        {
            for (int localZ = 2; localZ >= 0; localZ--)
            {
                // if there's any valid object without rigidbody;
                if (_stairsInBackpack[localY, localZ] != null)
                {
                    var temporalObject = new GameObject();
                    temporalObject = MonoBehaviour.Instantiate( // Instantiate a temporary object and destroy the original one.
                        _stairsInBackpack[localY, localZ],
                        _stairsInBackpack[localY, localZ].transform.position,
                        _stairsInBackpack[localY, localZ].transform.rotation);

                    MonoBehaviour.Destroy(_stairsInBackpack[localY, localZ]);
                    StaticVariables._stairsInBackpackCounter--;

                    temporalObject.AddComponent<Rigidbody>()
                        .velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 5f), Random.Range(-1f, 1f));

                    droppedStairCounter++;
                    if (droppedStairCounter == 10)
                        return;
                }
            }
        }
    }
    public void ResetBoundsOfLatestSpawnedStair()
    {
        _boundsOfLatestSpawnedStair.center = _boxColliderOfPlayer.bounds.center;
        _boundsOfLatestSpawnedStair.min = _boxColliderOfPlayer.bounds.max;
        _boundsOfLatestSpawnedStair.max = _boxColliderOfPlayer.bounds.min;
    }

    public void PlaceStairs()
    {
        if (_boundsOfLatestSpawnedStair == null || _boxColliderOfPlayer.bounds.max.x < _boundsOfLatestSpawnedStair.min.x)
        {
            DeleteStairFromBackpack();

            var spawnPosition = new Vector3(
                _boxColliderOfPlayer.bounds.center.x,
                _boxColliderOfPlayer.bounds.min.y,
                _boxColliderOfPlayer.bounds.center.z);

            _boundsOfLatestSpawnedStair = MonoBehaviour.Instantiate(_stairPrefab, spawnPosition, _stairPrefab.transform.rotation).GetComponent<BoxCollider>().bounds;
        }
    }
}
