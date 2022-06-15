using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour, ISelect {
    public TipBuild tipBuild;
    public GameObject maskFog;
    public float MaxHealth = 100;
    public float CurrentHealth;
    public float buildTime;
    [HideInInspector] public float transfom = 0;
    public string nameUnit;
    [HideInInspector] public bool live = true;
    [HideInInspector] public bool building = false;
    [HideInInspector] public bool select = false;
    public Players player = Players.Player1;
    public Sprite image;
    [HideInInspector] public TargetPoint Target;
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

    public virtual void OnSetTarget(TargetPoint target, int num)
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
    public void GetColor()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        mesh.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player == Players.Player1)
        {
            GameObject marker = Instantiate(UnitManager.Instance.SelectBuildPlayer, transform.position + offset, UnitManager.Instance.SelectBuildPlayer.transform.rotation) as GameObject;
            marker.transform.parent = transform;
            gameObject.layer = 6;
        }
        else
        {
            Projector myProjector = selectionRing.GetComponent<Projector>();
            myProjector.material = GameManager.Instance.player2Material;
            GameObject marker = Instantiate(UnitManager.Instance.SelectBuildEnemy, transform.position + offset, UnitManager.Instance.SelectBuildEnemy.transform.rotation) as GameObject;
            marker.transform.parent = transform;
            gameObject.layer = 7;
        }
    }
}
