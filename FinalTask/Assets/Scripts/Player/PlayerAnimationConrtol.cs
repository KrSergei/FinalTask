using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationConrtol : MonoBehaviour
{
    Animator _anim;
    [SerializeField] private string _currentState;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Метод для изменения текущего состояния (включения) анимации по имени входящего параметра
    /// </summary>
    /// <param name="newState">Имя анимации, которую необходимо включить</param>
    public void ChangeState(string newState)
    {
        if (newState == _currentState) return;
        if (_anim) _anim.Play("Base Layer." + newState);
        _currentState = newState;

    }
}
