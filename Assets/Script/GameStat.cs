using UnityEngine;
using System.Collections;

public static class GameStat{

    public static ActionState actionState = ActionState.Free;
    public static Material player1Mat;
    public static Material player2Mat;
    public static int baseNumberPlaer1=0;
    public static int baseNumberPlaer2=0;
    public static string playerName="Игрок";
    public static bool activ = true;
    public static float player1money;
    public static float player1Incom;
    public static Technology player1Technology;
    public static Technology player2Technology;
}
public enum ActionState
{
    Free,
    Build
}
public enum Players
{
    Player1,
    Player2
}
public enum Force
{
    Allies,
    Enemies
}
public struct Technology
{
    public bool RSZO;
    public bool heavyFactory;
    public bool heavyTank;
    public bool fireTank;
    public bool towerMulti;

}