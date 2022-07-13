using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMissile : Unit
{
    public float reloading = 12f;
    public float minAttackRadius = 30;
    private bool isReloading = false;
    private bool attak = false;
    public GameObject Fire;
    private RayTarget rayTarget;
    void Awake()
    {
        randTime = Random.Range(0, 0.99f);
        GetObject();
    }
    void Start()
    {
        rayTarget = turrentLook.GetComponent<RayTarget>();
        damag = 30;
        idlePosition = transform.position;
        speed = 7;
        speedFire = 40;
        Agent.speed = speed;
        isBusy = false;
        cameraMain = GameObject.Find("Main Camera");
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        CurrentHealth = MaxHealth;
        GetColor();
        GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        turrent.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        gun.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        UnitManager.Instance.AddUnit(this, player);
    }

    void Update()
    {
        if (live && GameStat.activ)
        {
            switch (state)
            {

                case UnitState.Idle:
                    {
                        if (!attak)
                        {
                            turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, transform.eulerAngles.y, turrent.transform.eulerAngles.z);
                        }
                        if (Target != null)
                        {
                            Target.RemoveLink();
                            Target = null;
                        }
                    }
                    break;
                case UnitState.AgresivStand:
                    {
                        if (enemyTarget == null)
                        {
                            StartCoroutine(FindEnemy(1, agroRadius));
                        }
                        if (enemyTarget == null)
                        {
                            Agent.isStopped = false;
                            Agent.SetDestination(idlePosition);
                            state = UnitState.Idle;
                            break;
                        }
                        if (enemyTarget != null)
                        {
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > agroRadius * agroRadius)
                            {
                                StartCoroutine(FindEnemy(1, agroRadius));
                                if (enemyTarget == null)
                                {
                                    Agent.isStopped = false;
                                    Agent.SetDestination(idlePosition);
                                    state = UnitState.Idle;
                                    break;
                                }
                            }
                            if (!attak)
                            {
                                turrentLook.transform.LookAt(enemyTarget.transform.position);
                                turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, turrentLook.transform.eulerAngles.y, turrent.transform.eulerAngles.z);
                            }
                            if ( Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < attackRadius * attackRadius) 
                            {
                                    Agent.isStopped = true;
                                    if (!isReloading&& Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > minAttackRadius * minAttackRadius) { Attack(enemyTarget); }
                                if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < minAttackRadius * minAttackRadius)
                                {
                                    Agent.isStopped = false;
                                    Agent.SetDestination(idlePosition);
                                }
                            }
                        }
                    }
                    break;
                case UnitState.MoveTarget:
                    {
                        if (!attak)
                        {
                            Agent.SetDestination(targetPosition);
                            if (Vector3.Distance(transform.position, targetPosition) <= 3)
                            {
                                Agent.isStopped = true;
                                idlePosition = transform.position;
                                state = UnitState.AgresivStand;
                            }
                            turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, transform.eulerAngles.y, turrent.transform.eulerAngles.z);
                        }
                    }
                    break;
                case UnitState.MoveEnemy:
                    {
                        if (!attak)
                        {
                            if (comandTarget != null)
                            {
                                if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) > attackRadius * attackRadius)
                                {
                                    Agent.SetDestination(comandTarget.transform.position);
                                }
                                if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) <= attackRadius * attackRadius)
                                {
                                    Agent.isStopped = true;
                                    if (!attak)
                                    {
                                        turrentLook.transform.LookAt(comandTarget.transform.position);
                                        turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, turrentLook.transform.eulerAngles.y, turrent.transform.eulerAngles.z);
                                        if (!isReloading && Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) > minAttackRadius * minAttackRadius) { Attack(comandTarget); }
                                    } 
                                    if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) < minAttackRadius * minAttackRadius)
                                    {
                                        Agent.isStopped = false;
                                        Agent.SetDestination(idlePosition);
                                    }
                                }

                            }
                            else
                            {
                                if (comandTarget == null)
                                {
                                    Agent.isStopped = true;
                                    idlePosition = transform.position;
                                    state = UnitState.AgresivStand;
                                }

                            }
                        }
                    }
                    break;
                case UnitState.MoveFrend:
                    {
                        if (!attak)
                        {
                            if (comandTarget != null)
                            {
                                Agent.SetDestination(comandTarget.transform.position);
                                if (!attak)
                                {
                                    turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, transform.eulerAngles.y, turrent.transform.eulerAngles.z);
                                }
                            }
                            else
                            if (comandTarget == null)
                            {
                                Agent.isStopped = true;
                                idlePosition = transform.position;
                                state = UnitState.AgresivStand;
                            }
                        }
                    }
                    break;
                case UnitState.AttakTerritory:
                    {
                        if (enemyTarget == null)
                        {
                            if (!isBusy)
                            {
                                StartCoroutine(FindEnemy(1, agroRadius));
                            }
                            if (haveTarget)
                            {
                                haveTarget = false;
                                Agent.isStopped = false;
                                Agent.SetDestination(targetPosition);
                            }
                            IdleTurrent();
                        }
                        if (enemyTarget != null)
                        {
                            if (!haveTarget)
                            {
                                haveTarget = true;
                            }
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius)
                            {
                                if (!isBusy)
                                {
                                    StartCoroutine(FindEnemy(1, attackRadius));
                                }
                                Agent.isStopped = false;
                            }
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < attackRadius * attackRadius)
                            {
                                turrentLook.transform.LookAt(enemyTarget.transform.position);
                                turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, turrentLook.transform.eulerAngles.y, turrent.transform.eulerAngles.z);
                                if (rayTarget.GoodTarget(mask)&& Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > minAttackRadius * minAttackRadius)
                                {
                                    Agent.isStopped = true;
                                    if (!isReloading) { Attack(enemyTarget); }
                                }
                                if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < minAttackRadius * minAttackRadius)
                                {
                                    Agent.isStopped = false;
                                    Agent.SetDestination(idlePosition); }

                            }
                        }
                    }
                    break;

            }
        }

    }
    private void Attack(GameObject enemy)
    {
        attak = true;
        float time=0;
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(MissileFire(time, enemy.transform.position,i));
            time += 0.3f;
        }
        isReloading = true;
        StartCoroutine(Reloading());
    }
    public IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloading);
        isReloading = false;
    }
    public IEnumerator MissileFire(float time_,Vector3 targetPos,int ID)
    {
        yield return new WaitForSeconds(time_);
        GameObject fireGun = Instantiate(Fire, gun.transform.position, gun.transform.rotation) as GameObject;
        MissileGo missileGo = fireGun.GetComponent<MissileGo>();
        missileGo.damag = damag;
        missileGo.speed = speedFire;
        missileGo.playerControl = player;
        missileGo.targetPos = targetPos;
        if (ID == 5) attak = false;
    }
}

