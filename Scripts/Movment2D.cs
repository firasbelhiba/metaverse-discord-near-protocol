using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Connection;

public class Movment2D : NetworkBehaviour
{
    public float m_speed = 1.0f;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _userInput;
    private bool _flipped;

     void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        GetComponent<PlayerInput>().enabled = base.IsOwner;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;
        _userInput = context.ReadValue<Vector2>();
        _animator.SetFloat("speed", _userInput.magnitude);
        if (_userInput.x < 0 && !_flipped) 
        {
            _flipped = true;
        } else if (_userInput.x > 0 && _flipped)
        {
            _flipped = false;
        }
        transform.localScale = new Vector3(_flipped ? -1 : 1, 1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!base.IsOwner) return;
        _rigidbody2D.MovePosition(_rigidbody2D.position + _userInput * Time.deltaTime * m_speed);
    }
}
