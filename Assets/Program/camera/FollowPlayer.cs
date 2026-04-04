using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
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
    void Update()
    {
        transform.position = new Vector3 (_playerTransform.position.x,_playerTransform.position.y,-1);    
    }
}
