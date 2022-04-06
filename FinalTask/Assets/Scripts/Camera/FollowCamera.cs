using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
 
    [SerializeField] private Camera[] _camerasArray;
    [SerializeField] private Transform _playerTransform;   //трансформ игрока
    [SerializeField] private Camera _currentCamera;
    private Vector3 offset;
    private int _currentCameraIndex;


    void Start()
    {
        _camerasArray = GetComponentsInChildren<Camera>();
        _currentCameraIndex = 0;
        //_currentCamera = _camerasArray[_currentCameraIndex];
        SetInActiveNotUsesCameras();
        _playerTransform = FindObjectOfType<Player>().transform;
        //Получение значения
        offset = transform.position - _playerTransform.position;
    }


    public void ChangeCurrentCamera()
    {
        if(_currentCameraIndex == _camerasArray.Length - 1)
        {
            _currentCameraIndex = 0;
        }
        else
        {
            ++_currentCameraIndex;
        }
        
        _camerasArray[_currentCameraIndex].gameObject.SetActive(true);
         SetInActiveNotUsesCameras();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) ChangeCurrentCamera();

        //Перемещение камеры вслед за игроком
        transform.position = _playerTransform.position + offset;
    }

    private void SetInActiveNotUsesCameras()
    {
        for (int i = 0; i < _camerasArray.Length; i++)
        {
            if (i != _currentCameraIndex) _camerasArray[i].gameObject.SetActive(false);
        }
    }
}
