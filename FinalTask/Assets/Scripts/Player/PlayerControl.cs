using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Constants
    private const string PLAYER_IDLE_STATE = "Player_IDLE";
    private const string PLAYER_WALK_FORWARD_STATE = "Player_WalkForward";
    private const string PLAYER_RUN_FORWARD_STATE = "Player_RunForward";
    //private const string PLAYER_WALK_BACKWARD_STATE = "WalkBackward";

    private const string PLAYER_IDLE_PARAMETR = "IDLE";
    private const string PLAYER_WALK_FORWARD_PARAMETR = "WalkForwardTr";
    private const string PLAYER_RUN_FORWARD_PARAMETR = "RunForwardTr";
    private const string PLAYER_WALK_BACKWARD_PARAMETR = "WalkBackwardTr";

    #endregion

    [SerializeField] private bool _isRunning = false;

    public PlayerAnimationConrtol playerAnimationConrtol;

    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 2f;
    [SerializeField] private Vector3 _directionToMove;

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

        //если нажата клавиша W, проигрывание анимации Player_WalkForward
        //if (Input.GetKey(KeyCode.W))
        //{
        //    _directionToMove = Vector3.forward * _walkSpeed;
        //    playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_FORWARD_PARAMETR);
        //    ++ _countPressedbutton;
        //    Debug.Log("_countPressedbutton = " + _countPressedbutton);
        //}
        ////если нажата клавиша S, проигрывание анимации Player_WalkBackword
        //if (Input.GetKey(KeyCode.S))
        //{
        //    playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_BACKWARD_PARAMETR);
        //    _directionToMove = -Vector3.forward * _walkSpeed;
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    playerAnimationConrtol.ChangeAnimationByBoolParametr(PLAYER_WALK_BACKWARD_PARAMETR, true);
        //    _directionToMove = -Vector3.forward * _walkSpeed;
        //}

        ////if (_isRunning == false)
        ////{

        ////}
        ////if (_isRunning)
        ////{
        ////    //если нажата клавиша W, проигрывание анимации Player_RunForward
        ////    if (Input.GetKey(KeyCode.W))
        ////    {
        ////        playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_FORWARD_PARAMETR);
        ////        _directionToMove = Vector3.forward * _runSpeed;
        ////    } 
        ////}

        ////если скорость персонажа равна 0, проигрывание анимации IDLE
        //if (_cc.velocity == Vector3.zero) playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_IDLE_PARAMETR);
        ////расчет вектора движения
        //_directionToMove *= Time.deltaTime;
        //StartCoroutine(Move(_directionToMove));

        
        //если нажата клавиша W, проигрывание анимации Player_WalkForward
        if (Input.GetKey(KeyCode.W))
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_FORWARD_PARAMETR);
            _directionToMove = Vector3.forward * _walkSpeed;
        }
        //если нажата клавиша S, проигрывание анимации Player_WalkBackword
        if (Input.GetKey(KeyCode.S))
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_BACKWARD_PARAMETR);
            _directionToMove = -Vector3.forward * _walkSpeed;
        }

        
        //если скорость персонажа равна 0, проигрывание анимации IDLE
        if (_cc.velocity == Vector3.zero) playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_IDLE_PARAMETR);
        //_directionToMove *= Time.deltaTime;
        //StartCoroutine(Move(_directionToMove));

    }

    private void FixedUpdate()
    {
        //расчет вектора движения
        _directionToMove *= Time.deltaTime;
        StartCoroutine(Move(_directionToMove));
    }

    /// <summary>
    /// Проверка на состояние бега
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
