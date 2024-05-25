using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class WorldSpaceUIController : MonoBehaviour
{
    [SerializeField] private GameObject _damageIconPrefab;
    [SerializeField] private List<TMP_Text> textList = new List<TMP_Text>();
    private Transform _mainCameraTransform;
    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _mainCameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        var dealDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<MoveAndDamageSystem>();
        var spawnSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AssignTargetSystem>();

        spawnSystem.OnSpawn += DisplayHP;

        dealDamageSystem.OnDealDamage += DisplayDamageIcon;
        dealDamageSystem.OnMove += Move;
    }

    private void OnDisable()
    {
        if (World.DefaultGameObjectInjectionWorld == null) return;
        var dealDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<MoveAndDamageSystem>();
        var spawnSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AssignTargetSystem>();

        spawnSystem.OnSpawn -= DisplayHP;

        dealDamageSystem.OnDealDamage -= DisplayDamageIcon;
        dealDamageSystem.OnMove -= Move;
    }

    private void DisplayDamageIcon(float damageAmount, float3 startPosition, Entity entity)
    {
        foreach (TMP_Text text in textList)
        {
            if (entityManager.GetName(entity) == text.gameObject.name)
            {
                text.text = $"<color=black>{damageAmount.ToString()}</color>";

                //if the health rach 0 make the text empty
                if (damageAmount <= 0)
                {
                    text.text = "";
                }
            }
        }
    }

    private void DisplayHP(float3 startPosition, Entity entity)
    {
        var directionToCamera = (Vector3)startPosition - _mainCameraTransform.position;
        var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
        var newIcon = Instantiate(_damageIconPrefab, new Vector3(startPosition.x, startPosition.y + 1.5f, startPosition.z), rotationToCamera, transform);
        var newIconText = newIcon.GetComponent<TextMeshProUGUI>();
        textList.Add(newIconText);
        newIconText.name = entityManager.GetName(entity);
        newIconText.text = $"<color=black>{100}</color>";
    }

    private void Move(float3 Position, Entity entity)
    {
        foreach (TMP_Text text in textList)
        {
            if (entityManager.GetName(entity) == text.gameObject.name)
            {
                text.transform.position = new Vector3(Position.x, Position.y + 1.5f, Position.z);
            }
        }
    }
}
