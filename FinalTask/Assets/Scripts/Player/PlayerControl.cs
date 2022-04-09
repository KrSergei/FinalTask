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


        //≈сли нажата люба€ из клавиш управлени€ (W, A, S, D), в управл€ющем анимацией скрипте
        //вызываетс€ метод дл€ изменени€ соответввующего значени€ в дереве анимаций Movement, с передачей параметра текущего значени€
        if (_directionForward != 0 || _directionSide != 0)
        {
            playerAnimationConrtol.SetFloatValueDirection(VERTICAL_PARAMETR_BLEND_TREE, _directionForward * CheckingIsRunningState());
            playerAnimationConrtol.SetFloatValueDirection(HORIZONTAL_PARAMETR_BLEND_TREE, _directionSide * CheckingIsRunningState());

            //¬ычисление результирующего вектора движени€: сумма двух векторов по оси x и z.
            //¬екторы умножены на значение _directionForward и _directionSide соответсвенно. 
            //»тоговый суммарный вектор нормализован и умножен на значение скорости в зависимости от состо€ни€ (бег или шаг).
            _directionToMove = (((Vector3.forward * _directionForward) + ((Vector3.right * _directionSide))).normalized) * CheckingIsRunningState() * Time.deltaTime;
        }
        else
        {
            //ќбнуление вектора передвижени€
            _directionToMove = Vector3.zero;
            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward);
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide);
        }
        #endregion
        //«апуск корутины поворота игрока с учетом направлеин€ камеры
        //StartCoroutine(Rotate(_directionToMove));

        //«апуск корутины передвижени€ с указанием результирующего вектора направлени€ движени€
        StartCoroutine(Move(_directionToMove));

    }

    /// <summary>
    /// ѕроверка на состо€ние бега, если нажата кнопка LeftShift, то включен режим шага. ¬озвращает значение скорости, на которое
    /// умножаетс€ итоговый вектор перемещени€
    /// </summary>
    private float CheckingIsRunningState()
    {
        return (Input.GetKey(KeyCode.LeftShift)) ? _runSpeed : _walkSpeed;
    }

    IEnumerator Rotate(Vector3 newDirection)
    {
        #region var1 Don't working normal
        ////вычисление угола поворота
        //float targetAngle = Mathf.Atan2(newDirection.x, Mathf.Abs(newDirection.z)) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        ////вычисление угла поворота дл€ постепенного поворота с учетом сокрости поворота и времени
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        ////поворот игрока
        //transform.rotation = Quaternion.Euler(0f, angle, 0f);
        ////вычислеине нового вектора движени€  с учетом поворота игрока
        //_directionToMove = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        #endregion+

        yield return null;
    }

    /// <summary>
    /// ѕередвижение игрока с вход€щим параметром - вектором направлени€ движени€
    /// </summary>
    /// <param name="newDirection"></param>
    /// <returns></returns>
    IEnumerator Move(Vector3 newDirection)
    {
        _cc.Move(newDirection);
        yield return null;
    }
}