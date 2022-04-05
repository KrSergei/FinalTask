using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Constants
    //private const string PLAYER_IDLE_STATE = "Player_IDLE";
    //private const string PLAYER_WALK_FORWARD_STATE = "Player_WalkForward";
    //private const string PLAYER_RUN_FORWARD_STATE = "Player_RunForward";
    //private const string PLAYER_WALK_BACKWARD_STATE = "WalkBackward";

    private const string PLAYER_IDLE_PARAMETR = "IDLE";
    private const string PLAYER_WALK_FORWARD_PARAMETR = "WalkForward";
    private const string PLAYER_RUN_FORWARD_PARAMETR = "RunForward";
    private const string PLAYER_WALK_BACKWARD_PARAMETR = "WalkBackward";
    private const string PLAYER_WALK_LEFT_PARAMETR = "WalkLeft";
    private const string PLAYER_WALK_RIGHT_PARAMETR = "WalkRight";
    #endregion

    [SerializeField] private bool _isRunning = false;

    public PlayerAnimationConrtol playerAnimationConrtol;

    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 2f;
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

    // Update is called once per frame
    void Update()
    {
        //CheckingIsRunningState();
        _directionSide = Input.GetAxisRaw("Horizontal");
        _directionForward = Input.GetAxisRaw("Vertical");

        //���� ������ ������� W, ������������ �������� Player_WalkForward
        if (_directionForward > 0)
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_FORWARD_PARAMETR);
            _directionToMove = Vector3.forward * _walkSpeed;
        }
        //���� ������ ������� S, ������������ �������� Player_WalkBackword
        if (_directionForward < 0)
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_BACKWARD_PARAMETR);
            _directionToMove = -Vector3.forward * _walkSpeed;
        }

        if(_directionSide > 0)
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_RIGHT_PARAMETR);
            _directionToMove = Vector3.right * _walkSpeed;
        }
        if(_directionSide < 0)
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_LEFT_PARAMETR);
            _directionToMove = -Vector3.right * _walkSpeed;
        }

        //���� �������� ��������� ����� 0, ������������ �������� IDLE
        if (_directionForward == 0 && _directionSide == 0) playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_IDLE_PARAMETR);
        //������ ������� ��������
        _directionToMove *= Time.deltaTime;
        StartCoroutine(Move(_directionToMove));
       
        

        //_directionToMove *= Time.deltaTime;
        //StartCoroutine(Move(_directionToMove));

    }

    //private void FixedUpdate()
    //{
        
    //}

    /// <summary>
    /// �������� �� ��������� ����
    /// </summary>
    private void CheckingIsRunningState()
    {
        if (Input.GetKey(KeyCode.LeftShift)) _isRunning = true;
        else _isRunning = false;
    }

    IEnumerator Move(Vector3 newDirection)
    {
        _cc.Move(newDirection);
        yield return null;
    }
}
