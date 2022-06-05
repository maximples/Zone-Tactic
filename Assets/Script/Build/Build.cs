using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour, ISelect {

    public float MaxHealth = 100;
    public float CurrentHealth;
    public float buildTime;
    public float transfom = 0;
    public string nameUnit;
    public bool live = true;
    public bool building = false;
    public bool select = false;
    public Players player = Players.Player1;
    public Sprite image;
    public TargetPoint Target;
    [SerializeField] protected GameObject selectionRing;
    [SerializeField] protected GameObject destroyBuild;
    [SerializeField] protected GameObject mesh;
    [SerializeField] protected GameObject HpBar;
        public void OnSelect(int num)
    {
        selectionRing.SetActive(true);
        HpBar.SetActive(true);
        select=true;
    }

    public void OnDeselect()
    {
        if (live)
        {
            selectionRing.SetActive(false);
            HpBar.SetActive(false);
            select=false;
        }
    }

    public void OnSetTarget(TargetPoint target, int num)
    {
        if (Target != null)
        {
            Target.RemoveLink();
        }
        Target = target;
        Target.AddLink();
    }
    public virtual void GetDamag(int damag)
    {
        CurrentHealth = CurrentHealth - damag;
        if (CurrentHealth <= 0)
        {
            BuildManager.Instance.RemoveUnit(this, player);
            if (this == UIManager.Instance.build)
            { UIManager.Instance.OnDeselectUnit(); }
            live = false;
            Instantiate(destroyBuild, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    public IEnumerator SelectRingFlip(Build build)
    {
        if (build.live)
        {
            build.selectionRing.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        if (build!=null)
        {
            build.selectionRing.SetActive(false);
        }
    }

}
