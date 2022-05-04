using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerControl))]
public class PlayerJumpHandler : MonoBehaviour
{
    private const string JUMP_ANIMATION_PARAMENTR = "Jump";


    [Header("Require Components")]
    [SerializeField] private PlayerControl _playerControl;
    [SerializeField] private CharacterController _cc;
    [SerializeField] private PlayerAnimationConrtol _playerAnimationConrtol;

    [Header("Jump components")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpStartVelocity;
    [SerializeField] private float _maxJumpHeight;


    [Header("Jump status")]
    [SerializeField] private bool _isJump;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _playerControl = GetComponent<PlayerControl>();
        _playerAnimationConrtol = GetComponent<PlayerAnimationConrtol>();
    }

    void Update()
    {
        if (_playerControl.IsGrounded && !_isJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump");
                _isJump = true;
            }
        }

        if (_isJump)
        {
            //пока позиция игрока меньше требуемой высоты прыжка
            if (transform.position.y <= _maxJumpHeight)
            {
                //изменение позиции по оси Y
                _playerControl.VelocityYForJump += _jumpForce * Time.deltaTime;
            }
            CancelStateForJump();
        }
    }

    IEnumerator DoJump(float highJump)
    {
        //print("Start jump");
        //пока позиция игрока меньше требуемой высоты прыжка
        if (transform.position.y <= highJump)
        {
            ////обнуление вектора гравитации
            //_gravity = Vector3.zero;
            //изменение позиции по оси Y
            _playerControl.VelocityYForJump += _jumpForce * Time.deltaTime;
        }
        else CancelStateForJump();
        yield return null;
    }



    private void CancelStateForJump()
    {
        _isJump = false;
        _jumpStartVelocity = 0;
        _maxJumpHeight = 0;
    }


    private float GetVelocityBeAxix()
    {
        return (_cc.velocity.z >= _cc.velocity.x) ? _cc.velocity.z : _cc.velocity.x;
    }
}
