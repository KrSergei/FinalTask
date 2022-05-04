using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    #region Constants
    private const string VERTICAL_PARAMETR_BLEND_TREE = "Forward"; 
    private const string HORIZONTAL_PARAMETR_BLEND_TREE = "Side";
    private const string JUMP_PARAMETR_BLEND_TREE = "Jump";
    private const string MOVING_PARAMETR_BLEND_TREE = "IsMoving";
    #endregion

    public PlayerAnimationConrtol playerAnimationConrtol;   //��������� ��������
    public Transform mainCamera;                            //��������� �������� ������
    public LayerMask layerMaskGround;                       //����� �����������
    public float distanceToGround = 0.2f;                   //���������� �� �����������
    public Vector3 targetJump;
    public bool doJump = false;

    public float _gravityForce2 = -20f;


    [SerializeField] private float _walkSpeed = 1f;         //�������� ������������ ��� �������� ������
    [SerializeField] private float _runSpeed = 3f;          //�������� ������������ ��� �������� ����
    [SerializeField] private float _directionSide;          //����������� �������� � �������
    [SerializeField] private float _directionForward;       //����������� �������� ������
    [SerializeField] private float _gravityForce;           //����������� ���� �������
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _deltaJumpForce;            
    [SerializeField] private Vector3 _directionToMove;      //�������� ����� ����������� ��������
    [SerializeField] private float _velocityYForJump;

    [SerializeField] private float _radiusGroundChecker;    //������ ����� ��� �������� ��������� ����� �� ����� ��� ���
    [SerializeField] private bool _isGround;                //����, ��������� ����� �� ����� ��� ���
    [SerializeField] private bool _isMoving;                //����, ��������� ����� � �������� ��� ���

    private CharacterController _cc; 
    [SerializeField]
    private Vector3 _gravity;                              //������ ����������
    private float _highForJump;                            //������ ��� ������. ��������� ������ ������� ������� ������ �� ������� Y + _jumpForce

    public bool IsGrounded{ get => _isGround; private set => _isGround = value; }
    public float VelocityYForJump { get => _velocityYForJump; set => _velocityYForJump = value; }
    public float GravityForce { get => _gravityForce; set => _gravityForce = value; }

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
        ////����� ������ �� �����������, �� ����������� ��� ��� �����
        IsGround();

        //������ �������� �������� ������ � ������ ����������� ������
        StartCoroutine(Rotate());

        //if (IsGrounded && !doJump)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        print("Jump");
        //        doJump = true;
        //        _directionToMove.y = _jumpForce;
        //    }
        //}
    }

    void FixedUpdate()
    {
        #region Control player
        //����� ������ �� �����������, �� ����������� ��� ��� �����
        //IsGround();

        //������ �������� �������� ������ � ������ ����������� ������
        //StartCoroutine(Rotate());

        if (doJump == false)
        {
            _directionSide = Input.GetAxis("Horizontal");
            _directionForward = Input.GetAxis("Vertical");
            //���� ������ ����� �� ������ ����������(W, A, S, D), � ����������� ��������� �������
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
                    * CheckingIsRunningState();
            }
            else
            {
                playerAnimationConrtol.SetFloatValueDirection("Forward", 0);
                playerAnimationConrtol.SetFloatValueDirection("Side", 0);
                //��������� ������� ������������
                _directionToMove = Vector3.zero;
                //playerAnimationConrtol.PlayAnimationByParametr(MOVING_PARAMETR_BLEND_TREE, false);
            }
        }
        #endregion

        if (IsGrounded && !doJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("Jump");
                doJump = true;
                _directionToMove.y = _jumpForce;
            }
        }

        //���������� ���������� � ��������� ������� ��������
        _directionToMove.y += SetGravityValue() * Time.deltaTime;

        //������ �������� ������������ � ��������� ��������������� ������� ����������� ��������
        StartCoroutine(Move(_directionToMove * Time.deltaTime));
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
        //print("Start jump");
        //���� ������� ������ ������ ��������� ������ ������
        if (transform.position.y <= highJump)
        {
            //��������� ������� ����������
            //_gravity = Vector3.zero;
            //��������� ������� �� ��� Y
            _directionToMove.y += _jumpForce;
        }
        else CanselActiveState();
        yield return null;
    }

    public void CanselActiveState()
    {
        doJump = false;
        _directionSide = 0;
        _directionForward = 0;
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
        if (checkGround)IsGrounded = true;
        else IsGrounded = false;
    }

    /// <summary>
    /// ������ ��������� ������� ���������� � ����������� �� �������� ���������
    /// </summary>
    /// <param name="isGround"></param>
    private float SetGravityValue()
    {
        return (_isGround) ? _directionToMove.y += _gravityForce2 * 0.01f : _directionToMove.y += _gravityForce2;
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