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
                UIManager.Instance.buildBar.value = 0;
                UIManager.Instance.BuildBarText.text = "";
            }
        }
        if (p == null && ProductQueue.Count > 0)
        {

            p = ProductQueue.Dequeue();
            timer = p.ConstructTime;
        }
        if (p != null)
        {
 
            if (select) { UIManager.Instance.buildBar.value = (p.ConstructTime - timer) / p.ConstructTime * 100; }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if(p.Name== "Системы защиты") { GameStat.player1Technology.towerMulti = true; }
                if (p.Name == "Ракетные технологии") { GameStat.player1Technology.RSZO = true; }
                if (p.Name == "Тяжёлая промышленость") { GameStat.player1Technology.heavyFactory = true; }
                if (p.Name == "Тяжёлый танк") { GameStat.player1Technology.heavyTank = true; }
                p = null;
            }
        }
    }
    public void BuildProduct(int ID)
    {
        if (p == null)
        {
            ProductQueue.Enqueue(products[ID]);
        }
        else 
        {
            StopAllCoroutines();
            UIManager.Instance.Message("Исследование уже идёт");
        }
    }
}
