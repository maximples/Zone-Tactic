using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

    private List<Unit> UnitsPlayer1;
    private List<Unit> UnitsPlayer2;
    private List<Unit> AirUnitsPlayer1;
    private List<Unit> AirUnitsPlayer2;
    public Unit SelectUnit;
    public static UnitManager Instance;
    public Material player1Mat;
    public Material player2Mat;
    void Awake()
    {
        Instance = this;
        UnitsPlayer1 = new List<Unit>();
        UnitsPlayer2 = new List<Unit>();
        AirUnitsPlayer1 = new List<Unit>();
        AirUnitsPlayer2 = new List<Unit>();
        //Units = new List<Unit>(GameObject.FindObjectsOfType<Unit>());
    }
    public Material GetUnitTexture(Players controller)
    {
        if(controller == Players.Player2)
        { return player2Mat; }
        else
            return player1Mat;
    }

    public void AddAirUnit(Unit unit, Players controller)
    {
        if (controller == Players.Player1) { AirUnitsPlayer1.Add(unit); }
        if (controller == Players.Player2) { AirUnitsPlayer2.Add(unit); }
    }
    public void AddUnit(Unit unit, Players controller)
    {
        if (controller == Players.Player1) { UnitsPlayer1.Add(unit); }
        if (controller == Players.Player2) { UnitsPlayer2.Add(unit); }
    }

    public void RemoveUnit(Unit unit, Players controller)
    {
        if (controller == Players.Player1) { UnitsPlayer1.Remove(unit); }
        if (controller == Players.Player2) { UnitsPlayer2.Remove(unit); }

    }
    public void RemoveAirUnit(Unit unit, Players controller)
    {
        if (controller == Players.Player1) { AirUnitsPlayer1.Remove(unit); }
        if (controller == Players.Player2) { AirUnitsPlayer2.Remove(unit); }

    }

    public Unit[] GetAllUnits(Players controller,Force getForce)
    {
        if (getForce == Force.Allies)
        {
            if (controller == Players.Player1)
            {
                return UnitsPlayer1.ToArray();
            }
            if (controller == Players.Player2)
            {
                return UnitsPlayer2.ToArray();
            }
        }
        if (getForce == Force.Enemies)
        {
            if (controller == Players.Player1)
            {
                return UnitsPlayer2.ToArray();
            }
            if (controller == Players.Player2)
            {
                return UnitsPlayer1.ToArray();
            }
        }
        return UnitsPlayer1.ToArray();
    }
    public Unit[] GetAllAirUnits(Players controller, Force getForce)
    {
        if (getForce == Force.Allies)
        {
            if (controller == Players.Player1)
            {
                return AirUnitsPlayer1.ToArray();
            }
            if (controller == Players.Player2)
            {
                return AirUnitsPlayer2.ToArray();
            }
        }
        if (getForce == Force.Enemies)
        {
            if (controller == Players.Player1)
            {
                return AirUnitsPlayer2.ToArray();
            }
            if (controller == Players.Player2)
            {
                return AirUnitsPlayer1.ToArray();
            }
        }
        return AirUnitsPlayer1.ToArray();
    }
}
