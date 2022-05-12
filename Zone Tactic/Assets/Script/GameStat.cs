using UnityEngine;
using System.Collections;

public static class GameStat{

    public static ActionState actionState = ActionState.Free;

}
public enum ActionState
{
    Free,
    Build
}