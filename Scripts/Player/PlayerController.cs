using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsMoving => _moveVector.Vector3ToFlat() != Vector3.zero && _enabled;
    public Vector3 GetMoveDirection => _enabled ? _moveVector.normalized : Vector3.zero;

    [SerializeField] private Speed speed;
    [SerializeField] private float gravity;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputDetector inputDetector;

    private Vector3 _moveVector;
    private bool _enabled;

    private void Start()
    {
        _enabled = true;
        // EndCutScene.Instance.OnPlaybackStarted += DisableController;
    }

    private void Update()
    {
        if(_enabled)
            Move();
    }

    private void Move()
    {
        _moveVector = inputDetector.GetJoystickInput.Vector2ToVector3XZ();
        if (!characterController.isGrounded)
            _moveVector.y = -gravity;
        characterController.Move(_moveVector.normalized * speed.GetSpeed * Time.deltaTime);
    }

    private void DisableController()
    {
        _enabled = false;
    }
}
