using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laboratory : Build
{
    public Product[] products;
    public Queue<Product> ProductQueue = new Queue<Product>();
    private Product p = null;
    private float timer;
   
    void Start()
    {
        if (live)
        {
            BuildManager.Instance.AddBuild(this, player);
        }
        CurrentHealth = MaxHealth;
        if (player == Players.Player1)
        {
            gameObject.layer = 6;
        }
        if (player == Players.Player2)
        {
            gameObject.layer = 7;
        }
        mesh.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player != Players.Player1) { selectionRing.GetComponent<MeshRenderer>().material.color = Color.red; }
    }

    public void Update()
    {
        if (p == null)
        {
            if (select)
            {
                UIManager.Instance.BuildBar.value = 0;
                UIManager.Instance.BuildBarText.text = "";
            }
        }
        if (p == null && ProductQueue.Count > 0)
        {
            p = ProductQueue.Dequeue();
            UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
            timer = p.ConstructTime;
        }
        if (p != null)
        {
 
            if (select) { UIManager.Instance.BuildBar.value = (p.ConstructTime - timer) / p.ConstructTime * 100; }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                p = null;
            }
        }
    }
    public void BuildProduct(int ID)
    {
        ProductQueue.Enqueue(products[ID]);
        UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
    }
}
