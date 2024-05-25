using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public SubScene subScenes;
    public bool StartTheLevel = false;
    Entity SceneEntity;
    bool endLoad;

    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

    }
    void Start()
    {

        SceneEntity = SceneSystem.GetSceneEntity(World.DefaultGameObjectInjectionWorld.Unmanaged, subScenes.SceneGUID);
        endLoad = false;

    }
    void Update()
    {
        if (SceneSystem.IsSceneLoaded(World.DefaultGameObjectInjectionWorld.Unmanaged, SceneEntity) && !endLoad)
        {
            StartTheLevel = true;
            Debug.Log("the subscene loaded");
            endLoad = true;
        }
    }

    public void RestartGame()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityManager.DestroyEntity(entityManager.UniversalQuery);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

}