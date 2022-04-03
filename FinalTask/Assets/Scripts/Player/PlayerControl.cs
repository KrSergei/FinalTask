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
    #endregion

    [SerializeField] private bool _isRunning = false;

    public PlayerAnimationConrtol playerAnimationConrtol;

    //[SerializeField] private float _walkSpeed = 1f;
    //[SerializeField] private float _runSpeed = 2f;

    CharacterController _cc;


    // Start is called before the first frame update
    void Start()
    {
        //Get charater controller
        _cc = GetComponent<CharacterController>();
        playerAnimationConrtol = GetComponent<PlayerAnimationConrtol>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckingIsRunningState();
        //if (Input.GetKeyDown(KeyCode.LeftShift)) _isRunning = true;
        //else _isRunning = false;

        //Если состояния игрока "бег"
        if(_isRunning)
        {

            //если нажата клавиша W, проигрывание анимации Player_RunForward
            if (Input.GetKey(KeyCode.W)) playerAnimationConrtol.ChangeState(PLAYER_RUN_FORWARD_STATE);
        }
        else
        {
            //если нажата клавиша W, проигрывание анимации Player_WalkForward
            if (Input.GetKey(KeyCode.W)) playerAnimationConrtol.ChangeState(PLAYER_WALK_FORWARD_STATE);

        }

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    //если нажата только одна клавиша "режим бега", проигрывание анимации IDLE
        //    playerAnimationConrtol.ChangeState(PLAYER_IDLE_STATE);
        //}

        //если не нажата ни одна клавиша, проигрывание анимации IDLE
        if (Input.anyKey == false) playerAnimationConrtol.ChangeState(PLAYER_IDLE_STATE);
    }

    /// <summary>
    /// Проверка на состояние бега
    /// </summary>
    private void CheckingIsRunningState()
    {
        if (Input.GetKey(KeyCode.LeftShift)) _isRunning = true;
        else _isRunning = false;
    }
}
