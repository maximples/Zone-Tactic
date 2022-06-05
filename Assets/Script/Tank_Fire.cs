using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Fire : Unit
{
    [SerializeField] private float reloading = 3;
    [SerializeField] private GameObject Fire;
    private bool isReloading = false;
    public RayTarget rayTarget;
    void Awake()
    {
        randTime = Random.Range(0, 0.99f);
    }
    void Start()
    {
        idlePosition = transform.position;
        speed = 8;
        speedFire = 20;
        Agent.speed = speed;
        isBusy = false;
        cameraMain = GameObject.Find("Main Camera");
        unitManager = cameraMain.GetComponent<UnitManager>();
        CurrentHealth = MaxHealth;
        GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        turrent.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        gun.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player != Players.Player1) { selectionRing.GetComponent<MeshRenderer>().material.color = Color.red; }
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
        GameObject fireGun = Instantiate(Fire, turrent.transform.position, turrent.transform.rotation) as GameObject;
        FireDamag fireGo = fireGun.GetComponent<FireDamag>();
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

}
