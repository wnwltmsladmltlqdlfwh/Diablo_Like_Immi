using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    GameObject _player;
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WolrdObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch(type)
        {
            case Define.WolrdObject.Monster:
                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WolrdObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.WolrdObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if(bc == null)
        {
            return Define.WolrdObject.Unknown;
        }

        return bc.WolrdObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WolrdObject type = GetWorldObjectType(go);

        switch(type)
        {
            case Define.WolrdObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(-1);
                    }
                }
                break;
            case Define.WolrdObject.Player:
                {
                    if( _player == go)
                    {
                        _player = null;
                    }
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
