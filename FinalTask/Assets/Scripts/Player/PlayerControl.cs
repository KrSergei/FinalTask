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
        
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.W)))
        {
            //���� ������ ������� W + Shift, ������������ �������� Player_RunForward
            playerAnimationConrtol.ChangeState(PLAYER_RUN_FORWARD_STATE);
        }

        //���� �� ������ �� ���� �������, ������������ �������� IDLE
        if (Input.anyKey == false) playerAnimationConrtol.ChangeState(PLAYER_IDLE_STATE);
        //���� ������ ������� W, ������������ �������� Player_WalkForward
        if (Input.GetKeyDown(KeyCode.W)) playerAnimationConrtol.ChangeState(PLAYER_WALK_FORWARD_STATE);

       

        

    }
}
