using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabrica_M : Build
{
    public Product[] products;
    public Queue<Product> ProductQueue = new Queue<Product>();
    [SerializeField] private GameObject kran;
    [SerializeField] private GameObject mesh1;
    [SerializeField] private GameObject buildPos;
    private Product p = null;
    private float timer;
    private Unit unit;
    private GameObject newUnit;
    private bool left=true;
    private bool obstacleYes = false;
    private UnityEngine.AI.NavMeshObstacle obstacle;
    public bool work = false;

    void Start()
    {
        if (live)
        {
            BuildManager.Instance.AddBuild(this, player);
            if (player == Players.Player1)
            {
                GameObject mask_ = Instantiate(maskFog, transform.position, transform.rotation) as GameObject;
                mask_.transform.parent = transform;
            }
        }
        CurrentHealth = MaxHealth;
        GetColor();
        mesh1.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
    }

    public void Update()
    {
        if (GameStat.activ)
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
                work = true;
                p = ProductQueue.Dequeue();
                if (select && player == Players.Player1)
                {
                    UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
                }
                timer = p.ConstructTime;
                newUnit = Instantiate(p.Prefab, buildPos.transform.position, buildPos.transform.rotation) as GameObject;
                unit = newUnit.GetComponent<Unit>();
                unit.player = player;
                unit.GetColor();
                newUnit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                newUnit.GetComponent<Unit>().enabled = false;
                newUnit.GetComponent<BoxCollider>().enabled = false;
            }
            if (p != null)
            {
                if (left)
                {
                    kran.transform.Translate(Vector3.left * Time.deltaTime * 5);
                    if (kran.transform.localPosition.x < -3.4f) { left = false; }
                }
                if (!left)
                {
                    kran.transform.Translate(Vector3.right * Time.deltaTime * 5);
                    if (kran.transform.localPosition.x > 3.4f) { left = true; }
                }

                if (select && player == Players.Player1)
                {
                    UIManager.Instance.buildBar.value = (p.ConstructTime - timer) / p.ConstructTime * 100;
                    UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
                }
                timer -= Time.deltaTime;
                if (timer <= 1&& timer>0)
                {
                    if (!obstacleYes)
                    {
                        obstacle = newUnit.AddComponent<UnityEngine.AI.NavMeshObstacle>();
                        obstacleYes = true;
                    }
                    newUnit.transform.Translate(Vector3.forward * Time.deltaTime * 8);
                    
                }
                if (timer <= 0)
                {
                    obstacleYes = false;
                    Destroy(obstacle);
                    newUnit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                    newUnit.GetComponent<Unit>().enabled = true;
                    newUnit.GetComponent<BoxCollider>().enabled = true;
                    if (Target != null)
                    {

                        unit.OnSetTarget(Target, 1);
                    }
                    work = false;
                    p = null;
                }
            }
        }
    }
    public void BuildProduct(int ID)
    {
        ProductQueue.Enqueue(products[ID]);
        if (player == Players.Player1)
        {
            UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
        }
    }
    public override void GetDamag(int damag)
    {
        CurrentHealth = CurrentHealth - damag;
        if (CurrentHealth <= 0)
        {
            if (newUnit != null) { Destroy(newUnit); }
            BuildManager.Instance.RemoveUnit(this, player);
            if (this == UIManager.Instance.build)
            { UIManager.Instance.OnDeselectUnit(); }
            live = false;
            Instantiate(destroyBuild, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
