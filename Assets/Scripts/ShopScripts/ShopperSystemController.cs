using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEditor;
//public enum FRUITS { Apple = 0 , Orange = 1, Lemon = 2, Watermelon =3 }
public class ShopperSystemController : MonoBehaviour
{

    public CameraController cameraController;
    public ShopperIndicatorUI shopperIndicatorUI;
    public DialogBox dialogBox;
   
    public TMPro.TMP_Text TotalCash_Text;
    public Transform CustomerPlaceSpwan;
    public Transform FruitSpwanPlace;
    public DestroyPlace DestroyPositionAgent;
    public GameObject HUDPanel;
    public GameObject InventoryPanel;
    public GameObject PausePanel;
    public GameObject HomePanel;

    public GlowButton glowbutton;
    public Button Cut_button;
    public Button Pickup_Button;
    public Button Shop_Button;
    public Button Pause_Button;
    public Slider Flow_slider;

    public int Health = 3;
    public float TotalCash;
    public float SliceCash;
    public int QueueCapacity = 4;
    public float TimeBetweenEverySpawn = 1;
    public int RandomModeRepeate = 30;

    public bool TutorialMode = false;
    public TutorialHand HandTutorial;
   
    public GameFlowData gameFlow;
    public Flow InfinityFlow;

    public PickedUpFruitData PickedUpFruit;
    public Char_Agent[] Humen_prefab;
    public List<PlaceShopper> ShopperServicePlace;

    public List<FruitInShop> fruitInShops = new List<FruitInShop>();
    public List<Image> List_Health_images = new List<Image>();
    [HideInInspector]
    public float TimeResponseCustomer = 40;

    private float CountFlow = 0;
    private int currentflow_temp = 0;
    private int flowSpawn_Temp = 0;
    private int Repeatflow_Temp = 0;

    private int PerviousChoose = 0;
    private const int MaxShopperCount = 4;


    private bool TryToSelectFruit = false;
    private bool TryToInitialzFlow = false;

    private bool EnableNormalMode = true;
    private bool EnableRandomMode = false;
    private int CountrepeatedRandomMode = 0;
    private bool EnableInfinityMode = false;
    private CustomerData customerData;
   
    private LeaderboardData leaderboard;
    private int CountCustomerInQueue = 0;

    private List<int> PercentFruits = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };

   
    public void Awake()
    {
        Application.targetFrameRate = 60;
      
    }


    private Tuple<string, Sprite> ChooseFruitForCustomer()
    {

        int rand = UnityEngine.Random.Range(0, fruitInShops.Count);
        var pos = FruitSpwanPlace.position;
        pos.y = 1.18f;

        return new Tuple<string, Sprite>(fruitInShops[rand].Name, fruitInShops[rand].logo);

    }

    private void GenerationWave(int countwave)
    {
        for (int b = 0; b <= countwave; b++)
        {


            int shopperCount = UnityEngine.Random.Range(1, MaxShopperCount + 1);
            // ClearShopperUI();
            List<int> tempOfchoose = new List<int>(shopperCount);
            var fruit_spwaned_data = ChooseFruitForCustomer();
            var namefruit = fruit_spwaned_data.Item1;
            Sprite fruit_icon = fruit_spwaned_data.Item2;

            // PickedUpFruit.Icon_image.sprite = fruit_icon;
            this.PerviousChoose = 0;
            for (int i = shopperCount; i > 0; i--)
            {
                var remining = PercentFruits[PercentFruits.Count - 1] - this.PerviousChoose;
                var indexPreviousChoose = PercentFruits.IndexOf(remining);

                var list_choose = PercentFruits.GetRange(0, (indexPreviousChoose) - (i - 1));
                var choose_rand = UnityEngine.Random.Range(0, list_choose.Count);

                var person_choose = list_choose[choose_rand];

                this.PerviousChoose += person_choose;

                tempOfchoose.Add(person_choose);
            }

            var order = tempOfchoose.OrderBy(X => X).ToList();

            order.ForEach(e =>
            {
                //customerData.customers.Enqueue(new Customer { Fruit = namefruit, PercentFruit = e, logo = fruit_icon });



                if (TutorialMode == true && CountCustomerInQueue < 4)
                {

                    customerData.customers.Enqueue(new Customer { Fruit = "Apple", PercentFruit = 30, logo = fruitInShops[0].logo });
                  //  Debug.Log("TUTORIAL ADDED DATA");
                }
                else 
                {
                    customerData.customers.Enqueue(new Customer { Fruit = namefruit, PercentFruit = e, logo = fruit_icon });
                }
                CountCustomerInQueue++;
                // Debug.Log($"Data: FruitName:{namefruit}Percent:{e}");

            });

            
            // StartCoroutine(SpawnShopper(order, fruit_icon, ShopperServicePlace));
            //   CustomerInWave = order.Count;
            tempOfchoose.Clear();
           //  Debug.Log("GenrationWave");
        }
    }

    private void RegenerationWaveAfterEmptyQueue()
    {
        GenerationWave(10);
        //   Debug.Log("Regenration Wave");
    }
    private IEnumerator SpawnCustomer(int repeatSpawn)
    {
        //yield return new WaitForSecondsRealtime(0.1f);
        if (repeatSpawn <= QueueCapacity)
        {
            for (int i = 0; i < repeatSpawn; i++)
            {

                var position_data = FindFreePlaceInQueueForCustomer();
                var idplace = position_data.Item1;
                var pos = position_data.Item2;
                //yield return new WaitForSecondsRealtime(0.1f);
                var data = customerData.customers.Dequeue();
                var rand = UnityEngine.Random.Range(0, Humen_prefab.Length);
                yield return new WaitForSecondsRealtime(TimeBetweenEverySpawn);

                var shopper = Instantiate(Humen_prefab[rand], CustomerPlaceSpwan.position, CustomerPlaceSpwan.rotation);
            
                yield return new WaitForSecondsRealtime(0.01f);
                shopper.IDPlace = idplace;
                shopper.fruitname = data.Fruit;
                shopper.SetUI(null, data.logo, data.PercentFruit, TimeResponseCustomer);
                if (TutorialMode)
                    shopper.transform.position = new Vector3(-21, 1, -4);
                shopper.SetDestination(pos);
              //    Debug.Log("ShopperPOS" + pos);

            }
        }

        if (customerData.customers.Count < 4)
        {
            RegenerationWaveAfterEmptyQueue();
        }
        TryToInitialzFlow = false;
    }



    public IEnumerator FlowSpwanCustomer()
    {



        if (TryToInitialzFlow == false)
        {
            TryToInitialzFlow = true;
            if (Repeatflow_Temp > 0 && EnableRandomMode == false)
            {
                Repeatflow_Temp--;
                flowSpawn_Temp = Mathf.Clamp(flowSpawn_Temp, 1, 4);
            }
            else if (EnableNormalMode == true)
            {
                if (Repeatflow_Temp == 0)
                {
                    NextflowMode();
                //    Debug.Log("1");
                }
            }
            else if (EnableRandomMode == true)
            {
                if (CountrepeatedRandomMode < 3)
                {
                    RandomModeFlow();
                  //  Debug.Log("2");
                }
            }
            else if (EnableInfinityMode == true)
            {
                InfinityMode();
               // Debug.Log("3");
            }



            if (EnableNormalMode || EnableRandomMode)
            {

                yield return new WaitUntil(() => QueueCapacity == 4);
                StartCoroutine(MessUpPlacePositionArray());
                yield return new WaitForSecondsRealtime(0.2f);
                StartCoroutine(SpawnCustomer(flowSpawn_Temp));
                SliderAddStep();
                // Debug.Log("CapacityQueue:" + QueueCapacity + "Flow:" + flowSpawn_Temp);
            }
            else
            {
                StartCoroutine(SpawnCustomer(QueueCapacity));
                SliderAddStep();
                 // Debug.Log("CapacityQueue:" + QueueCapacity + "Flow infinity" );
            }

            // Debug.Log("Flow_Run");
        }
    }
    private void NextflowMode()

    {
        //  Debug.Log("XXXX NextFlow XXXX");



        if (EnableRandomMode == false)
        {
            currentflow_temp++;
            currentflow_temp = Mathf.Clamp(currentflow_temp, 0, gameFlow.flows.Count - 1);
            TimeResponseCustomer = gameFlow.flows[currentflow_temp].CustomerTimeResponse;
            Repeatflow_Temp = gameFlow.flows[currentflow_temp].RepeatFlow;
            flowSpawn_Temp = gameFlow.flows[currentflow_temp].CustomerInQueue;
        }

        if (currentflow_temp == gameFlow.flows.Count - 1 && EnableRandomMode == false)
        {
            EnableNormalMode = false;
            EnableInfinityMode = false;
            EnableRandomMode = true;
            //   Debug.Log("Random Flow Enabled");
        }

    }

    private void RandomModeFlow()
    {
        var randomselectFlow = UnityEngine.Random.Range(0, gameFlow.flows.Count);
        SelectFlow(randomselectFlow);
        CountrepeatedRandomMode++;
        //   Debug.Log("XXXX Random Mode XXXX");
        if (CountrepeatedRandomMode == RandomModeRepeate)
        {
            EnableInfinityMode = true;
            EnableRandomMode = false;
            EnableNormalMode = false;
            InfinityMode();

        }

    }
    private void InfinityMode()
    {
        currentflow_temp = -100;

        TimeResponseCustomer = InfinityFlow.CustomerTimeResponse;
        Repeatflow_Temp = 1000;
        flowSpawn_Temp = InfinityFlow.CustomerInQueue;
        //  Debug.Log("XXXX infinity Mode initialaz XXXX");
    }
    private void SelectFlow(int index)

    {
        currentflow_temp = index;
        currentflow_temp = Mathf.Clamp(currentflow_temp, 0, gameFlow.flows.Count - 1);
        TimeResponseCustomer = gameFlow.flows[currentflow_temp].CustomerTimeResponse;
        Repeatflow_Temp = gameFlow.flows[currentflow_temp].RepeatFlow;
        flowSpawn_Temp = gameFlow.flows[currentflow_temp].CustomerInQueue;
      ///  Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZ");
    }



    private IEnumerator MessUpPlacePositionArray()
    {
        if (TutorialMode == false)
        {
            List<PlaceShopper> temp = new List<PlaceShopper>();
            int rand = 0;
            PlaceShopper place;
            for (int i = 0; i < ShopperServicePlace.Count; i++)
            {
                do
                {
                    rand = UnityEngine.Random.Range(0, ShopperServicePlace.Count);

                    place = ShopperServicePlace[rand];
                } while (temp.Contains(place));
                temp.Add(place);
                /// Debug.Log(".............." + rand);
            }

            ShopperServicePlace.Clear();
            yield return new WaitForSecondsRealtime(0.1f);


            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].ID = i;
                ShopperServicePlace.Add(temp[i]);
            }

            /// Debug.Log(".............." + place.transform.position);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    private Tuple<int, Vector3> FindFreePlaceInQueueForCustomer()
    {
        Vector3 pos = new Vector3();
        int index = 0;

        for (int i = 0; i < ShopperServicePlace.Count; i++)
        {
            if (!ShopperServicePlace[i].HaveShopper)
            {
                pos = ShopperServicePlace[i].transform.position;
                index = ShopperServicePlace[i].ID;
                QueueCapacity--;
                ShopperServicePlace[i].HaveShopper = true;
                //    Debug.Log("Find Place:" + pos);
                break;
            }
        }
        return new Tuple<int, Vector3>(index, pos);
    }


    public void FreePlaceAfterDestroyCustomer(int idPlace)
    {

        ShopperServicePlace[idPlace].HaveShopper = false;

    }


    public IEnumerator SpawFruit(string name, float price, float cashSlice)
    {
        // Debug.Log("AAAAAAAAAAAA:"+inJob);
        if (TryToSelectFruit == false)
        {
            TryToSelectFruit = true;
            // Debug.Log("BBBBBBBBBBB:"+inJob);
            StartCoroutine(ClearFruitInScene());
            yield return new WaitForSeconds(0.2f);
            foreach (var fruit in fruitInShops)
            {
                if (fruit.Name == name)
                {
                    if (TotalCash >= price)
                    {
                        InventoryPanel.SetActive(false);
                        var s = Instantiate(fruit.prefab, FruitSpwanPlace.position, fruit.prefab.transform.rotation);
                        s.GetComponent<Furit>().FuritTag = fruit.Name;
                        s.GetComponent<Rigidbody>().isKinematic = true;
                        this.SliceCash = cashSlice;
                        AmountCash(-price);
                        TryToSelectFruit = false;
                        if (TutorialMode == false)
                        {
                            //  dialogBox.Set("Ready For Cut", 3);
                            //Debug.Log("FruitSpawned");
                        }
                        Handler_OnFruitSpawn();
                    }
                    else
                    {
                        if (TutorialMode == false)
                        {
                            // dialogBox.Set("No Enoghe Cash", 3);
                        }
                        TryToSelectFruit = false;
                        //  Debug.Log("Cant Spawn Fruit");
                    }

                }

            }

            yield return new WaitForSeconds(0.2f);

        }
    }

    private IEnumerator ClearFruitInScene()
    {
        var list_furit = GameObject.FindGameObjectsWithTag("furit");
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < list_furit.Length; i++)
        {
            Destroy(list_furit[i].gameObject);
        }

        yield return new WaitForSeconds(0.1f);



    }



    public void SetPickedUpFruitData(float precent)
    {
        //100.65656565665
        PickedUpFruit.Percent_text.text = precent.ToString("0") + "gr";
    }
    public void AmountCash(float amount)
    {
        TotalCash += amount;
        SetTextForTotalCash(TotalCash.ToString("0"));
        //  Debug.Log("TotalCash:" + TotalCash);
    }
    private void SetTextForTotalCash(string amount)
    {
        TotalCash_Text.text = amount;
    }

    float stepSliderflow = 0.0f;
    private void SetFlowSlider()

    {
        int a = gameFlow.CountFlowRepeat();
        int b = InfinityFlow.RepeatFlow;
        CountFlow = a + b + RandomModeRepeate;
        stepSliderflow = 1.0f / CountFlow;
        //Debug.Log(stepSliderflow);
        Flow_slider.value = 0;
    }
    private void SliderAddStep()
    {
        Flow_slider.value += stepSliderflow;
    }

    
    public void SetHealth()
    {

        List_Health_images[Health - 1].DOFillAmount(0, 0.5f);
        Health--;



        if (Health == 0)
        {
            StartCoroutine(EndGame());
        }
    }
    private void ResetHealth()
    {
        for (int i = 0; i < List_Health_images.Count; i++)
        {
            List_Health_images[i].DOFillAmount(1, 0.1f);
        }
        Health = 3;
    }

    public IEnumerator StartGame()
    {
        customerData = new CustomerData();
        customerData.customers = new Queue<Customer>();
        leaderboard = new LeaderboardData();
        TotalCash = 1000;

        glowbutton.GlowPos("1");
        ResetHealth();
        if (ExistSave("fruitshop"))
        {

            leaderboard = LoadLeaderboard("fruitshop");

        }
        else
        {
            TutorialMode = true;
            leaderboard.tutorial = true;
        }

        HUDPanel.SetActive(true);
      
        GenerationWave(10);

        if (TutorialMode == true)
        {
            SelectFlow(0);
            StartCoroutine(SpawFruit("Apple", 100, 150));
            StartCoroutine(HandTutorial.Tutorial());
          //  Debug.Log(".................");
        }
        else
        {
            SelectFlow(1);
          ///  Debug.Log("XXXXXXXXXXXXXXXXXXXX");

        }
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(FlowSpwanCustomer());


        yield return new WaitForSecondsRealtime(0.1f);
        SetFlowSlider();
        SetTextForTotalCash(TotalCash.ToString("0"));
        Cut_button.onClick.AddListener(() =>
        {

            //  cameraController.SwitchCamera(1);
            if(TutorialMode)
            {
                HandTutorial.StepTutorial = 0;
            }
            glowbutton.GlowPos("1");
            Handler_OnChangePhase(PhaseGame.Cut);
        });
        Pickup_Button.onClick.AddListener(() =>
        {
            if (TutorialMode)
            {
                HandTutorial.StepTutorial = 2;
            }
            glowbutton.GlowPos("2");
            Handler_OnChangePhase(PhaseGame.Pickup);
        });
        Shop_Button.onClick.AddListener(() =>
        {
            if (TutorialMode)
            {
                HandTutorial.StepTutorial = 4;
                
            }
          //  glowbutton.GlowPos("3");
            InventoryPanel.SetActive(true);
        });
        Pause_Button.onClick.AddListener(() =>
        {

            HUDPanel.SetActive(false);
            InventoryPanel.SetActive(false);
            PausePanel.SetActive(true);
           // glowbutton.GlowPos("4");
        });
        if (TutorialMode == true)
            Pause_Button.interactable = false;
        Debug.Log("GameStart");
        yield return new WaitForSecondsRealtime(0.1f);
        HomePanel.SetActive(false);
        PausePanel.SetActive(false);
        
    }
    public IEnumerator EndGame()
    {

        yield return new WaitForSecondsRealtime(0.1f);
        ClearSceneInEndGame();
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(ClearFruitInScene());
        yield return new WaitForSecondsRealtime(0.1f);

        TutorialMode = false;
        QueueCapacity = 4;

        ShopperServicePlace.ForEach(e => { e.HaveShopper = false; });

        SetLeaderboard();
        SaveLeaderboard("fruitshop");
        Debug.Log("Game OVER");
        HomePanel.SetActive(true);

        yield return new WaitForSecondsRealtime(0.1f);
        PausePanel.SetActive(false);
        HUDPanel.SetActive(false);
        Cut_button.onClick.RemoveAllListeners();

        Pickup_Button.onClick.RemoveAllListeners();
        Pause_Button.onClick.RemoveAllListeners();
        Handler_OnEndGame();
        yield return null;
    }
    private void ClearSceneInEndGame()
    {
        var humen = FindObjectsOfType<Char_Agent>().ToList();
        var cash = FindObjectsOfType<Money>().ToList();
        humen.ForEach(e =>
        {
            Destroy(e.gameObject);
        });
        cash.ForEach(e =>
        {
            Destroy(e.gameObject);
        });
        CountFlow = 0;
        currentflow_temp = 0;
        flowSpawn_Temp = 0;
        Repeatflow_Temp = 0;
        PerviousChoose = 0;
        TryToSelectFruit = false;
        TryToInitialzFlow = false;
        EnableNormalMode = true;
        EnableRandomMode = false;
        CountrepeatedRandomMode = 0;
        EnableInfinityMode = false;
        CountCustomerInQueue = 0;
    }


    #region Read and Write File
    public bool ExistSave(string FileName)
    {
        bool find = false;
        if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
        {
            find = true;
        }
        return find;
    }
    public void SaveLeaderboard(string FileName)
    {

        var json_data = JsonUtility.ToJson(leaderboard);
        if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
        {
            File.Delete(Application.persistentDataPath + "//" + FileName + ".json");
        }
        File.WriteAllText(Application.persistentDataPath + "//" + FileName + ".json", json_data);
         Debug.Log("Data Saved");
    }
    public LeaderboardData LoadLeaderboard(string FileName)
    {
        LeaderboardData data = new LeaderboardData();
        if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
        {
            var j_data = File.ReadAllText(Application.persistentDataPath + "//" + FileName + ".json");
            data = JsonUtility.FromJson<LeaderboardData>(j_data);

        }
         Debug.Log("Data Loaded");
        return data;
    }
    public void SetLeaderboard()
    {
        TutorialMode = false;
        leaderboard.totalCash = this.TotalCash;
        leaderboard.tutorial = false;
       
    }
    #endregion
    #region Events
    private Action resetwave;
    public event Action OnResetWave
    {
        add { resetwave += value; }
        remove { resetwave -= value; }
    }
    protected void Handler_OnReset()
    {
        if (resetwave != null)
        {
            resetwave();
        }
    }
    private Action endgame;
    public event Action OnEndGame
    {
        add { endgame += value; }
        remove { endgame -= value; }
    }
    protected void Handler_OnEndGame()
    {
        if (endgame != null)
        {
            endgame();
        }
    }
    private Action<Vector3> agentmove;
    public event Action<Vector3> OnAgentMove
    {
        add { agentmove += value; }
        remove { agentmove -= value; }
    }
    protected void Handler_OnAgentMove(Vector3 des)
    {
        if (agentmove != null)
        {
            agentmove(des);
        }
    }
    private Action fruitspawn;
    public event Action OnFruitSpawn
    {
        add { fruitspawn += value; }
        remove { fruitspawn -= value; }
    }
    protected void Handler_OnFruitSpawn()
    {
        if (fruitspawn != null)
        {
            fruitspawn();
        }
    }

    public enum PhaseGame { None = 0, Wait = 1, Cut = 2, Pickup = 3, Win = 4, Lose = 5 }
    private Action<PhaseGame> phase;
    public event Action<PhaseGame> OnChangePhase
    {
        add { phase += value; }
        remove { phase -= value; }
    }
    protected void Handler_OnChangePhase(PhaseGame phasegame)
    {
        if (phase != null)
        {
            phase(phasegame);
        }
    }
    #endregion
}
#region Structs
[Serializable]
public struct FruitInShop
{
    public string Id;
   
    public string Name;
    public float Volume;
    public GameObject prefab;
    public Sprite logo;

}

[Serializable]
public struct PickedUpFruitData
{
   
    public Image Icon_image;
    public TMPro.TMP_Text Percent_text;
    
}
[Serializable]
public struct Customer
{
    public int PercentFruit;
    public string Fruit;
    public Sprite logo;
}
[Serializable]
public struct CustomerData
{
    public Queue<Customer> customers;
}

[Serializable]
public struct Flow
{
    public float CustomerTimeResponse;
    public int CustomerInQueue;
    public int RepeatFlow;
}
[Serializable]
public struct GameFlowData
{
  public  List<Flow> flows;
    public int CountFlowRepeat()
    {
        int c = 0;
        for (int i = 0; i < flows.Count; i++)
        {
            c += flows[i].RepeatFlow;
        }
        return c;
    }
}
[Serializable]
public struct LeaderboardData
{
    public float totalCash;
    public bool tutorial;

}
#endregion