using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToverMulti : Build
{
    public enum ToverState
    {
        Idle,
        AttakTarget

    }
    public ToverState state;
    public int damag = 3;
    [SerializeField] private float attackRadius = 50;
    [SerializeField] private GameObject enemyTarget;
    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject gunFire;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject turrent;
    [SerializeField] private GameObject turrentLook;
    private float speedFire = 40;
    private float reloading = 0.3f;
    private float randTime=0;
    private GameObject cameraMain;
    private bool isBusy = false;
    private bool isReloading = false;
    private UnitManager unitManager;
    private int mask;
    public RayTarget rayTarget;
    void Start()
    {
        randTime = Random.Range(0, 0.99f);
        state = ToverState.Idle;
        if (live)
        {
            BuildManager.Instance.AddBuild(this, player);
            if (player == Players.Player1)
            {
                GameObject mask_ = Instantiate(maskFog, transform.position, transform.rotation) as GameObject;
                mask_.transform.parent = transform;
            }
        }
        GetColor();
        isBusy = false;
        cameraMain = GameObject.Find("Main Camera");
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        CurrentHealth = MaxHealth;
        turrent.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player == Players.Player1)
        {
            mask = 191;
        }
        if (player == Players.Player2)
        {
            mask = 127;
        }
    }

    void Update()
    {
        if (live && GameStat.activ&&!building)
        {
            switch (state)
            {

                case ToverState.Idle:
                    {
                        if (turrent.transform.localEulerAngles.y > 2)
                        {

                            if (turrent.transform.localEulerAngles.y > 180)
                            {
                                turrent.transform.Rotate(Vector3.up);
                            }
                            else
                            {
                                turrent.transform.Rotate(Vector3.down);
                            }
                        }
                        if (!isBusy)
                        {
                            StartCoroutine(FindEnemy(3, attackRadius));
                        }
                        if(enemyTarget!=null)
                        {
                            state = ToverState.AttakTarget;
                        }
                    }
                    break;
                case ToverState.AttakTarget:
                    {
                        if (enemyTarget != null)
                        {
                            LookTarget(enemyTarget.transform.position);
                            if (rayTarget.GoodTarget(mask))
                            {


                                if (!isReloading && Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < attackRadius * attackRadius) { Attack(); }
                            }
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius) { enemyTarget = null; }
                        }
                        if (enemyTarget== null)
                        {
                            if (!isBusy)
                            {
                                FindEnemy(1, attackRadius);
                            }
                            if (enemyTarget == null)
                            {
                                state = ToverState.Idle;
                            }
                        }
                    }
                    break;
            }
        }
    }
    public IEnumerator FindEnemy(float time, float radius)
    {
        isBusy = true;
        foreach (Unit unit in unitManager.GetAllUnits(player, Force.Enemies))
        {
            if (Vector3.SqrMagnitude(unit.gameObject.transform.position - transform.position) < radius * radius)
            {
                enemyTarget = unit.gameObject;
                unit.AgroFindEnemy();
                break;
            }
        }
        if (enemyTarget == null)
        {
            foreach (Build build in BuildManager.Instance.GetAllBuilds(player, Force.Enemies))
            {
                if (Vector3.SqrMagnitude(build.gameObject.transform.position - transform.position) < radius * radius)
                {
                    enemyTarget = build.gameObject;
                    break;
                }
            }
        }
        yield return new WaitForSeconds(time + randTime);
        isBusy = false;
    }
    private void Attack()
    {
        GameObject fireGun = Instantiate(Fire, gunFire.transform.position, turrentLook.transform.rotation) as GameObject;
        FireGo fireGo = fireGun.GetComponent<FireGo>();
        fireGo.damag = damag;
        fireGo.speed = speedFire;
        fireGo.playerControl = player;
        isReloading = true;
        StartCoroutine(Reloading());
    }
    public IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloading);
        isReloading = false;
    }
    public void LookTarget(Vector3 targetPos)
    {
        turrentLook.transform.LookAt(targetPos);
        turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, turrentLook.transform.eulerAngles.y, turrent.transform.eulerAngles.z);
        gun.transform.eulerAngles = new Vector3(turrentLook.transform.eulerAngles.x, gun.transform.eulerAngles.y, gun.transform.eulerAngles.z);
    }
}
