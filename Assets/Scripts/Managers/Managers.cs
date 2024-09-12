using UnityEngine;

public class Managers : Singleton<Managers>
{
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();

    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }

    void Start()
    {
        
    }

    void Update()
    {
        _input.Update();
    }
}
