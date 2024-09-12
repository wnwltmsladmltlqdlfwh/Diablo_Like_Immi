using System;
using UnityEngine;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressd = false;

    public void Update()
    {
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
