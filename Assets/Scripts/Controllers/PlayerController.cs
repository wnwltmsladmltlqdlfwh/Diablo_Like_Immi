using System;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Moveable) | (1 << (int)Define.Layer.Monster);

    PlayerStat _stat;
    bool _stopSkill = false;

    NavMeshAgent nma;

    public override void Init()
    {
        WolrdObjectType = Define.WolrdObject.Player;

        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        nma = gameObject.GetOrAddComponent<NavMeshAgent>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null) 
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    void OnHitEvent()
    {
        if(_lockTarget != null)
        {
            Stat target = _lockTarget.GetComponent<Stat>();
            target.OnAttacked(_stat);
        }

        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    protected override void UpdateDie()
    {
        //사망
    }

    protected override void UpdateMoving() // 이동
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if(_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 2f)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_stat.Speed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            //transform.position += dir.normalized * moveDist;

            transform.rotation =
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        
    }

    protected override void UpdateIdle()
    {

    }

    protected override void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
        
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}
