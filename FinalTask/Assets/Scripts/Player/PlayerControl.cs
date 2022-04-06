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
        StartCoroutine(Move(_directionToMove));
    }


    /// <summary>
    /// �������� �� ��������� ����, ���� ������ ������ LeftShift, �� ������� ����� ����. ���������� �������� ��������, �� �������
    /// ���������� �������� ������ �����������
    /// </summary>
    private float CheckingIsRunningState()
    {
        return (Input.GetKey(KeyCode.LeftShift)) ? _runSpeed : _walkSpeed;
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
