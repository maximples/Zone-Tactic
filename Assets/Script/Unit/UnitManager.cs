using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

    private List<Unit> UnitsPlayer1;
    private List<Unit> UnitsPlayer2;
    private List<Unit> UnitsPlayer3;
    private List<Unit> UnitsAllies;
    public Unit SelectUnit;
    public static UnitManager Instance;
    public Material player1Mat;
    public Material player2Mat;
    public Material player3Mat;
    public GameObject SelectUnitPlayer;
    public GameObject SelectUnitEnemy;
    public GameObject SelectUnitAllies;
    public GameObject SelectBuildPlayer;
    public GameObject SelectBuildEnemy;
    public GameObject SelectBuildAllies;
    void Awake()
    {
        Instance = this;
        UnitsPlayer1 = new List<Unit>();
        UnitsPlayer2 = new List<Unit>();
        UnitsPlayer3 = new List<Unit>();
        UnitsAllies = new List<Unit>();
        //Units = new List<Unit>(GameObject.FindObjectsOfType<Unit>());
    }
    public Material GetUnitTexture(Players controller)
    {
        if(controller == Players.Player2)
        { return player2Mat; }
        if (controller == Players.Player3)
        { return player3Mat; }
        else
            return player1Mat;
    }

    public void AddUnit(Unit unit, Players controller)
    {
        if (controller == Players.Player1) 
        { 
            UnitsPlayer1.Add(unit);
            UnitsAllies.Add(unit);
        }
        if (controller == Players.Player2) { UnitsPlayer2.Add(unit); }
        if (controller == Players.Player3)
        { 
            UnitsPlayer3.Add(unit);
            UnitsAllies.Add(unit);
        }
    }

    public void RemoveUnit(Unit unit, Players controller)
    {
        if (controller == Players.Player1)
        {
            UnitsPlayer1.Remove(unit);
            UnitsAllies.Remove(unit);
        }
        if (controller == Players.Player2) { UnitsPlayer2.Remove(unit); }
        if (controller == Players.Player3)
        {
            UnitsPlayer3.Remove(unit);
            UnitsAllies.Remove(unit);
        }
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
            if (controller == Players.Player3)
            {
                return UnitsPlayer3.ToArray();
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
                return UnitsAllies.ToArray();
            }
            if (controller == Players.Player3)
            {
                return UnitsPlayer2.ToArray();
            }
        }
        return UnitsPlayer1.ToArray();
    }
    public Unit[] GetAllUnitsRepir(Players controller, Force getForce)
    {
        if (getForce == Force.Allies)
        {
            if (controller == Players.Player1)
            {
                return UnitsAllies.ToArray();
            }
            if (controller == Players.Player2)
            {
                return UnitsPlayer2.ToArray();
            }
            if (controller == Players.Player3)
            {
                return UnitsAllies.ToArray();
            }

        }
        return UnitsPlayer1.ToArray();
    }
    }
