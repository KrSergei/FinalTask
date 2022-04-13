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
    public Transform _mainCamera;

    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 3f;
    [SerializeField] private Vector3 _directionToMove;
    [SerializeField] private float _directionSide;
    [SerializeField] private float _directionForward;


    CharacterController _cc;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
        playerAnimationConrtol = GetComponent<PlayerAnimationConrtol>();
        _directionToMove = Vector3.zero;
        _mainCamera = Camera.main.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        #region Control player
        _directionSide = Input.GetAxis("Horizontal");
        _directionForward = Input.GetAxis("Vertical");
  
        //������ �������� �������� ������ � ������ ����������� ������
        StartCoroutine(Rotate());

        ////���� ������ ����� �� ������ ���������� (W, A, S, D), � ����������� ��������� �������
        ////���������� ����� ��� ��������� ��������������� �������� � ������ �������� Movement, � ��������� ��������� �������� ��������
        if (_directionForward != 0 || _directionSide != 0)
        {
            playerAnimationConrtol.SetFloatValueDirection(VERTICAL_PARAMETR_BLEND_TREE, _directionForward * CheckingIsRunningState());
            playerAnimationConrtol.SetFloatValueDirection(HORIZONTAL_PARAMETR_BLEND_TREE, _directionSide * CheckingIsRunningState());

            //���������� ��������������� ������� ��������: ����� ���� �������� �� ��� x � z, ���������� �� ���� �������� ������ �� ����, ������ �������� ������� ������
            //������� �������� �� �������� _directionForward � _directionSide �������������. 
            //�������� ��������� ������ ������� �� �������� �������� � ����������� �� ��������� (��� ��� ���).
            _directionToMove = Quaternion.Euler(0f, _mainCamera.eulerAngles.y, 0f) 
                * ((Vector3.forward * _directionForward) + (Vector3.right * _directionSide))
                * CheckingIsRunningState() * Time.fixedDeltaTime;
        }
        else
        {
            //��������� ������� ������������
            _directionToMove = Vector3.zero;
            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward);
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide);
        }
        #endregion

        ////������ �������� ������������ � ��������� ��������������� ������� ����������� ��������
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

    IEnumerator Rotate()
    {
        //������� ������ c ������ ���� �������� ������
        transform.rotation = Quaternion.Euler(0f, _mainCamera.eulerAngles.y, 0f);
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