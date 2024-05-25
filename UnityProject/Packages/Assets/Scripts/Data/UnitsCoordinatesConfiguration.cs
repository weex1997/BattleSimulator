
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lineup", menuName = "Scriptable Object/Units Coordinates", order = 2)]
public class UnitsCoordinatesConfiguration : ScriptableObject
{
    public List<Vector2> Coordinates = new List<Vector2>();
}
