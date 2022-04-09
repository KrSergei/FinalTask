using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private Vector2 _turn;
    public float _sensivity = .5f;
    public GameObject _player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        _turn.x += Input.GetAxis("Mouse X") * _sensivity;
        _turn.y += Input.GetAxis("Mouse Y") * _sensivity;
        _player.transform.localRotation = Quaternion.Euler(0, _turn.x, 0);
        transform.localRotation = Quaternion.Euler(- _turn.y, _turn.x, 0);

    }
}
