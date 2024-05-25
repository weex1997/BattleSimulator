using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "Scriptable Object/Grid Data", order = 1)]
public class GridConfiguration : ScriptableObject
{
    public int xGridCount;
    public int zGridCount;
    public float baseOffset;
    public float xPadding;
    public float zPadding;
    public GameObject gridPrefab;

    public Vector3 teamBlueUnitPostion;
    public Vector3 teamRedUnitPostion;

    public GameObject teamBlueUnitPrefab;
    public GameObject teamRedUnitPrefab;

}
