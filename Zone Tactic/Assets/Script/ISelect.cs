using UnityEngine;
using System.Collections;

public interface ISelect{

    void OnSelect(int num);
    void OnDeselect();
    void OnSetTarget(TargetPoint target);
}
