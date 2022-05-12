using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour, ISelect {

    public TargetPoint Target;
    public List<Product> Products;
    public Queue<Product> ProductQueue = new Queue<Product>();

    private Product p = null;
    private float timer;

    public void Update()
    {
        if(p == null && ProductQueue.Count > 0)
        {
            p = ProductQueue.Dequeue();
            timer = p.ConstructTime;
        }
        if(p != null)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                GameObject _new = Instantiate(p.Prefab, transform.position, Quaternion.identity) as GameObject;
                if(Target != null)
                {
                    _new.SendMessage("OnSetTarget", Target);
                }
                p = null;
            }
        }
    }

    public void OnSelect(int num)
    {
        
    }

    public void OnDeselect()
    {
       
    }

    public void OnSetTarget(TargetPoint target)
    {
        if (Target != null)
        {
            Target.RemoveLink();
        }
        Target = target;
        Target.AddLink();
    }
}
