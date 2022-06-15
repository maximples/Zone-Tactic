using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionControl : MonoBehaviour
{
    public GameObject defensFlag;
    public GameObject destroyFlag;


    // Update is called once per frame
    void Update()
    {
        if (GameStat.activ)
            if (defensFlag== null)
            {
                GameManager.Instance.Win("Поражение");
            }
            if (destroyFlag == null)
            {
                GameManager.Instance.Win("Победа");
            }
    }
}
