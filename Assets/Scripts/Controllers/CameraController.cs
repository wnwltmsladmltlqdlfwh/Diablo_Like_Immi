using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0f, 6f, -5f);

    [SerializeField]
    GameObject _player = null;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        // �÷��̾��� �����Ӱ� ���ÿ� ī�޶� �̵��ϹǷ�, �÷��̾ �̵� �� ī�޶� �����̵��� �Ѵ�.
        if (_mode == Define.CameraMode.QuterView)
        {
            RaycastHit hit;
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuterView;
        _delta = delta;
    }
}
