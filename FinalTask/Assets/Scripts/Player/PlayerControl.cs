using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Constants
    private const string VERTICAL_PARAMETR_BLEND_TREE = "Forward"; 
    private const string HORIZONTAL_PARAMETR_BLEND_TREE = "Side";
    #endregion

    public PlayerAnimationConrtol playerAnimationConrtol;   //��������� ��������
    public Transform mainCamera;                            //��������� �������� ������
    public LayerMask layerMaskGround;                       //����� �����������
    public float distanceToGround = 0.1f;                   //���������� �� �����������

    [SerializeField] private float _walkSpeed = 1f;         //�������� ������������ ��� �������� ������
    [SerializeField] private float _runSpeed = 3f;          //�������� ������������ ��� �������� ����
    [SerializeField] private float _directionSide;          //����������� �������� � �������
    [SerializeField] private float _directionForward;       //����������� �������� ������
    [SerializeField] private float _gravityForce;           //����������� ���� �������
    [SerializeField] private Vector3 _directionToMove;      //�������� ����� ����������� ��������
    [SerializeField] private bool _isGround;                //����, ��������� ����� �� ����� ��� ���
    private CharacterController _cc;  
    private Vector3 _gravity;                               //������ ����������
    private float _radiusCheckingGround = 0.25f;                    //������ ����� ��� �������� ��������� ����� �� ����� ��� ���

    void Start()
    {
        //��������� ���������� CharacterController
        _cc = GetComponent<CharacterController>();
        //��������� ���������� PlayerAnimationConrtol
        playerAnimationConrtol = GetComponent<PlayerAnimationConrtol>();
        //��������� ������� �������� � 0
        _directionToMove = Vector3.zero;
        //����������� ���������� �������� ������
        mainCamera = Camera.main.transform;
        //������� �������
        Cursor.visible = false;
        //������������ ������� �� ������ �����
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        #region Control player
        //����� ������ �� �����������, �� ����������� ��� ��� �����
        IsGround();

        _directionSide = Input.GetAxis("Horizontal");
        _directionForward = Input.GetAxis("Vertical");
  
        //������ �������� �������� ������ � ������ ����������� ������
        StartCoroutine(Rotate());

        //���� ������ ����� �� ������ ���������� (W, A, S, D), � ����������� ��������� �������
        //���������� ����� ��� ��������� ��������������� �������� � ������ �������� Movement, � ��������� ��������� �������� ��������
        if (_directionForward != 0 || _directionSide != 0)
        {
            playerAnimationConrtol.SetFloatValueDirection(VERTICAL_PARAMETR_BLEND_TREE, _directionForward * CheckingIsRunningState());
            playerAnimationConrtol.SetFloatValueDirection(HORIZONTAL_PARAMETR_BLEND_TREE, _directionSide * CheckingIsRunningState());

            //���������� ��������������� ������� ��������: ����� ���� �������� �� ��� x � z, ���������� �� ���� �������� ������ �� ����, ������ �������� ������� ������
            //������� �������� �� �������� _directionForward � _directionSide �������������. 
            //�������� ��������� ������ ������� �� �������� �������� � ����������� �� ��������� (��� ��� ���).
            _directionToMove = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f)
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

        //���������� ���������� � ������ ���������� �� �����������
        _directionToMove += _gravity;
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

    /// <summary>
    /// ����� �������� �� ����������� ��� ��� ��������
    /// </summary>
    private void IsGround()
    {
        //�������� �� ����������� ������ �� ����������� � ��������� ������ �����
        bool checkGround = (Physics.CheckSphere(transform.position, _radiusCheckingGround, layerMaskGround));
        //���� ����� �� �����, �� ��������� ����� _isGround = true
        //� ��������� ����������� ���������� ��� ����������� �������� ������������ �� ��������� �������������
        //����� ���������� � ������� �������� ������ ������� ����������
        if (checkGround)_isGround = true;
        else _isGround = false;
        //����� ������ ��������� ������� ����������
        SetGravityValue(checkGround);
    }

    /// <summary>
    /// ������ ��������� ������� ���������� � ����������� �� �������� ���������
    /// </summary>
    /// <param name="isGround"></param>
    private void SetGravityValue(bool isGround)
    {
        if (isGround) _gravity = Physics.gravity * 0.01f; 
        else _gravity += Physics.gravity * Time.deltaTime * _gravityForce;
    }

    /// <summary>
    /// ������� ������
    /// </summary>
    /// <returns></returns>
    IEnumerator Rotate()
    {
        //������� ������ c ������ ���� �������� ������
        transform.rotation = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f);
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