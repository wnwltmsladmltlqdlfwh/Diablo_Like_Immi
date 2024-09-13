using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressd = false;

    public void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) // UI 생성 후, 마우스가 UI 오브젝트를 눌렀으면 return;
            return;

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if(MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressd = true;
            }
            else
            {
                if (_pressd)
                    MouseAction.Invoke(Define.MouseEvent.Click);
                _pressd = false;
            }
        }
    }
}
