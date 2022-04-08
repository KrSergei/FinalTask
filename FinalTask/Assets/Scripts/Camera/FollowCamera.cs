using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
 
    [SerializeField] private Camera[] _camerasArray;       //массив камер
    [SerializeField] private Transform _playerTransform;   //трансформ игрока

    private Vector3 offset;                 //расстояние между точкой привязки камер (родительский объект для камер) и игроком
    private int _currentCameraIndex;        //индекс текущй камеры


    void Start()
    {
        //заполнение массива камер
        _camerasArray = GetComponentsInChildren<Camera>();
        //установка позиции стартовой камеры
        _currentCameraIndex = 0;
        //отключение неактивных камер
        SetInActiveNotUsesCameras();
        //поиска позиции игрока
        _playerTransform = FindObjectOfType<Player>().transform;
        //Получение значения расстояния до точки привязки камер до игрока
        offset = transform.position - _playerTransform.position;
    }

    /// <summary>
    /// Метод переключения текущей камеры и отключения неактивных камер
    /// </summary>
    public void ChangeCurrentCamera()
    {
        //если текущий инжекс камеры равен длине массива - 1, установка индекса декущей камеры равно 0, иначе инкремент индекса
        if(_currentCameraIndex == _camerasArray.Length - 1)
        {
            _currentCameraIndex = 0;
        }
        else
        {
            ++_currentCameraIndex;
        }
        //активация выбранной камеры по индексу текущей камеры
        _camerasArray[_currentCameraIndex].gameObject.SetActive(true);
        //вызов метода по деактивации не активных камер
         SetInActiveNotUsesCameras();
    }

    void Update()
    {
        //переключение камеры по нажатию клавиши C
        if (Input.GetKeyDown(KeyCode.C)) ChangeCurrentCamera();
        //Перемещение камеры вслед за игроком
        transform.position = _playerTransform.position + offset;
    }

    /// <summary>
    /// Метод деактивации неактивных камер
    /// </summary>
    private void SetInActiveNotUsesCameras()
    {
        for (int i = 0; i < _camerasArray.Length; i++)
        {
            //если значение i не равно текущему индексу камеры, то деактивация камеры с индексом i
            if (i != _currentCameraIndex) _camerasArray[i].gameObject.SetActive(false);
        }
    }
}
