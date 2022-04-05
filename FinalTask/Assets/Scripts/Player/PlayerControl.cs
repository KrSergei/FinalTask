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
    private const string PLAYER_WALK_BACKWARD_PARAMETR = "WalkBackward";
    private const string PLAYER_WALK_LEFT_PARAMETR = "WalkLeft";
    private const string PLAYER_WALK_RIGHT_PARAMETR = "WalkRight";
    private const string PLAYER_RUN_FORWARD_PARAMETR = "RunForward";
    private const string PLAYER_RUN_BACKWARD_PARAMETR = "RunBackward";
    private const string PLAYER_RUN_LEFT_PARAMETR = "RunLeft";
    private const string PLAYER_RUN_RIGHT_PARAMETR = "RunRight";
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

    // Update is called once per frame
    void FixedUpdate()
    {
        _directionSide = Input.GetAxisRaw("Horizontal");
        _directionForward = Input.GetAxisRaw("Vertical");
        if (CheckingIsRunningState())
        {
            ////если нажата клавиша W, проигрывание анимации Player_RunForward
            if (_directionForward > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_FORWARD_PARAMETR);
                _directionToMove = Vector3.forward * _runSpeed;
            }
            //если нажата клавиша S, проигрывание анимации Player_RunBackward
            if (_directionForward < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_BACKWARD_PARAMETR);
                _directionToMove = -Vector3.forward * _runSpeed;
            }
            //если нажата клавиша D, проигрывание анимации Player_RunRight
            if (_directionSide > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_RIGHT_PARAMETR);
                _directionToMove = Vector3.right * _runSpeed;
            }
            //если нажата клавиша A, проигрывание анимации Player_RunLeft
            if (_directionSide < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_LEFT_PARAMETR);
                _directionToMove = -Vector3.right * _runSpeed;
            }
            //расчет вектора движения
            _directionToMove *= Time.deltaTime;
        }
        else
        {
            //если нажата клавиша W, проигрывание анимации Player_WalkForward
            if (_directionForward > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_FORWARD_PARAMETR);
                _directionToMove = Vector3.forward * _walkSpeed;
            }
            //если нажата клавиша S, проигрывание анимации Player_WalkBackward
            if (_directionForward < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_BACKWARD_PARAMETR);
                _directionToMove = -Vector3.forward * _walkSpeed;
            }
            //если нажата клавиша D, проигрывание анимации Player_WalkRight
            if (_directionSide > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_RIGHT_PARAMETR);
                _directionToMove = Vector3.right * _walkSpeed;
            }
            //если нажата клавиша A, проигрывание анимации Player_WalkLeft
            if (_directionSide < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_LEFT_PARAMETR);
                _directionToMove = -Vector3.right * _walkSpeed;
            }
        //расчет вектора движения
        _directionToMove *= Time.deltaTime;
        }
        //если скорость персонажа равна 0, проигрывание анимации IDLE
        if (_directionForward == 0 && _directionSide == 0)
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_IDLE_PARAMETR);
        }
        StartCoroutine(Move(_directionToMove));
    }


    /// <summary>
    /// Проверка на состояние бега, если нажата кнопка LeftShift, то включен режим шага.
    /// </summary>
    private bool CheckingIsRunningState()
    {
        return (Input.GetKey(KeyCode.LeftShift)) ? false : true;
    }

    IEnumerator Move(Vector3 newDirection)
    {
        _cc.Move(newDirection);
        yield return null;
    }
}
