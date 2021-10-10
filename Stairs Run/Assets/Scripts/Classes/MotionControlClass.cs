using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionControlClass : StairClass
{
    private GameObject _player;
    private Transform _playerTransform;
    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;

    public bool _isGameStarted, _isWinConditionTrue, _isMovementDisabled;

    private float _playerVerticalSpeed, _playerHorizontalSpeed;

    public MotionControlClass(float playerVerticalSpeed, float playerHorizontalSpeed)
    {
        _player = GameObject.Find("Chibi");
        _playerTransform = _player.transform;
        _playerRigidbody = _player.GetComponent<Rigidbody>();
        _playerAnimator = _player.GetComponent<Animator>();

        _playerVerticalSpeed = playerVerticalSpeed;
        _playerHorizontalSpeed = playerHorizontalSpeed;

        _isGameStarted = false;
        _isWinConditionTrue = false;
        _isMovementDisabled = false;
    }
    public void CloseGravityAndResetVelocity()
    {
        _playerRigidbody.useGravity = false;
        _playerRigidbody.velocity = Vector3.zero;
        _playerRigidbody.angularVelocity = Vector3.zero;
    }

    public void FallDown()
    {
        // Set useGravity to true so chibi can fall down
        _playerRigidbody.useGravity = true;

        // Add rigidbody to placed stairs and destroy them after one second
        foreach (GameObject spawnedStair in GameObject.FindGameObjectsWithTag("SpawnedStair"))
        {
            if (spawnedStair.GetComponent<Rigidbody>() == null)
                spawnedStair.AddComponent<Rigidbody>();

            spawnedStair.GetComponent<BoxCollider>().isTrigger = false; // Closing is trigger so stairs can stop when they hit platform.

            MonoBehaviour.Destroy(spawnedStair, 1f);
        }
    }

    public void ClimbLadder() => Move(0, _playerVerticalSpeed * Time.deltaTime);
    public void MovePlayerForward() => Move(-_playerHorizontalSpeed * Time.deltaTime);

    private void Move(float directionX, float directionY = 0)
    {
        if (_isGameStarted && !_isWinConditionTrue && !_isMovementDisabled)
        {
            _playerTransform.position = new Vector3(
                    _playerTransform.position.x + directionX,
                    _playerTransform.position.y + directionY,
                    _playerTransform.position.z);

        }
    }
    public void AnimationController()
    {
        if (_isWinConditionTrue)
            _playerAnimator.SetTrigger("Dance");

        else if (_playerRigidbody.velocity.magnitude > 0.5f && _isGameStarted)
            _playerAnimator.SetTrigger("Fall");

        else if (_isGameStarted)
            _playerAnimator.SetTrigger("Run");
    }

    public void DisableAnimator() => _playerAnimator.enabled = false;
    public void TurnChibisFaceTowardsPlayer() => _playerTransform.rotation = Quaternion.Euler(0, 90, 0);
}
