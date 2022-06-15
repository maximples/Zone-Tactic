using UnityEngine;
using System.Collections;

public class TargetPoint : MonoBehaviour {
    public int LinkCount;
   // private bool activ = true;
  //  void //Start()
   // {
       // StartCoroutine(Timer());
   // }
    public void RemoveLink()
    {
        LinkCount--;
        if(LinkCount <= 0)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        //if(activ)
      //  {
            transform.Rotate(Vector3.up);
        //}
    }
        public void AddLink()
    {
        LinkCount++;
    }
    /*private IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        GetComponent<MeshRenderer>().enabled = false;
        activ = false;
    }*/
}
