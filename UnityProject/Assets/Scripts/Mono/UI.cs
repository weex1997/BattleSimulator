using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Button startButton;
    public Button restartButton;
    public GameObject buttonPrefab;
    public GameObject ScrollContent;
    public GameObject EndScreenObject;
    public TMP_Text TeamWinText;
    public bool startGame = false;
    public int numbersOfLineups;
    List<GameObject> LineupsButtons = new List<GameObject>();
    private static UI _instance;
    public static UI Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(LevelManager.Instance.RestartGame);

        for (int i = 0; i < numbersOfLineups; i++)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            buttonObject.transform.parent = ScrollContent.transform;
            buttonObject.transform.position = new Vector3(0, 0, 0);
            buttonObject.transform.localScale = ScrollContent.transform.localScale;
            buttonObject.GetComponentInChildren<TMP_Text>().text = "Team " + (i + 1);
            int cachedIndex = i;
            Button button = buttonObject.GetComponent<Button>();
            LineupsButtons.Add(buttonObject);
            button.onClick.AddListener(delegate { ChooseLineup(cachedIndex); });
        }

    }

    void ChooseLineup(int _index)
    {
        Debug.Log("" + _index);
        var dealDamageSystem33 = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnSystem>();
        dealDamageSystem33.isChange = true;
        dealDamageSystem33.lineupIndex = _index;

    }
    void StartGame()
    {
        startGame = true;
        var AssignTargetSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AssignTargetSystem>();
        AssignTargetSystem.StartButtel = true;
        Debug.Log(startGame);
        startButton.gameObject.SetActive(false);
        foreach (GameObject button in LineupsButtons)
        {
            button.SetActive(false);
        }
    }
    public void EndScreen()
    {
        EndScreenObject.SetActive(true);

    }

}
