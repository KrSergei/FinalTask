using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Constants
    private const string VERTICAL_PARAMETR_BLEND_TREE = "Forward";
    private const string HORIZONTAL_PARAMETR_BLEND_TREE = "Side";
    #endregion

    public PlayerAnimationConrtol playerAnimationConrtol;
    public Transform _camera;

    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 3f;
    [SerializeField] private Vector3 _directionToMove;
    [SerializeField] private float _directionSide;
    [SerializeField] private float _directionForward;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _turnSmoothVelocity;

    CharacterController _cc;

    void Start()
    {
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
            playerAnimationConrtol.SetFloatValueDirection(VERTICAL_PARAMETR_BLEND_TREE, _directionForward * CheckingIsRunningState());
            playerAnimationConrtol.SetFloatValueDirection(HORIZONTAL_PARAMETR_BLEND_TREE, _directionSide * CheckingIsRunningState());

            //���������� ��������������� ������� ��������: ����� ���� �������� �� ��� x � z.
            //������� �������� �� �������� _directionForward � _directionSide �������������. 
            //�������� ��������� ������ ������������ � ������� �� �������� �������� � ����������� �� ��������� (��� ��� ���).
            _directionToMove = (((Vector3.forward * _directionForward) + ((Vector3.right * _directionSide))).normalized) * CheckingIsRunningState() * Time.deltaTime;
        }
        else
        {
            //��������� ������� ������������
            _directionToMove = Vector3.zero;
            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward);
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide);
        }
        #endregion
        //������ �������� �������� ������ � ������ ����������� ������
        //StartCoroutine(Rotate(_directionToMove));

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

    IEnumerator Rotate(Vector3 newDirection)
    {
        #region var1 Don't working normal
        ////���������� ����� ��������
        //float targetAngle = Mathf.Atan2(newDirection.x, Mathf.Abs(newDirection.z)) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        ////���������� ���� �������� ��� ������������ �������� � ������ �������� �������� � �������
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        ////������� ������
        //transform.rotation = Quaternion.Euler(0f, angle, 0f);
        ////���������� ������ ������� ��������  � ������ �������� ������
        //_directionToMove = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        #endregion+

        yield return null;
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