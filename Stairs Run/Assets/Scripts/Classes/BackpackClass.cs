using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackClass
{
    private GameObject[,] _stairsInBackpack = new GameObject[50, 3];
    private GameObject _miniStairForBackpack, _backpack;

    public int stairsInBackpackCounter;
    public BackpackClass()
    {
        _miniStairForBackpack = Resources.Load<GameObject>("MiniStack");
        _backpack = GameObject.Find("BackPack");
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
                    _stairsInBackpack[vertical, horizontal] = 
                        MonoBehaviour.Instantiate(_miniStairForBackpack, Vector3.zero, Quaternion.identity, _backpack.transform);

                    float localZValue = horizontal == 0 ? -0.1f : horizontal == 1 ? 0 : 0.1f;                // 0.1f is horizontal gap between mini stairs in backpack
                    float localYValue = vertical == 0 ? -0.05f : vertical == 1 ? 0 : 0.05f * (vertical - 1); // 0.05f is vertical gap between mini stairs in backpack
                    var spawnPosition = new Vector3(0, localYValue, localZValue);
                    var rotation = Quaternion.Euler(0, 90, 0);

                    _stairsInBackpack[vertical, horizontal].transform.localPosition = spawnPosition;
                    _stairsInBackpack[vertical, horizontal].transform.localRotation = rotation;

                    stairsInBackpackCounter++;
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
                    stairsInBackpackCounter--;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Drops up to 10 stairs.
    /// </summary>
    public void DropStairsFromBackPack()
    {
        int droppedStairCounter = 0;
        for (int localY = 49; localY >= 0; localY--)
        {
            for (int localZ = 2; localZ >= 0; localZ--)
            {
                // if there's any valid object instantiate it to a temporary object and destroy the original one.
                if (_stairsInBackpack[localY, localZ] != null)
                {
                    var temporalObject = MonoBehaviour.Instantiate( 
                        _stairsInBackpack[localY, localZ],
                        _stairsInBackpack[localY, localZ].transform.position,
                        _stairsInBackpack[localY, localZ].transform.rotation);

                    MonoBehaviour.Destroy(_stairsInBackpack[localY, localZ]);
                    stairsInBackpackCounter--;

                    temporalObject.AddComponent<Rigidbody>()
                        .velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 5f), Random.Range(-1f, 1f));

                    MonoBehaviour.Destroy(temporalObject, 1.5f);

                    droppedStairCounter++;
                    if (droppedStairCounter == 10)
                        return;
                }
            }
        }
    }
    public void DestroyBackpack() => MonoBehaviour.Destroy(_backpack);
}
