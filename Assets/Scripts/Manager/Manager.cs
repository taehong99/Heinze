using UnityEngine;

public static class Manager
{
    public static GameManager Game { get { return GameManager.Instance; } }
    public static DataManager Data { get { return DataManager.Instance; } }
    public static PoolManager Pool { get { return PoolManager.Instance; } }
    public static ResourceManager Resource { get { return ResourceManager.Instance; } }
    public static SceneManager Scene { get { return SceneManager.Instance; } }
    public static SoundManager Sound { get { return SoundManager.Instance; } }
    public static UIManager UI { get { return UIManager.Instance; } }
    public static PlayerManager Player { get { return PlayerManager.Instance; } }
    public static MouseManager Mouse { get { return MouseManager.Instance; } }
    public static EventManager Event { get { return EventManager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameManager.ReleaseInstance();
        MouseManager.ReleaseInstance();
        PlayerManager.ReleaseInstance();
        DataManager.ReleaseInstance();
        PoolManager.ReleaseInstance();
        ResourceManager.ReleaseInstance();
        SceneManager.ReleaseInstance();
        SoundManager.ReleaseInstance();
        UIManager.ReleaseInstance();
        EventManager.ReleaseInstance();

        GameManager.CreateInstance();
        MouseManager.CreateInstance();
        PlayerManager.CreateInstance();
        DataManager.CreateInstance();
        PoolManager.CreateInstance();
        ResourceManager.CreateInstance();
        SceneManager.CreateInstance();
        SoundManager.CreateInstance();
        EventManager.CreateInstance();
        UIManager.CreateInstance();
    }
}
