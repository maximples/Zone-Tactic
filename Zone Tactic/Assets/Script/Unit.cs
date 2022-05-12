using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour, ISelect {

    public TargetPoint Target = null;
     
    private UnityEngine.AI.NavMeshAgent _agent;
    public UnityEngine.AI.NavMeshAgent Agent
    {
        get
        {
            if(_agent == null)
            {
                _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            }
            return _agent;
        }
        set
        {
            _agent = value;
        }
    }

    private Vector3 targetPosition;

    void Start()
    {
        UnitManager.Instance.AddUnit(this);
    }

    void Update()
    {
        if(Target)
        {
            Agent.SetDestination(targetPosition);
        }
    }

    public void OnSelect(int num)
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void OnDeselect()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void OnSetTarget(TargetPoint target)
    {
        if(Target != null)
        {
            Target.RemoveLink();
        }
        Target = target;
        Target.AddLink();
        targetPosition = Target.transform.position;
        Agent.stoppingDistance = Random.Range(1.0F, 15.0F);
    }
}
