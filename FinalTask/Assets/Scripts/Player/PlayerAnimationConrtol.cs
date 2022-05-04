using UnityEngine;

public class PlayerAnimationConrtol : MonoBehaviour
{
    public Animator _anim;
    public float offsetCancelAnimation;

    [SerializeField] private string _currentState;
    [SerializeField] private string _currentKeyName;
    [SerializeField] private float _timeCurrentAnimationJump;

    public float TimeCurrentAnimationJump { get => _timeCurrentAnimationJump; set => _timeCurrentAnimationJump = value; }

    private void Start()
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

    public void PlayAnimationByParametr(string nameParametr, bool value)
    {
        _anim.SetBool(nameParametr, value);
        //print("Current animation length - " + _anim.GetCurrentAnimatorStateInfo(0).length);
        TimeCurrentAnimationJump = _anim.GetCurrentAnimatorStateInfo(0).length;
        Invoke("CancelCurrentParametr", _anim.GetCurrentAnimatorStateInfo(0).length);
    }

    public void PlayAnimationByParametr(string newParametr)
    {
        if (_currentKeyName.Equals(newParametr)) return;
        _anim.SetTrigger(newParametr);
        _currentKeyName = newParametr;
        //print("Current animation length - " + _anim.GetCurrentAnimatorStateInfo(0).length);
        //��������� �������� ����� ��������� �������� ����� ���������
        Invoke("CancelCurrentParametr", _anim.GetCurrentAnimatorStateInfo(0).length);
    }

    /// <summary>
    /// ����� ���������� ������� �������� Movement.
    /// </summary>
    /// <param name="newParamentr">��� ���������, � ������� ���������� ��������</param>
    /// <param name="newValue">��������������� �������� ���������</param>
    public void SetFloatValueDirection(string newParamentr, float newValue)
    {
        _anim.SetFloat(newParamentr, newValue, 0.1f, Time.deltaTime);
    }

    private void CancelCurrentParametr()
    {
        _currentKeyName = "";
        GetComponent<PlayerControl>().CanselActiveState();
    }
}
