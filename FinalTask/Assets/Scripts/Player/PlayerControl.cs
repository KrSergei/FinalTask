using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
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
    public Vector3 targetJump;
    public bool doJump = false;

    [SerializeField] private float _walkSpeed = 1f;         //�������� ������������ ��� �������� ������
    [SerializeField] private float _runSpeed = 3f;          //�������� ������������ ��� �������� ����
    [SerializeField] private float _directionSide;          //����������� �������� � �������
    [SerializeField] private float _directionForward;       //����������� �������� ������
    [SerializeField] private float _gravityForce;           //����������� ���� �������
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _deltaJumpForce;            
    [SerializeField] private Vector3 _directionToMove;      //�������� ����� ����������� ��������
    [SerializeField] private bool _isGround;                //����, ��������� ����� �� ����� ��� ���

    private CharacterController _cc; 
    [SerializeField]
    private Vector3 _gravity;                              //������ ����������
    private float _radiusGroundChecker = 0.25f;            //������ ����� ��� �������� ��������� ����� �� ����� ��� ���
    private float _highForJump;                            //������ ��� ������. ��������� ������ ������� ������� ������ �� ������� Y + _jumpForce

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

    private void Update()
    {
        
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
                * CheckingIsRunningState() * Time.deltaTime;
        }
        else
        {
            //��������� ������� ������������
            _directionToMove = Vector3.zero;
            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward);
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide);
        }
        #endregion

        if (_isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                doJump = true;
                //���������� ������� ������
                _highForJump = transform.position.y + _jumpForce;
                StartCoroutine(DoJump(_highForJump));
            }
        }
        //���� ����� ��������� � ���������� ������, ������ �������� �� ���������� ������
        if (doJump)
        {
            //������ �������� ������
            StartCoroutine(DoJump(_highForJump));
        }
        //���������� ���������� � ��������� ������� ��������
        _directionToMove += _gravity;
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
    /// ���������� �����
    /// </summary>
    /// <param name="highJump">������ �� ������� ������ �������� �����</param>
    /// <returns></returns>
    IEnumerator DoJump(float highJump)
    {
        //���� ������� ������ ������ ��������� ������ ������
        if (transform.position.y <= highJump)
        {
            //��������� ������� ����������
            _gravity = Vector3.zero;
            //��������� ������� �� ��� Y
            _directionToMove.y += _jumpForce * _deltaJumpForce;
        }
        else doJump = false;
        yield return null;
    }

    /// <summary>
    /// ����� �������� �� ����������� ��� ��� ��������
    /// </summary>
    private void IsGround()
    {
        //�������� �� ����������� ������ �� ����������� � ��������� ������ �����
        bool checkGround = (Physics.CheckSphere(transform.position, _radiusGroundChecker, layerMaskGround));
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
        if (isGround) _gravity = Physics.gravity * _gravityForce;
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