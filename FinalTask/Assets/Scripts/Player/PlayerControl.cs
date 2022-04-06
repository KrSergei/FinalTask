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
<<<<<<< HEAD
        #region Control player
        _directionSide = Input.GetAxis("Horizontal");
        _directionForward = Input.GetAxis("Vertical");

        //Если нажата любая из клавиш управления (W, A, S, D), в управляющем анимацией скрипте
        //вызывается метод для изменения соответввующего значения в дереве анимаций Movement, с передачей параметра текущего значения
        if (_directionForward != 0 || _directionSide != 0)
        {
            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward * CheckingIsRunningState());
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide * CheckingIsRunningState());

            //Вычисление результирующего вектора движения: сумма двух векторов по оси x и z.
            //Векторы умножены на значение _directionForward и _directionSide соответсвенно. 
            //Итоговый суммарный вектор нормализован и умножен на значение скорости в зависимости от состояния (бег или шаг).
            _directionToMove = (((Vector3.forward * _directionForward)
                + ((Vector3.right * _directionSide))).normalized) 
                * CheckingIsRunningState() * Time.deltaTime;
        }
        else
        {

            playerAnimationConrtol.SetFloatValueDirection("Forward", _directionForward);
            playerAnimationConrtol.SetFloatValueDirection("Side", _directionSide);
            //Обнуление вектора передвижения
            _directionToMove = Vector3.zero;
        }
        #endregion
        //Запуск корутины передвижения с указанием результирующего вектора направления движения
=======
        _directionSide = Input.GetAxisRaw("Horizontal");
        _directionForward = Input.GetAxisRaw("Vertical");
        if (CheckingIsRunningState())
        {
            ////если нажата клавиша W, проигрывание анимации Player_RunForward
            if (_directionForward > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_FORWARD_PARAMETR);
                _directionToMove = Vector3.forward * _runSpeed;
            }
            //если нажата клавиша S, проигрывание анимации Player_RunBackward
            if (_directionForward < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_BACKWARD_PARAMETR);
                _directionToMove = -Vector3.forward * _runSpeed;
            }
            //если нажата клавиша D, проигрывание анимации Player_RunRight
            if (_directionSide > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_RIGHT_PARAMETR);
                _directionToMove = Vector3.right * _runSpeed;
            }
            //если нажата клавиша A, проигрывание анимации Player_RunLeft
            if (_directionSide < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_RUN_LEFT_PARAMETR);
                _directionToMove = -Vector3.right * _runSpeed;
            }
            //расчет вектора движения
            _directionToMove *= Time.deltaTime;
        }
        else
        {
            //если нажата клавиша W, проигрывание анимации Player_WalkForward
            if (_directionForward > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_FORWARD_PARAMETR);
                _directionToMove = Vector3.forward * _walkSpeed;
            }
            //если нажата клавиша S, проигрывание анимации Player_WalkBackward
            if (_directionForward < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_BACKWARD_PARAMETR);
                _directionToMove = -Vector3.forward * _walkSpeed;
            }
            //если нажата клавиша D, проигрывание анимации Player_WalkRight
            if (_directionSide > 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_RIGHT_PARAMETR);
                _directionToMove = Vector3.right * _walkSpeed;
            }
            //если нажата клавиша A, проигрывание анимации Player_WalkLeft
            if (_directionSide < 0)
            {
                playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_WALK_LEFT_PARAMETR);
                _directionToMove = -Vector3.right * _walkSpeed;
            }
        //расчет вектора движения
        _directionToMove *= Time.deltaTime;
        }
        //если скорость персонажа равна 0, проигрывание анимации IDLE
        if (_directionForward == 0 && _directionSide == 0)
        {
            playerAnimationConrtol.ChangeAnimationByParametr(PLAYER_IDLE_PARAMETR);
        }
>>>>>>> parent of ce50395 (Added BlenderTree and refactoring script)
        StartCoroutine(Move(_directionToMove));
    }


    /// <summary>
    /// Проверка на состояние бега, если нажата кнопка LeftShift, то включен режим шага. Возвращает значение скорости, на которое
    /// умножается итоговый вектор перемещения
    /// </summary>
    private bool CheckingIsRunningState()
    {
        return (Input.GetKey(KeyCode.LeftShift)) ? false : true;
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
