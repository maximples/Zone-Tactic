using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LabPanel : MonoBehaviour
{
    private Laboratory laboratory;
    public TextMeshProUGUI nameUnitPrefab;
    public TextMeshProUGUI costUnit;
    public TextMeshProUGUI description;
    public GameObject botton_0_0;
    public GameObject botton_0_1;
    public GameObject botton_0_2;
    public GameObject botton_0_3;
    public void BuilUnitBotton_0_0()
    {
        if (GameStat.player1money >= laboratory.products[0].price)
        {
            laboratory.BuildProduct(0);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(UIManager.Instance.Message("Недостаточно денег"));
        }
    }
    public void BuilUnitBotton_0_1()
    {
        if (GameStat.player1money >= laboratory.products[1].price)
        {
            laboratory.BuildProduct(1);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(UIManager.Instance.Message("Недостаточно денег"));
        }
    }
    public void BuilUnitBotton_0_2()
    {
        if (GameStat.player1money >= laboratory.products[2].price)
        {
            laboratory.BuildProduct(2);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(UIManager.Instance.Message("Недостаточно денег"));
        }
    }
    public void BuilUnitBotton_0_3()
    {
        if (GameStat.player1Technology.heavyFactory)
        {
            if (GameStat.player1money >= laboratory.products[3].price)
            {
                laboratory.BuildProduct(3);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(UIManager.Instance.Message("Недостаточно денег"));
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(UIManager.Instance.Message("Нужно исследовать тяжёлую промышленность"));
        }
    }
    public void EnterBotton_0_0()
    {
        nameUnitPrefab.text = laboratory.products[0].Name;
        costUnit.text = "Цена:  " + laboratory.products[0].price + " Время: " + laboratory.products[0].ConstructTime;
        description.text = laboratory.products[0].description;
    }
    public void EnterBotton_0_1()
    {
        nameUnitPrefab.text = laboratory.products[1].Name;
        costUnit.text = "Цена:  " + laboratory.products[1].price + " Время: " + laboratory.products[1].ConstructTime;
        description.text = laboratory.products[1].description;
    }
    public void EnterBotton_0_2()
    {
        nameUnitPrefab.text = laboratory.products[2].Name;
        costUnit.text = "Цена:  " + laboratory.products[2].price + " Время: " + laboratory.products[2].ConstructTime;
        description.text = laboratory.products[2].description;
    }
    public void EnterBotton_0_3()
    {
        nameUnitPrefab.text = laboratory.products[3].Name;
        costUnit.text = "Цена:  " + laboratory.products[3].price + " Время: " + laboratory.products[3].ConstructTime;
        description.text = laboratory.products[3].description;
    }
    public void ExiteBotton()
    {
        nameUnitPrefab.text = "";
        costUnit.text = "";
        description.text = "";
    }
    public void GetLab(Laboratory newLab)
    {
        laboratory = newLab;
    }
}
