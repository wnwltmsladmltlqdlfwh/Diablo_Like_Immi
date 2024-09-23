using Unity.VisualScripting;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    #region Contents
    GameManager _game = new GameManager();

    public static GameManager Game { get { return Instance._game; } }
    #endregion

    #region core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _sceneManagerEx = new SceneManagerEx();
    UIManager _ui = new UIManager();
    SoundManager _sound = new SoundManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._sceneManagerEx; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SoundManager Sound {  get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    #endregion

    void Awake()
    {
        Init();
    }

    void Update()
    {
        _input.Update();
    }

    static void Init()
    {
        Pool.Init();
        Sound.Init();
        Data.Init();
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Clear();
    }
}
