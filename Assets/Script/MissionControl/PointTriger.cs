using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTriger : MonoBehaviour
{
    [SerializeField] private int pointID;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Unit")
        {
            Unit unitNew = other.GetComponent<Unit>();
            if (unitNew != null)
            {
                if (unitNew.player == Players.Player1 && unitNew.live)
                {
                    ControlLevel1.Instance.EventControl(pointID);
                    Destroy(gameObject);
                }
            }
        }
    }

}
