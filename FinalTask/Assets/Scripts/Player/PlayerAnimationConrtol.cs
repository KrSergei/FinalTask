using UnityEngine;

public class PlayerAnimationConrtol : MonoBehaviour
{
    public Animator _anim;
    public float offsetCancelAnimation;

    [SerializeField] private string _currentState;
    [SerializeField] private string _currentKeyName;


    private void Start()
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

    public void PlayAnimationByParametr(string nameParametr, bool value)
    {
        _anim.SetBool(nameParametr, value);
        //print("Current animation length - " + _anim.GetCurrentAnimatorStateInfo(0).length);
        Invoke("CancelCurrentParametr", _anim.GetCurrentAnimatorStateInfo(0).length);
    }

    public void PlayAnimationByParametr(string newParametr)
    {
        if (_currentKeyName.Equals(newParametr)) return;
        _anim.SetTrigger(newParametr);
        _currentKeyName = newParametr;
        //print("Current animation length - " + _anim.GetCurrentAnimatorStateInfo(0).length);
        //обнуление текущего имени параметра анимации после окончания
        Invoke("CancelCurrentParametr", _anim.GetCurrentAnimatorStateInfo(0).length);
    }

    /// <summary>
    /// Метод управления деревом анимации Movement.
    /// </summary>
    /// <param name="newParamentr">Имя параметра, в который передается значение</param>
    /// <param name="newValue">Устанавливаемое значение параметра</param>
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
