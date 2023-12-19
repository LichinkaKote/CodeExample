using Assets.Scripts;
using Assets.Scripts.Core;
using Assets.Scripts.Core.PlayerSystems;
using Assets.Scripts.UI.Windows;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Prefabs prefabs;

    public static Game Instance { get; private set; }
    public static GameManager GameManager { get; private set; }
    public static Prefabs Prefabs { get; private set; }
    public static GameEvents Events { get; private set; }
    public static PalyerPersistentSystems Player { get; private set; }
    public static Library Library { get; private set; }
    public static SceneLoader SceneLoader { get; private set; }
    public static WindowController WindowController { get; private set; }


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitPersistentSystems();
        }
        else
            Destroy(gameObject);
    }
    private void InitPersistentSystems()
    {
        Prefabs = prefabs;
        Events = new GameEvents();
        SceneLoader = new SceneLoader();
        Library = new Library();
        GameManager = new GameManager(Prefabs.GameWorld);
        InitWindowController();
        Library.Load().Then(() =>
            {
                Player = new PalyerPersistentSystems();
            }).Catch(_ =>
            {
                Debug.LogError("Library.Load failed");
                Application.Quit();
            });
    }

    private void InitWindowController()
    {
        WindowController = Instantiate(prefabs.WindowController);
        DontDestroyOnLoad(WindowController.gameObject);
    }
}
