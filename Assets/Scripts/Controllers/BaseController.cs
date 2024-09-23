using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Define.State _state = Define.State.Idle;
    protected virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1F);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1F);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1F, -1, 0);
                    break;
            }
        }
    }

    public Define.WolrdObject WolrdObjectType { get; protected set; } = Define.WolrdObject.Unknown;

    [SerializeField]
    protected Vector3 _destPos;

    [SerializeField]
    protected GameObject _lockTarget;

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }
    public abstract void Init();
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateSkill() { }
}
