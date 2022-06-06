using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FactoryPanel : MonoBehaviour
{
    private Factory_L factory;
    public TextMeshProUGUI nameUnitPrefab;
    public TextMeshProUGUI costUnit;
    public TextMeshProUGUI description;
    public GameObject botton_0_0;
    public GameObject botton_0_1;
    public void BuilUnitBotton_0_0()
    {
        if (GameStat.player1money >= factory.products[0].price)
        {
            factory.BuildProduct(0);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(UIManager.Instance.Message("Недостаточно денег"));
        }
    }
    public void BuilUnitBotton_0_1()
    {
        if (GameStat.player1Technology.heavyTank)
        {
            if (GameStat.player1money >= factory.products[1].price)
            {
                factory.BuildProduct(1);
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
            StartCoroutine(UIManager.Instance.Message("Исследуйте технологию тяжёлый танк"));
        }
    }

    public void EnterBotton_0_0()
    {
        nameUnitPrefab.text = factory.products[0].Name;
        costUnit.text = "Цена:  " + factory.products[0].price + " Время: " + factory.products[0].ConstructTime;
        description.text = factory.products[0].description;
    }
    public void EnterBotton_0_1()
    {
        nameUnitPrefab.text = factory.products[1].Name;
        costUnit.text = "Цена:  " + factory.products[1].price + " Время: " + factory.products[1].ConstructTime;
        description.text = factory.products[1].description;
    }
    public void ExiteBotton()
    {
        nameUnitPrefab.text = "";
        costUnit.text = "";
        description.text = "";
    }
    public void GetFactory(Factory_L newfactory_L)
    {
        factory = newfactory_L;
    }
}
