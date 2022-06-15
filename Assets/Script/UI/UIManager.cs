using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    private Builder builder;
    public GameObject HpBarGameObject;
    public GameObject FabricPanel;
    public GameObject BuilderPanel;
    public GameObject FactoryPanel;
    public GameObject LabPanel;
    public Unit unit=null;
    public Build build=null;
    public Image portretImage;
    public Sprite imageNull;
    public float maxValue = 100;
    private static float current;
    public Slider slider;
    public Slider buildBar;
    public TextMeshProUGUI HpBar;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI BuildBarText;
    public TextMeshProUGUI nameUnit;
    public TextMeshProUGUI controllUnit;
    public TextMeshProUGUI damag;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI nameUnitPrefab;
    public TextMeshProUGUI costUnit;
    public TextMeshProUGUI description;
    public TextMeshProUGUI money;
    public static UIManager Instance;
    public LabPanel labPanel;
    public FactoryPanel factoryPanel;
    [SerializeField] private BuildManager buildManger;
    public static float currentValue
    {
        get { return current; }
    }
    void Awake()
    {
        Instance = this;
        HpBar.text = "";
        slider.fillRect.GetComponent<Image>().color = Color.green;
        slider.maxValue = maxValue;
        slider.minValue = 0;
        current = maxValue;
        buildBar.fillRect.GetComponent<Image>().color = Color.green;
        buildBar.maxValue = 100;
        buildBar.minValue = 0;
        portretImage.sprite = imageNull;
    }
    public void OnSelectUnit(GameObject selectUnit)
    {
        unit = selectUnit.GetComponent<Unit>();
        portretImage.sprite = unit.image;
        nameUnit.text = unit.nameUnit;
        maxValue = unit.MaxHealth;
        current=unit.CurrentHealth;
        damag.text = unit.damag+"";
        speed.text = unit.speed+"";
        HpBarGameObject.SetActive(true);
        if (unit.player== Players.Player1)
        {
            controllUnit.text = GameStat.playerName;
            controllUnit.color = Color.green;
            if (unit.tipUnit ==TipUnit.Builder && unit.live)
            {
                BuilderPanel.SetActive(true);
                builder = unit.gameObject.GetComponent<Builder>();
            }
        }
        if (unit.player == Players.Player2)
        {
            controllUnit.text = "����";
            controllUnit.color = Color.red;
        }
    }
    void Update()
    {
        money.text = "������ "+ Mathf.Round(GameStat.player1money);
        if (unit != null)
        {
                slider.maxValue = unit.MaxHealth;
                if (current < 0) current = 0;
                if (current > maxValue) current = maxValue;
                slider.value = unit.CurrentHealth;
                HpBar.text = unit.MaxHealth + "/" + unit.CurrentHealth;
        }
        else
        if (build != null)
        {
                slider.maxValue = build.MaxHealth;
                if (current < 0) current = 0;
                if (current > maxValue) current = maxValue;
                slider.value = build.CurrentHealth;
                HpBar.text = build.MaxHealth + "/" + Mathf.Round(build.CurrentHealth);
                
        }

    }
    public void OnSelectBuild(Build selectBuild)
    {
        build = selectBuild;
        unit = null;
        portretImage.sprite = build.image;
        nameUnit.text = build.nameUnit;
        maxValue = build.MaxHealth;
        current = build.CurrentHealth;
        damag.text =  "";
        speed.text =  "";
        HpBarGameObject.SetActive(true);
        if (build.player == Players.Player1)
        {
            controllUnit.text = GameStat.playerName;
            controllUnit.color = Color.green;
            if (build.tipBuild==TipBuild.FabricaM&& !build.building)
            {
                FabricPanel.SetActive(true);
                buildBar.gameObject.SetActive(true);
                Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
                BuildBarText.text = "� ������� " + fabrica.ProductQueue.Count;
            }
            if (build.tipBuild == TipBuild.Labaratori && !build.building)
            {
                LabPanel.SetActive(true);
                buildBar.gameObject.SetActive(true);
                Laboratory laboratory = build.gameObject.GetComponent<Laboratory>();
                labPanel.GetLab(laboratory);
                laboratory.labPanel = labPanel;
            }
            if (build.tipBuild == TipBuild.FabricaL && !build.building)
            {
                FactoryPanel.SetActive(true);
                buildBar.gameObject.SetActive(true);
                Factory_L factory_L = build.gameObject.GetComponent<Factory_L>();
                factoryPanel.GetFactory(factory_L);
            }
        }
        if (build.player == Players.Player2)
        {
            controllUnit.text = "����";
            controllUnit.color = Color.red;
        }
    }
    public void OnDeselectUnit()
    {
        portretImage.sprite = imageNull;
        nameUnit.text = "";
        controllUnit.text = "";
        HpBar.text = "";
        damag.text = "";
        speed.text = "";
        BuildBarText.text = "";
        messageText.text = "";
        HpBarGameObject.SetActive(false);
        FabricPanel.SetActive(false);
        BuilderPanel.SetActive(false);
        LabPanel.SetActive(false);
        buildBar.gameObject.SetActive(false);
        FactoryPanel.SetActive(false);
        unit = null;
        build = null;
     }
    public void BuilUnitBotton_0_0()
    {
       
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        if (GameStat.player1money >= fabrica.products[0].price)
        {
            GameStat.player1money -= fabrica.products[0].price;
            fabrica.BuildProduct(0);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("������������ �����"));
        }
    }
    public void BuilUnitBotton_0_1()
    {
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        if (GameStat.player1money >= fabrica.products[1].price)
        {
            GameStat.player1money -= fabrica.products[1].price;
            fabrica.BuildProduct(1);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("������������ �����"));
        }
    }
    public void BuilUnitBotton_0_2()
    {
        if (GameStat.player1Technology.RSZO)
        {
            Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
            if (GameStat.player1money >= fabrica.products[2].price)
            {
                GameStat.player1money -= fabrica.products[2].price;
                fabrica.BuildProduct(2);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Message("������������ �����"));
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("���������� ���������� ����"));
        }
    }
    public void BuilUnitBotton_0_3()
    {
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        if (GameStat.player1money >= fabrica.products[3].price)
        {
            GameStat.player1money -= fabrica.products[3].price;
            fabrica.BuildProduct(3);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("������������ �����"));
        }
    }
    public void BuildBotton_0_0()
    {
        if (GameStat.player1money >= buildManger.templates[0].price)
        {
            buildManger.buildBuilding(0, builder);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("������������ �����"));
        }
    }
    public void BuildBotton_0_1()
    {
        if (GameStat.baseNumberPlaer1 > 0)
        {
            if (GameStat.player1money >= buildManger.templates[1].price)
            {
                buildManger.buildBuilding(1, builder);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Message("������������ �����"));
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("��������� ��������� �����"));
        }
    }
    public void BuildBotton_0_2()
    {
        if (GameStat.baseNumberPlaer1 > 0)
        {
            if (GameStat.player1money >= buildManger.templates[2].price)
        {
            buildManger.buildBuilding(2, builder);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("������������ �����"));
        }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("��������� ��������� �����"));
        }
    }
    public void BuildBotton_0_3()
    {
        if (GameStat.baseNumberPlaer1 > 0)
        {
            if (GameStat.player1Technology.heavyFactory)
            {
                if (GameStat.player1money >= buildManger.templates[3].price)
                {
                    buildManger.buildBuilding(3, builder);
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(Message("������������ �����"));
                }
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Message("���������� ���������� ����� ������ �������"));
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("��������� ��������� �����"));
        }
    }
    public void BuildBotton_1_0()
    {
        if (GameStat.player1money >= buildManger.templates[4].price)
        {
            buildManger.buildBuilding(4, builder);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("������������ �����"));
        }

    }
    public void BuildBotton_1_1()
    {
        if (GameStat.baseNumberPlaer1 > 0)
        {
            if (GameStat.player1money >= buildManger.templates[5].price)
            {
                buildManger.buildBuilding(5, builder);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Message("������������ �����"));
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("��������� ��������� �����"));
        }
    }
    public void BuildBotton_1_2()
    {
        if (GameStat.baseNumberPlaer1 > 0)
        {
            if (GameStat.player1Technology.repireTover)
            {
                if (GameStat.player1money >= buildManger.templates[6].price)
                {
                    buildManger.buildBuilding(6, builder);
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(Message("������������ �����"));
                }
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Message("���������� ���������� ������ �������"));
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("��������� ��������� �����"));
        }
    }

    public void BuildBotton_1_3()
    {
        if (GameStat.baseNumberPlaer1 > 0)
        {
            if (GameStat.player1Technology.towerMulti)
            {
                if (GameStat.player1money >= buildManger.templates[7].price)
                {
                    buildManger.buildBuilding(7, builder);
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(Message("������������ �����"));
                }
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Message("���������� ���������� �������� �����"));
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Message("��������� ��������� �����"));
        }
    }
    public void EnterBotton_0_0()
    {
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        nameUnitPrefab.text = fabrica.products[0].Name;
        costUnit.text = "����:  " + fabrica.products[0].price + " �����: " + fabrica.products[0].ConstructTime;
        description.text = fabrica.products[0].description;
    }
    public void EnterBotton_0_1()
    {
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        nameUnitPrefab.text = fabrica.products[1].Name;
        costUnit.text = "����:  " + fabrica.products[1].price + " �����: " + fabrica.products[1].ConstructTime;
        description.text = fabrica.products[1].description;
    }
    public void EnterBotton_0_2()
    {
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        nameUnitPrefab.text = fabrica.products[2].Name;
        costUnit.text = "����:  " + fabrica.products[2].price + " �����: " + fabrica.products[2].ConstructTime;
        description.text = fabrica.products[2].description;
    }
    public void EnterBotton_0_3()
    {
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        nameUnitPrefab.text = fabrica.products[3].Name;
        costUnit.text = "����:  " + fabrica.products[3].price + " �����: " + fabrica.products[3].ConstructTime;
        description.text = fabrica.products[3].description;
    }
    public void EnterBottonBuild_0_0()
    {
        nameUnitPrefab.text = buildManger.templates[0].Name;
        costUnit.text = "����:  " + buildManger.templates[0].price + " �����: " + buildManger.templates[0].ConstructTime;
        description.text = buildManger.templates[0].description;
    }
    public void EnterBottonBuild_0_1()
    {
        nameUnitPrefab.text = buildManger.templates[1].Name;
        costUnit.text = "����:  " + buildManger.templates[1].price + " �����: " + buildManger.templates[1].ConstructTime;
        description.text = buildManger.templates[1].description;
    }
    public void EnterBottonBuild_0_2()
    {
        nameUnitPrefab.text = buildManger.templates[2].Name;
        costUnit.text = "����:  " + buildManger.templates[2].price + " �����: " + buildManger.templates[2].ConstructTime;
        description.text = buildManger.templates[2].description;
    }
    public void EnterBottonBuild_0_3()
    {
        nameUnitPrefab.text = buildManger.templates[3].Name;
        costUnit.text = "����:  " + buildManger.templates[3].price + " �����: " + buildManger.templates[3].ConstructTime;
        description.text = buildManger.templates[3].description;
    }
    public void EnterBottonBuild_1_0()
    {
        nameUnitPrefab.text = buildManger.templates[4].Name;
        costUnit.text = "����:  " + buildManger.templates[4].price + " �����: " + buildManger.templates[4].ConstructTime;
        description.text = buildManger.templates[4].description;
    }
    public void EnterBottonBuild_1_1()
    {
        nameUnitPrefab.text = buildManger.templates[5].Name;
        costUnit.text = "����:  " + buildManger.templates[5].price + " �����: " + buildManger.templates[5].ConstructTime;
        description.text = buildManger.templates[5].description;
    }
    public void EnterBottonBuild_1_2()
    {
        nameUnitPrefab.text = buildManger.templates[6].Name;
        costUnit.text = "����:  " + buildManger.templates[6].price + " �����: " + buildManger.templates[6].ConstructTime;
        description.text = buildManger.templates[6].description;
    }
    public void EnterBottonBuild_1_3()
    {
        nameUnitPrefab.text = buildManger.templates[7].Name;
        costUnit.text = "����:  " + buildManger.templates[7].price + " �����: " + buildManger.templates[7].ConstructTime;
        description.text = buildManger.templates[7].description;
    }
    public void ExiteBotton()
    {
        nameUnitPrefab.text = "";
        costUnit.text = "";
        description.text = "";
    }
    public IEnumerator Message(string message)
    {
        messageText.color = Color.red;
        messageText.text = message;
        yield return new WaitForSeconds(3);
        messageText.text = "";
    }
    public IEnumerator MessageGreen(string message)
    {
        messageText.color = Color.green;
        messageText.text = message;
        yield return new WaitForSeconds(3);
        messageText.text = "";
    }
}