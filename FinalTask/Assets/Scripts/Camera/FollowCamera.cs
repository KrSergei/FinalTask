using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
 
    [SerializeField] private Camera[] _camerasArray;       //������ �����
    [SerializeField] private Transform _playerTransform;   //��������� ������

    private Vector3 offset;                 //���������� ����� ������ �������� ����� (������������ ������ ��� �����) � �������
    private int _currentCameraIndex;        //������ ������ ������


    void Start()
    {
        //���������� ������� �����
        _camerasArray = GetComponentsInChildren<Camera>();
        //��������� ������� ��������� ������
        _currentCameraIndex = 0;
        //���������� ���������� �����
        SetInActiveNotUsesCameras();
        //������ ������� ������
        _playerTransform = FindObjectOfType<Player>().transform;
        //��������� �������� ���������� �� ����� �������� ����� �� ������
        offset = transform.position - _playerTransform.position;
    }

    /// <summary>
    /// ����� ������������ ������� ������ � ���������� ���������� �����
    /// </summary>
    public void ChangeCurrentCamera()
    {
        //���� ������� ������ ������ ����� ����� ������� - 1, ��������� ������� ������� ������ ����� 0, ����� ��������� �������
        if(_currentCameraIndex == _camerasArray.Length - 1)
        {
            _currentCameraIndex = 0;
        }
        else
        {
            ++_currentCameraIndex;
        }
        //��������� ��������� ������ �� ������� ������� ������
        _camerasArray[_currentCameraIndex].gameObject.SetActive(true);
        //����� ������ �� ����������� �� �������� �����
         SetInActiveNotUsesCameras();
    }

    void Update()
    {
        //������������ ������ �� ������� ������� C
        if (Input.GetKeyDown(KeyCode.C)) ChangeCurrentCamera();
        //����������� ������ ����� �� �������
        transform.position = _playerTransform.position + offset;
    }

    /// <summary>
    /// ����� ����������� ���������� �����
    /// </summary>
    private void SetInActiveNotUsesCameras()
    {
        for (int i = 0; i < _camerasArray.Length; i++)
        {
            //���� �������� i �� ����� �������� ������� ������, �� ����������� ������ � �������� i
            if (i != _currentCameraIndex) _camerasArray[i].gameObject.SetActive(false);
        }
    }
}
