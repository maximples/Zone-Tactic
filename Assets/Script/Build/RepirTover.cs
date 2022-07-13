using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepirTover : Build
{
    public enum BuildState
    {
        Idle,
        Repir
    }
    public float repireRadius = 15;
    [SerializeField] private GameObject Kran;
    private GameObject repireTarget;
    [SerializeField] private ParticleSystem repireEffect;
    [SerializeField] private GameObject repirePos;
    [SerializeField] private GameObject look;
    private Unit unitTarget;
    public BuildState state=BuildState.Idle;
    private bool isBusy = false;
    private bool repire = false;
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
            BoxCollider box = GetComponent<BoxCollider>();
            box.size = new Vector3(3, 5, 3);      }
        GetColor();
        CurrentHealth = MaxHealth;
        Kran.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
    }
    private void Update()
    {
        if (live && GameStat.activ&!building)
        {
            switch (state)
            {

                case BuildState.Idle:
                    {
                        if (!isBusy)
                        {
                            StartCoroutine(FindTarget(3, repireRadius));
                        }
                    }
                    break;
                case BuildState.Repir:
                    {
                        if (repireTarget != null)
                        {
                            if (Vector3.SqrMagnitude(repireTarget.transform.position - transform.position) > repireRadius * repireRadius)
                            {
                                repireTarget = null;
                                unitTarget = null;
                                state = BuildState.Idle;
                                break;
                            }
                            else
                            {
                                look.transform.LookAt(repireTarget.transform.position);
                                Kran.transform.eulerAngles = new Vector3(Kran.transform.eulerAngles.x, look.transform.eulerAngles.y, Kran.transform.eulerAngles.z);
                                if (!repire)
                                {
                                    StartCoroutine(repearStep());
                                }
                            }

                        }
                        if (repireTarget == null)
                        {
                            state = BuildState.Idle;
                        }
                    }
                    break;
            }
        }
    }

    public override void OnSetTarget(TargetPoint target, int num)
    {

    }
    private IEnumerator FindTarget(float time, float radius)
    {
        isBusy = true;
        foreach (Unit unit in UnitManager.Instance.GetAllUnitsRepir(player, Force.Allies))
        {
            if (unit.live)
            {
                if (Vector3.SqrMagnitude(unit.gameObject.transform.position - transform.position) < radius * radius)
                {
                    if (unit.CurrentHealth < unit.MaxHealth)
                    {
                        repireTarget = unit.gameObject;
                        unitTarget = unit;
                        state = BuildState.Repir;
                        break;
                    }
                }
            }
        }
        yield return new WaitForSeconds(time);
        isBusy = false;
    }
    public IEnumerator repearStep()
    {
        repire = true;
        Instantiate(repireEffect, repirePos.transform.position, transform.rotation);
        unitTarget.CurrentHealth =Mathf.Round( unitTarget.CurrentHealth +unitTarget.MaxHealth / unitTarget.buildingTime);
        if (unitTarget.CurrentHealth >= unitTarget.MaxHealth)
        {
            unitTarget.CurrentHealth = unitTarget.MaxHealth;
            repireTarget = null;
            unitTarget = null;
            state = BuildState.Idle;
        }
        yield return new WaitForSeconds(0.8f);
        repire = false;
    }
}

