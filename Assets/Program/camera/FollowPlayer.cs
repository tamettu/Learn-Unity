using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.15f;
    Transform _playerTransform;
    GameObject _player;
    // Start is called before the first frame update

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player != null)  _playerTransform = _player.transform;
        else Debug.Log("Can't find _player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}
