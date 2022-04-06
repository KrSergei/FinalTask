using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Constants

    #endregion

    [SerializeField] private bool _isRunning = false;

    public PlayerAnimationConrtol playerAnimationConrtol;

    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 3f;
    [SerializeField] private Vector3 _directionToMove;
    [SerializeField] private float _directionSide;
    [SerializeField] private float _directionForward;

    CharacterController _cc;
    // Start is called before the first frame update
    void Start()
    {
        //Get charater controller
        _cc = GetComponent<CharacterController>();
        playerAnimationConrtol = GetComponent<PlayerAnimationConrtol>();
        _directionToMove = Vector3.zero; 
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
        #region Control player
        _directionSide = Input.GetAxis("Horizontal");
        _directionForward = Input.GetAxis("Vertical");

        //���� ������ ����� �� ������ ���������� (W, A, S, D), � ����������� ��������� �������
        //���������� ����� ��� ��������� ��������������� �������� � ������ �������� Movement, � ��������� ��������� �������� ��������
        if (_directionForward != 0 || _directionSide != 0)
        {
            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward * CheckingIsRunningState());
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide * CheckingIsRunningState());

            //���������� ��������������� ������� ��������: ����� ���� �������� �� ��� x � z.
            //������� �������� �� �������� _directionForward � _directionSide �������������. 
            //�������� ��������� ������ ������������ � ������� �� �������� �������� � ����������� �� ��������� (��� ��� ���).
            _directionToMove = (((Vector3.forward * _directionForward)
                + ((Vector3.right * _directionSide))).normalized) 
                * CheckingIsRunningState() * Time.deltaTime;
        }
        else
        {

            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward);
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide);
            //��������� ������� ������������
            _directionToMove = Vector3.zero;
        }
        #endregion
        //������ �������� ������������ � ��������� ��������������� ������� ����������� ��������
=======
        _directionSide = Input.GetAxisRaw("Horizontal");
        _directionForward = Input.GetAxisRaw("Vertical");
        if (CheckingIsRunningState())
        {
            ////���� ������ ������� W, ������������ �������� Player_RunForward
            if (_directionForward > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_FORWARD_PARAMETR);
                _directionToMove = Vector3.forward * _runSpeed;
            }
            //���� ������ ������� S, ������������ �������� Player_RunBackward
            if (_directionForward < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_BACKWARD_PARAMETR);
                _directionToMove = -Vector3.forward * _runSpeed;
            }
            //���� ������ ������� D, ������������ �������� Player_RunRight
            if (_directionSide > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_RIGHT_PARAMETR);
                _directionToMove = Vector3.right * _runSpeed;
            }
            //���� ������ ������� A, ������������ �������� Player_RunLeft
            if (_directionSide < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_LEFT_PARAMETR);
                _directionToMove = -Vector3.right * _runSpeed;
            }
            //������ ������� ��������
            _directionToMove *= Time.deltaTime;
        }
        else
        {
            //���� ������ ������� W, ������������ �������� Player_WalkForward
            if (_directionForward > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_FORWARD_PARAMETR);
                _directionToMove = Vector3.forward * _walkSpeed;
            }
            //���� ������ ������� S, ������������ �������� Player_WalkBackward
            if (_directionForward < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_BACKWARD_PARAMETR);
                _directionToMove = -Vector3.forward * _walkSpeed;
            }
            //���� ������ ������� D, ������������ �������� Player_WalkRight
            if (_directionSide > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_RIGHT_PARAMETR);
                _directionToMove = Vector3.right * _walkSpeed;
            }
            //���� ������ ������� A, ������������ �������� Player_WalkLeft
            if (_directionSide < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_LEFT_PARAMETR);
                _directionToMove = -Vector3.right * _walkSpeed;
            }
        //������ ������� ��������
        _directionToMove *= Time.deltaTime;
        }
        //���� �������� ��������� ����� 0, ������������ �������� IDLE
        if (_directionForward == 0 && _directionSide == 0)
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_IDLE_PARAMETR);
        }
>>>>>>> parent of ce50395 (Added BlenderTree and refactoring script)
        StartCoroutine(Move(_directionToMove));
    }


    /// <summary>
    /// �������� �� ��������� ����, ���� ������ ������ LeftShift, �� ������� ����� ����. ���������� �������� ��������, �� �������
    /// ���������� �������� ������ �����������
    /// </summary>
    private bool CheckingIsRunningState()
    {
        return (Input.GetKey(KeyCode.LeftShift)) ? false : true;
    }

    /// <summary>
    /// ������������ ������ � �������� ���������� - �������� ����������� ��������
    /// </summary>
    /// <param name="newDirection"></param>
    /// <returns></returns>
    IEnumerator Move(Vector3 newDirection)
    {
        _cc.Move(newDirection);
        yield return null;
    }
}
