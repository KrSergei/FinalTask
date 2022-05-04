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

    public PlayerAnimationConrtol playerAnimationConrtol;   //Компонент аниматор
    public Transform mainCamera;                            //Трансформ основной камеры
    public LayerMask layerMaskGround;                       //Маска поверхности
    public float distanceToGround = 0.2f;                   //Расстояние до поверхности
    public Vector3 targetJump;
    public bool doJump = false;

    public float _gravityForce2 = -20f;


    [SerializeField] private float _walkSpeed = 1f;         //Скорость передвижения при анимации ходьбы
    [SerializeField] private float _runSpeed = 3f;          //Скорость передвижения при анимации бега
    [SerializeField] private float _directionSide;          //Направлеине движения в стороны
    [SerializeField] private float _directionForward;       //Направлеине движения вперед
    [SerializeField] private float _gravityForce;           //Коэффициент силы тяжести
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _deltaJumpForce;            
    [SerializeField] private Vector3 _directionToMove;      //Итоговый ветор направления движения
    [SerializeField] private float _velocityYForJump;

    [SerializeField] private float _radiusGroundChecker;    //Радиус сферы для проверки находится игрок на земле или нет
    [SerializeField] private bool _isGround;                //Флаг, находится игрок на земле или нет
    [SerializeField] private bool _isMoving;                //Флаг, находится игрок в движении или нет

    private CharacterController _cc; 
    [SerializeField]
    private Vector3 _gravity;                              //Вектор гравитации
    private float _highForJump;                            //Высота для прыжка. Вычисляет суммой текущей позиции игрока по позиции Y + _jumpForce

    public bool IsGrounded{ get => _isGround; private set => _isGround = value; }
    public float VelocityYForJump { get => _velocityYForJump; set => _velocityYForJump = value; }
    public float GravityForce { get => _gravityForce; set => _gravityForce = value; }

    void Start()
    {
        //Получение компонента CharacterController
        _cc = GetComponent<CharacterController>();
        //Получение компонента PlayerAnimationConrtol
        playerAnimationConrtol = GetComponent<PlayerAnimationConrtol>();
        //Установка вектора движения в 0
        _directionToMove = Vector3.zero;
        //Определение трансформа основной камеры
        mainCamera = Camera.main.transform;
        //Скрытие курсора
        Cursor.visible = false;
        //Блокирование курсора по центру карты
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ////Вызом метода по определению, на поверхности или нет игрок
        IsGround();

        //Запуск корутины поворота игрока с учетом направления камеры
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
        //Вызом метода по определению, на поверхности или нет игрок
        //IsGround();

        //Запуск корутины поворота игрока с учетом направления камеры
        //StartCoroutine(Rotate());

        if (doJump == false)
        {
            _directionSide = Input.GetAxis("Horizontal");
            _directionForward = Input.GetAxis("Vertical");
            //Если нажата любая из клавиш управления(W, A, S, D), в управляющем анимацией скрипте
            //вызывается метод для изменения соответввующего значения в дереве анимаций Movement, с передачей параметра текущего значения
            if (_directionForward != 0 || _directionSide != 0)
            {
                playerAnimationConrtol.SetFloatValueDirection(VERTICAL_PARAMETR_BLEND_TREE, _directionForward * CheckingIsRunningState());
                playerAnimationConrtol.SetFloatValueDirection(HORIZONTAL_PARAMETR_BLEND_TREE, _directionSide * CheckingIsRunningState());
                //Вычисление результирующего вектора движения: сумма двух векторов по оси x и z, умноженной на угол поворота игрока на угол, равный поворота главной камеры
                //Векторы умножены на значение _directionForward и _directionSide соответсвенно. 
                //Итоговый суммарный вектор умножен на значение скорости в зависимости от состояния (бег или шаг).
                _directionToMove = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f)
                    * ((Vector3.forward * _directionForward) + (Vector3.right * _directionSide))
                    * CheckingIsRunningState();
            }
            else
            {
                playerAnimationConrtol.SetFloatValueDirection("Forward", 0);
                playerAnimationConrtol.SetFloatValueDirection("Side", 0);
                //Обнуление вектора передвижения
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

        //Добавление гравитации к итоговому вектору движения
        _directionToMove.y += SetGravityValue() * Time.deltaTime;

        //Запуск корутины передвижения с указанием результирующего вектора направления движения
        StartCoroutine(Move(_directionToMove * Time.deltaTime));
    }

    /// <summary>
    /// Проверка на состояние бега, если нажата кнопка LeftShift, то включен режим шага. Возвращает значение скорости, на которое
    /// умножается итоговый вектор перемещения
    /// </summary>
    private float CheckingIsRunningState()
    {
        return (Input.GetKey(KeyCode.LeftShift)) ? _runSpeed : _walkSpeed;
    }

    /// <summary>
    /// Реализация прыжа
    /// </summary>
    /// <param name="highJump">высота на которую должен прыгнуть игрок</param>
    /// <returns></returns>
    IEnumerator DoJump(float highJump)
    {
        //print("Start jump");
        //пока позиция игрока меньше требуемой высоты прыжка
        if (transform.position.y <= highJump)
        {
            //обнуление вектора гравитации
            //_gravity = Vector3.zero;
            //изменение позиции по оси Y
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
    /// Метод проверки на поверхности или нет персонаж
    /// </summary>
    private void IsGround()
    {
        //Проверка на нахожждение игрока на поверхности с указанной маской слоев
        bool checkGround = (Physics.CheckSphere(transform.position, _radiusGroundChecker, layerMaskGround));
        //Если игрок на земле, то установка флага _isGround = true
        //и установка минимальной гравитации для возможности плавного передвижения по наклонным поверкхностям
        //иначе добавление к вестору движения игрока вектора гравитации
        if (checkGround)IsGrounded = true;
        else IsGrounded = false;
    }

    /// <summary>
    /// Методу установки вектора гравитации в зависимости от значения параметра
    /// </summary>
    /// <param name="isGround"></param>
    private float SetGravityValue()
    {
        return (_isGround) ? _directionToMove.y += _gravityForce2 * 0.01f : _directionToMove.y += _gravityForce2;
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

    /// <summary>
    /// Передвижение игрока с входящим параметром - вектором направления движения
    /// </summary>
    /// <param name="newDirection"></param>
    /// <returns></returns>
    IEnumerator Move(Vector3 newDirection)
    {
        _cc.Move(newDirection);
        yield return null;
    }
}