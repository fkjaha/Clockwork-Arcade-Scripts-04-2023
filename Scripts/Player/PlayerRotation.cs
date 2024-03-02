using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform rotationTarget;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        if (playerController.IsMoving)
        {
            RotateTarget(playerController.GetMoveDirection.Vector3ToFlat());
        }
    }

    private void RotateTarget(Vector3 direction)
    {
        rotationTarget.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
