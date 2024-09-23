using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    enum CursorType
    {
        None,
        Attack,
        Hand
    }
    CursorType _cursorType = CursorType.None;

    Texture2D _attackCursor;
    Texture2D _handCursor;

    int _mask = (1 << (int)Define.Layer.Moveable) | (1 << (int)Define.Layer.Monster);

    void Start()
    {
        _attackCursor = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handCursor = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Attack)
                {
                    UnityEngine.Cursor.SetCursor(_attackCursor, new Vector2(_attackCursor.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.Hand)
                {
                    UnityEngine.Cursor.SetCursor(_handCursor, new Vector2(_handCursor.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
