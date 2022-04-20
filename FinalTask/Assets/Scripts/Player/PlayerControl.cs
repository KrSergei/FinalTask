using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Constants
    private const string VERTICAL_PARAMETR_BLEND_TREE = "Forward"; 
    private const string HORIZONTAL_PARAMETR_BLEND_TREE = "Side";
    #endregion

    public PlayerAnimationConrtol playerAnimationConrtol;   //Компонент аниматор
    public Transform mainCamera;                            //Трансформ основной камеры
    public LayerMask layerMaskGround;                       //Маска поверхности
    public float distanceToGround = 0.1f;                   //Расстояние до поверхности

    [SerializeField] private float _walkSpeed = 1f;         //Скорость передвижения при анимации ходьбы
    [SerializeField] private float _runSpeed = 3f;          //Скорость передвижения при анимации бега
    [SerializeField] private float _directionSide;          //Направлеине движения в стороны
    [SerializeField] private float _directionForward;       //Направлеине движения вперед
    [SerializeField] private float _gravityForce;           //Коэффициент силы тяжести
    [SerializeField] private Vector3 _directionToMove;      //Итоговый ветор направления движения
    [SerializeField] private bool _isGround;                //Флаг, находится игрок на земле или нет
    private CharacterController _cc;  
    private Vector3 _gravity;                               //Вектор гравитации
    private float _radiusCheckingGround = 0.25f;                    //Радиус сферы для проверки находится игрок на земле или нет

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

    void FixedUpdate()
    {
        #region Control player
        //Вызом метода по определению, на поверхности или нет игрок
        IsGround();

        _directionSide = Input.GetAxis("Horizontal");
        _directionForward = Input.GetAxis("Vertical");
  
        //Запуск корутины поворота игрока с учетом направления камеры
        StartCoroutine(Rotate());

        //Если нажата любая из клавиш управления (W, A, S, D), в управляющем анимацией скрипте
        //вызывается метод для изменения соответввующего значения в дереве анимаций Movement, с передачей параметра текущего значения
        if (_directionForward != 0 || _directionSide != 0)
        {
            playerAnimationConrtol.SetFloatValueDirection(VERTICAL_PARAMETR_BLEND_TREE, _directionForward * CheckingIsRunningState());
            playerAnimationConrtol.SetFloatValueDirection(HORIZONTAL_PARAMETR_BLEND_TREE, _directionSide * CheckingIsRunningState());

            //Вычисление результирующего вектора движения: сумма двух векторов по оси x и z, усноженной на угол поворота игрока на угол, равный поворота главной камеры
            //Векторы умножены на значение _directionForward и _directionSide соответсвенно. 
            //Итоговый суммарный вектор умножен на значение скорости в зависимости от состояния (бег или шаг).
            _directionToMove = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f)
                * ((Vector3.forward * _directionForward) + (Vector3.right * _directionSide))
                * CheckingIsRunningState() * Time.fixedDeltaTime;
        }
        else
        {
            //Обнуление вектора передвижения
            _directionToMove = Vector3.zero;
            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward);
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide);
        }
        #endregion

        //Добавление гравитации с учетом нахождения на поверхности
        _directionToMove += _gravity;
        ////Запуск корутины передвижения с указанием результирующего вектора направления движения
        StartCoroutine(Move(_directionToMove));
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
    /// Метод проверки на поверхности или нет персонаж
    /// </summary>
    private void IsGround()
    {
        //Проверка на нахожждение игрока на поверхности с указанной маской слоев
        bool checkGround = (Physics.CheckSphere(transform.position, _radiusCheckingGround, layerMaskGround));
        //Если игрок на земле, то установка флага _isGround = true
        //и установка минимальной гравитации для возможности плавного передвижения по наклонным поверкхностям
        //иначе добавление к вестору движения игрока вектора гравитации
        if (checkGround)_isGround = true;
        else _isGround = false;
        //Вызов метода установки вектора гравитации
        SetGravityValue(checkGround);
    }

    /// <summary>
    /// Методу установки вектора гравитации в зависимости от значения параметра
    /// </summary>
    /// <param name="isGround"></param>
    private void SetGravityValue(bool isGround)
    {
        if (isGround) _gravity = Physics.gravity * 0.01f; 
        else _gravity += Physics.gravity * Time.deltaTime * _gravityForce;
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