using UnityEngine;
using System.Collections;

public class TargetPoint : MonoBehaviour {
    private int LinkCount;
    
    public void RemoveLink()
    {
        LinkCount--;
        if(LinkCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddLink()
    {
        LinkCount++;
    }
}
