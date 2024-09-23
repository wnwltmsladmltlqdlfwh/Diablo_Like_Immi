using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10;
    [SerializeField]
    float _attackRange = 2f;

    NavMeshAgent nma;

    public override void Init()
    {
        WolrdObjectType = Define.WolrdObject.Monster;

        _stat = gameObject.GetComponent<Stat>();

        nma = gameObject.GetOrAddComponent<NavMeshAgent>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance < _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        // 플레이어가 내 사정거리보다 가까우면 공격

        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)
            {
                nma.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            nma.SetDestination(_destPos);
            nma.speed = _stat.Speed;

            transform.rotation =
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat target = _lockTarget.GetComponent<Stat>();
            target.OnAttacked(_stat);

            if(target.Hp <= 0)
            {
                Managers.Game.Despawn(target.gameObject);
            }

            if(target.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if(distance <= _attackRange)
                {
                    State = Define.State.Skill;
                }
                else
                {
                    State = Define.State.Moving;
                }
            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }
    }

    protected override void UpdateDie()
    {

    }
}
