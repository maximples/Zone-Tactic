using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirUnit : Unit
{
    public float reloading = 0.8f;
    private bool isReloading = false;
    public GameObject Fire;
    public GameObject gunPosition1;
    public GameObject gunPosition2;
    public RayTarget rayTarget;
    void Awake()
    {
        randTime = Random.Range(0, 0.99f);
    }
    void Start()
    {
        damag = 3;
        idlePosition = transform.position;
        speed = 15;
        speedFire = 40;
        Agent.speed = speed;
        isBusy = false;
        cameraMain = GameObject.Find("Main Camera");
        unitManager = cameraMain.GetComponent<UnitManager>();
        CurrentHealth = MaxHealth;
        GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player == Players.Player1)
        {
            mask = 191;
            gameObject.layer = 6;
        }
        if (player == Players.Player2)
        {
            mask = 127;
            gameObject.layer = 7;
        }
        if (player != Players.Player1) { selectionRing.GetComponent<MeshRenderer>().material.color = Color.red; }
        UnitManager.Instance.AddAirUnit(this, player);
    }

    void Update()
    {
        if (live && GameStat.activ)
        {
            switch (state)
            {

                case UnitState.Idle:
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
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius)
                            {
                                Agent.isStopped = false;
                                Agent.SetDestination(enemyTarget.transform.position);
                            }
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < attackRadius * attackRadius)
                            {
                                turrent.transform.LookAt(enemyTarget.transform.position);
                                if (rayTarget.GoodTarget(mask))
                                {
                                    Agent.isStopped = true;
                                    if (!isReloading) { Attack(); }
                                }
                                else
                                {
                                    Agent.SetDestination(enemyTarget.transform.position);
                                }
                            }
                            if (Vector3.SqrMagnitude(idlePosition - transform.position) > agroRadius * agroRadius * 1.4f)
                            {
                                Agent.isStopped = false;
                                Agent.SetDestination(idlePosition);
                                state = UnitState.Idle;
                                break;
                            }
                        }
                    }
                    break;
                case UnitState.MoveTarget:
                    {
                        if (enemyTarget != null)
                        {
                            turrent.transform.LookAt(enemyTarget.transform.position);
                            if (rayTarget.GoodTarget(mask))
                            {
                                if (!isReloading) { Attack(); }
                            }
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius)
                            {
                                enemyTarget = null;
                            }
                        }

                        Agent.SetDestination(targetPosition);
                        if (Vector3.Distance(transform.position, targetPosition) <= 3)
                        {
                            Agent.isStopped = true;
                            idlePosition = transform.position;
                            state = UnitState.AgresivStand;
                        }
                        if (enemyTarget == null)
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
                                StartCoroutine(FindEnemy(1, attackRadius));
                            }
                        }
                    }
                    break;
                case UnitState.MoveEnemy:
                    {
                        if (comandTarget != null)
                        {
                            if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) > attackRadius * attackRadius)
                            {
                                Agent.SetDestination(comandTarget.transform.position);
                            }
                            if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) < attackRadius * attackRadius)
                            {
                                enemyTarget = comandTarget;
                                Agent.isStopped = true;
                            }

                            if (enemyTarget != null)
                            {
                                turrent.transform.LookAt(enemyTarget.transform.position);
                                if (rayTarget.GoodTarget(mask))
                                {
                                    if (!isReloading) { Attack(); }
                                }
                                else
                                {
                                    Agent.SetDestination(enemyTarget.transform.position);
                                }
                                if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius)
                                {
                                    enemyTarget = null;
                                    Agent.isStopped = false;
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
                                }
                            }
                            if (enemyTarget == null)
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
                                    StartCoroutine(FindEnemy(1, attackRadius));
                                }
                            }
                        }
                        else
                        {
                            if (comandTarget == null)
                            {
                                _agent.ResetPath();
                                idlePosition = transform.position;
                                state = UnitState.AgresivStand;
                            }

                        }
                    }
                    break;
                case UnitState.MoveFrend:
                    {
                        if (comandTarget != null)
                        {
                            Agent.SetDestination(comandTarget.transform.position);
                            if (enemyTarget != null)
                            {
                                turrent.transform.LookAt(enemyTarget.transform.position);
                                if (rayTarget.GoodTarget(mask))
                                {
                                    Agent.isStopped = true;
                                    if (!isReloading) { Attack(); }
                                }
                                if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius)
                                {
                                    enemyTarget = null;
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
                                }
                            }
                            if (enemyTarget == null)
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
                                    StartCoroutine(FindEnemy(1, attackRadius));
                                }
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
                    break;
                case UnitState.AttakTerritory:
                    {
                        if (enemyTarget == null)
                        {
                            if (!isBusy)
                            {
                                StartCoroutine(FindEnemy(1, agroRadius));
                            }
                        }
                        if (enemyTarget == null)
                        {
                            Agent.isStopped = false;
                            Agent.SetDestination(targetPosition);
                            break;
                        }
                        if (enemyTarget != null)
                        {
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius)
                            {
                                if (!isBusy)
                                {
                                    StartCoroutine(FindEnemy(1, attackRadius));
                                }
                                Agent.isStopped = false;
                                Agent.SetDestination(enemyTarget.transform.position);
                            }
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < attackRadius * attackRadius)
                            {
                                turrent.transform.LookAt(enemyTarget.transform.position);
                                if (rayTarget.GoodTarget(mask))
                                {
                                    Agent.isStopped = true;
                                    if (!isReloading) { Attack(); }
                                }
                                else
                                {
                                    Agent.SetDestination(enemyTarget.transform.position);
                                }

                            }
                        }
                    }
                    break;
            }
        }

    }
    private void Attack()
    {
        GameObject fireGun = Instantiate(Fire, gunPosition1.transform.position, turrent.transform.rotation) as GameObject;
        FireGo fireGo = fireGun.GetComponent<FireGo>();
        fireGo.damag = damag;
        fireGo.speed = speedFire;
        fireGo.playerControl = player;
        GameObject fireGun_ = Instantiate(Fire, gunPosition2.transform.position, turrent.transform.rotation) as GameObject;
        FireGo fireGo_ = fireGun_.GetComponent<FireGo>();
        fireGo_.damag = damag;
        fireGo_.speed = speedFire;
        fireGo_.playerControl = player;
        isReloading = true;
        StartCoroutine(Reloading());
    }
    public IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloading);
        isReloading = false;
    }
    public override void GetDamag(int damag)
    {
        if (state == UnitState.Idle) { AgroFindEnemy(); }
        CurrentHealth = CurrentHealth - damag;
        if (CurrentHealth <= 0)
        {
            unitManager.RemoveAirUnit(this, player);
            if (this == UIManager.Instance.unit)
            { UIManager.Instance.OnDeselectUnit(); }
            live = false;
            if (state == UnitState.MoveTarget)
            {
                _agent.ResetPath();
            }
            Instantiate(destroyEffect, turrent.transform.position, turrent.transform.rotation);
            Destroy(gameObject);
        }
    }
}

