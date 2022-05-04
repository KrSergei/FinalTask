using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform mainCamera;                            //Трансформ основной камеры



    public float speed = 3f;
    public float rotateSpeed = 90f;
    public float gravity = -20f;
    public float jumpSpeed = 15;
    [SerializeField] private bool _isGrounded;

    private CharacterController _cc;
    private Animator _anim;
    Vector3 move;
    Vector3 rotate;


    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }


    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");

        //Определению, на поверхности или нет игрок
        IsGround();
        //Запуск корутины поворота игрока с учетом направления камеры
        StartCoroutine(Rotate());
        if (_isGrounded)
        {
            //_isGrounded = true;
            move = transform.forward * speed * vInput;
            rotate = transform.up * rotateSpeed * hInput;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move.y = jumpSpeed;
            }
        }
        //else _isGrounded = false;
        move.y += SetValueGravity() * Time.deltaTime;

        _cc.Move(move * Time.deltaTime);
        //transform.Rotate(rotate * Time.deltaTime  
    }

    private void IsGround()
    {
        _isGrounded = (_cc.isGrounded) ? true : false;
    }

    private float SetValueGravity()
    {
        return (_cc.isGrounded) ? gravity * 0.1f : gravity;
    }


    IEnumerator DoMove()
    {
        _cc.Move(move * Time.deltaTime);
        yield return null;
    }

    /// <summary>
    /// Поворот игрока
    /// </summary>
    /// <returns></returns>
    IEnumerator Rotate()
    {
        //Поворот игрока c учетом угла поворота камеры
        transform.rotation = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f);
        yield return null;
    }
}
