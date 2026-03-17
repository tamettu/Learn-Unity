using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class player_move : MonoBehaviour
{
    //=~=~=~=~=~=~=~=~=~=~=~=~Initialization=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~
    Animator _animator;
    Rigidbody2D _rb;
    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _rollSpeed = 2f;
    [SerializeField] float _totalRollTime= 0.43f;
    [SerializeField] float _rollCD = 0.5f;
    float _totalTime = 0f;
    float _lastRollTime = 0f;
    bool _isRoll = false;
    //=~=~=~=~=~=~=~=~=~=~=~=~End of Initialization=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~
    
    //=~=~=~=~=~=~=~=~=~=~=~=~ALL Method=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~
    Vector2 Move()
    {
        Vector2 direction = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.A))direction.x +=-1;
        if (Input.GetKey(KeyCode.D))direction.x +=1;
        if (Input.GetKey(KeyCode.W))direction.y +=1;
        if (Input.GetKey(KeyCode.S))direction.y +=-1;
        
        _rb.velocity = direction.normalized * _moveSpeed;
        return direction;
    }
    void Rolling(Vector2 direction)
    {
        if (_totalTime - _lastRollTime <= _totalRollTime && _isRoll)
        {
            _rb.AddForce(direction.normalized * _rollSpeed, ForceMode2D.Impulse);
        }
        else _animator.SetBool("IsRolling",false);

        if (_isRoll && _totalTime - _lastRollTime >= _rollCD)
        {
            _isRoll = false; 
            return;
        }

        if (!Input.GetKeyDown(KeyCode.LeftControl)) return;   

        _isRoll = true; _animator.SetBool("IsRolling",true);
        _lastRollTime = _totalTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _totalTime = Time.time;
        var direction = Move();
        if (direction.x != 0 || direction.y != 0)
        {
            _animator.SetBool("IsMoving",true); 
            _animator.SetFloat("Y",direction.y);
            _animator.SetFloat("X",direction.x);
            Rolling(direction);
        }
        else if (_isRoll)Rolling(direction);
        else if (!_isRoll) _animator.SetBool("IsMoving",false);
        
        
        
    }

}
