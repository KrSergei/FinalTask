using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationConrtol : MonoBehaviour
{
    Animator _anim;
    [SerializeField] private string _currentState;
    [SerializeField] private string _currentKeyName;
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
        if (_anim) _anim.Play("Base Layer." + newState, 0, 0.5f);
        _currentState = newState;
    }

    public void ChangeAnimationByParametr(string newParametr)
    {
        if (_currentKeyName.Equals(newParametr)) return;
        _currentKeyName = newParametr;
        _anim.SetTrigger(newParametr);
        Debug.Log("_currentParametr = " + _currentKeyName);
    }
    public void ChangeAnimationByBoolParametr(string newParametr, bool state)
    {
        if (_currentKeyName == newParametr) return;
        _currentKeyName = newParametr;
        _anim.SetBool(newParametr, state);
        Debug.Log("_currentKeyName = " + _currentKeyName);
    }
}
