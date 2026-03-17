using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] Vector2 _windowSize = new Vector2(512,512);
    [SerializeField] float _delay = 1.0f;
    bool _IsOutOfRange = false;
    float _outRangeTime = 0;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_IsOutOfRange);
        var nowTime = Time.time;
        Vector2 playerPosition = playerTransform.position;
        Vector2 cameraPosition = transform.position;
        if (!_IsOutOfRange 
        && (playerPosition.x > _windowSize.x 
        || playerPosition.y > _windowSize.y))
        {
            _IsOutOfRange = true;
            _outRangeTime = nowTime;
        }
        if ( _IsOutOfRange && nowTime - _outRangeTime >= _delay)
        {
            bool outX = false;
            bool outY = false;
            while (!outX || !outY)
            {
                if ((playerPosition.x < 0 && cameraPosition.x * _moveSpeed <= playerPosition.x)
                || (playerPosition.x > 0 && cameraPosition.x * _moveSpeed >= playerPosition.x))
                {
                    outX = true;
                }
                if ((playerPosition.y < 0 && cameraPosition.y * _moveSpeed <= playerPosition.y)
                || (playerPosition.y > 0 && cameraPosition.y * _moveSpeed >= playerPosition.y))
                {
                    outY = true;
                }
                transform.Translate(new Vector2
                (!outX ? 0 : cameraPosition.x * _moveSpeed * Time.deltaTime
                , !outY ? 0: cameraPosition.y * _moveSpeed * Time.deltaTime));
            }
            _IsOutOfRange = false;
        }
    }
}
