using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    [SerializeField] private ObjectSelect objectSelect;
    [SerializeField] private UIManager uiManager;
    private List<ISelect> group1 = new List<ISelect>();
    private List<ISelect> group2 = new List<ISelect>();
    private List<ISelect> group3 = new List<ISelect>();
    private List<ISelect> group4 = new List<ISelect>();
    private List<ISelect> group5 = new List<ISelect>();
    private bool[] groupYes = new bool[5];
    // Start is called before the first frame update
    void Start()
    {
        groupYes[0] = false; groupYes[1]=false; groupYes[2]=false; groupYes[3]=false; groupYes[4]=false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Alpha1)&& Input.GetKey(KeyCode.LeftShift))
        {
            AddGroup(group1,0);
        }
        if (Input.GetKey(KeyCode.Alpha1)&&groupYes[0])
        {
            SelectGroup(group1);
        }
        if (Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftShift))
        {
            AddGroup(group2,1);
        }
        if (Input.GetKey(KeyCode.Alpha2) && groupYes[1])
        {
            SelectGroup(group2);
        }
        if (Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftShift))
        {
            AddGroup(group3,2);
        }
        if (Input.GetKey(KeyCode.Alpha3) && groupYes[2])
        {
            SelectGroup(group3);
        }
        if (Input.GetKey(KeyCode.Alpha4) && Input.GetKey(KeyCode.LeftShift))
        {
            AddGroup(group4,3);
        }
        if (Input.GetKey(KeyCode.Alpha4) && groupYes[3])
        {
            SelectGroup(group4);
        }
        if (Input.GetKey(KeyCode.Alpha5) && Input.GetKey(KeyCode.LeftShift))
        {
            AddGroup(group5,4);
        }
        if (Input.GetKey(KeyCode.Alpha5) && groupYes[4])
        {
            SelectGroup(group5);
        }
    }
    private void SelectGroup(List<ISelect> group)
    {
        objectSelect.selectObjects.ForEach(x => x.OnDeselect());
        objectSelect.selectObjects.Clear();
        foreach (Unit unit in group)
        {
            if (unit.live)
            {
                objectSelect.selectObjects.Add(unit);
                if (objectSelect.selectObjects.Count == 1)
                {
                    uiManager.OnSelectUnit(unit.gameObject);
                }
                unit.SendMessage("OnSelect", objectSelect.selectObjects.Count);
            }
        }
    }
    private void AddGroup(List<ISelect> group, int ID)
    {
        group.Clear();
        foreach (Unit unit in objectSelect.selectObjects)
        {
            if (unit.live)
            {
                group.Add(unit);
                if (!groupYes[ID]) 
                { 
                    groupYes[ID] = true;
                }
            }
        }
        if (objectSelect.selectObjects.Count > 0)
        {
            StartCoroutine(UIManager.Instance.MessageGreen(ID + 1 + " отряд собран"));
        }
    }
}
