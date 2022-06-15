using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Fire : Unit
{
    [SerializeField] private float reloading = 3;
    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject FirePos;
    private bool isReloading = false;
    private RayTarget rayTarget;
    void Awake()
    {
        randTime = Random.Range(0, 0.99f);
        GetObject();
    }
    void Start()
    {
        rayTarget = gun.GetComponent<RayTarget>();
        idlePosition = transform.position;
        speed = 8;
        speedFire = 20;
        Agent.speed = speed;
        isBusy = false;
        cameraMain = GameObject.Find("Main Camera");
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        CurrentHealth = MaxHealth;
        GetColor();
        if (player == Players.Player1)
        {
            GameObject mask_ = Instantiate(maskFog, transform.position, transform.rotation) as GameObject;
            mask_.transform.parent = transform;
        }
        if (player != Players.Player1)
        {
            Projector myProjector = selectionRing.GetComponent<Projector>();
            myProjector.material = GameManager.Instance.player2Material;
        }
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
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) > attackRadius * attackRadius)
                            {
                                Agent.isStopped = false;
                                Agent.SetDestination(enemyTarget.transform.position);
                            }
                            if (Vector3.SqrMagnitude(enemyTarget.transform.position - transform.position) < attackRadius * attackRadius)
                            {
                                LookTarget(enemyTarget.transform.position);
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
                            LookTarget(enemyTarget.transform.position);
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
                                LookTarget(enemyTarget.transform.position);
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
                                LookTarget(enemyTarget.transform.position);
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
                                LookTarget(enemyTarget.transform.position);
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
        GameObject fireGun = Instantiate(Fire, FirePos.transform.position, gun.transform.rotation) as GameObject;
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
    public void LookTarget(Vector3 targetPos)
    {
        targetPos = targetPos + offset;
        turrentLook.transform.LookAt(targetPos);
        turrent.transform.eulerAngles = new Vector3(turrent.transform.eulerAngles.x, turrentLook.transform.eulerAngles.y, turrent.transform.eulerAngles.z);
        gun.transform.eulerAngles = new Vector3(turrentLook.transform.eulerAngles.x, gun.transform.eulerAngles.y, gun.transform.eulerAngles.z);
    }

}
