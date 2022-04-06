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
    /// ����� ��� ��������� �������� ��������� (���������) �������� �� ����� ��������� ���������
    /// </summary>
    /// <param name="newState">��� ��������, ������� ���������� ��������</param>
    public void ChangeState(string newState)
    {
        if (newState == _currentState) return;
        if (_anim) _anim.Play("Base Layer." + newState, 0, 0.5f);
        _currentState = newState;
    }

    public void ChangeAnimationByParametr(string newParametr)
    {
        if (_currentKeyName.Equals(newParametr)) return;
        _anim.SetTrigger(newParametr);
        _currentKeyName = newParametr;
    }

    /// <summary>
    /// ����� ���������� ������� �������� Movement.
    /// </summary>
    /// <param name="newParamentr">��� ���������, � ������� ���������� ��������</param>
    /// <param name="newValue">��������������� �������� ���������</param>
    public void SetFloatValueDirection(string newParamentr, float newValue)
    {
        _anim.SetFloat(newParamentr, newValue);
    }
}
