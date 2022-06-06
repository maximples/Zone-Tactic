using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laboratory : Build
{
    public Product[] products;
    public Queue<Product> ProductQueue = new Queue<Product>();
    public LabPanel labPanel;
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
                if(p.Name== "Системы защиты") 
                { 
                    GameStat.player1Technology.towerMulti = true;
                    labPanel.botton_0_0.SetActive(false);
                }
                if (p.Name == "Ракетные технологии")
                {
                    GameStat.player1Technology.RSZO = true;
                    labPanel.botton_0_1.SetActive(false);
                }
                if (p.Name == "Тяжёлая промышленость") 
                { 
                    GameStat.player1Technology.heavyFactory = true;
                    labPanel.botton_0_2.SetActive(false);
                }
                if (p.Name == "Тяжёлый танк")
                { 
                    GameStat.player1Technology.heavyTank = true;
                    labPanel.botton_0_3.SetActive(false);
                }
                p = null;
            }
        }
    }
    public void BuildProduct(int ID)
    {
        if (p == null)
        {
            GameStat.player1money -= products[ID].price;
            ProductQueue.Enqueue(products[ID]);
        }
        else 
        {
            Debug.Log(0);
            StopAllCoroutines();
            StartCoroutine(UIManager.Instance.Message("Исследование уже идёт"));
        }
    }
}
