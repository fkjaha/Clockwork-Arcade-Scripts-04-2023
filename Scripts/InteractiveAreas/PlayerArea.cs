using UnityEngine;
using UnityEngine.Events;

public class PlayerArea : MonoBehaviour
{
    public event UnityAction OnPlayerEntered;
    public event UnityAction OnPlayerLeft;

    public Player GetPlayer => _playerInArea;

    private Player _playerInArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _playerInArea = player;
            OnPlayerEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (_playerInArea == player)
                _playerInArea = null;
            OnPlayerLeft?.Invoke();
        }
    }
}
