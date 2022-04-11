using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 15f;  //Скорость поворота камеры
    private Camera _mainCamera;                       //Основная камера

    void Start()
    {
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }

    
    void FixedUpdate()
    {
        //Получение значение поворота камеры по оси y
        float yawCamera = _mainCamera.transform.eulerAngles.y;
        //Поворот игрока
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0f), _turnSpeed * Time.fixedDeltaTime);
    }
}
