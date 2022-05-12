using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

    private List<Unit> Units;

    public static UnitManager Instance; 

    void Awake()
    {
        Instance = this;
        Units = new List<Unit>();
        //Units = new List<Unit>(GameObject.FindObjectsOfType<Unit>());
    }

    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
    }

    public Unit[] GetAllUnits()
    {
        return Units.ToArray();
    }
}
