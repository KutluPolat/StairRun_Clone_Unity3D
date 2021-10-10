using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairClass
{
    private BoxCollider _boxColliderOfPlayer;
    private Bounds _boundsOfLatestSpawnedNormalStair;
    private GameObject _normalStairsForBuildingStair;

    public StairClass()
    {
        _normalStairsForBuildingStair = Resources.Load<GameObject>("Stair");
        _boxColliderOfPlayer = GameObject.Find("Chibi").GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Resetting bounds of latest spawned stair according to the player's position so when we call PlaceStairs() method stairs should start to spawn under the player.
    /// </summary>
    public void SetStairSpawnPositionUnderThePlayer()
    {
        _boundsOfLatestSpawnedNormalStair.center = _boxColliderOfPlayer.bounds.center;
        _boundsOfLatestSpawnedNormalStair.min = _boxColliderOfPlayer.bounds.max;
        _boundsOfLatestSpawnedNormalStair.max = _boxColliderOfPlayer.bounds.min;
    }

    /// <summary>
    /// Places stairs under the player.
    /// </summary>
    public void PlaceStairs()
    {
        if (_boundsOfLatestSpawnedNormalStair == null || _boxColliderOfPlayer.bounds.max.x < _boundsOfLatestSpawnedNormalStair.min.x)
        {
            GameManager.backpack.DeleteStairFromBackpack();

            var spawnPosition = new Vector3(
                _boxColliderOfPlayer.bounds.center.x,
                _boxColliderOfPlayer.bounds.min.y,
                _boxColliderOfPlayer.bounds.center.z);

            _boundsOfLatestSpawnedNormalStair = MonoBehaviour.Instantiate(_normalStairsForBuildingStair, spawnPosition, _normalStairsForBuildingStair.transform.rotation).GetComponent<BoxCollider>().bounds;
        }
    }
}
