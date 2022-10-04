using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using DG.Tweening;
//public enum FRUITS { Apple = 0 , Orange = 1, Lemon = 2, Watermelon =3 }
public class ShopperSystemController : MonoBehaviour
{


    public CameraController cameraController;
    public ShopperIndicatorUI shopperIndicatorUI;
    public DialogBox dialogBox;
    public RectTransform Contents;
    public int MaxShopperCount;
    public TMPro.TMP_Text Point_Text;

    public float TotalCash;
    public float SliceCash;
    public float ScorePoint = 0;

    private int customerinwave = -1;
    public int CustomerInWave
    {
        set {
            customerinwave = value;
            if(customerinwave == 0)
            {
               
                //StartCoroutine(ResetWave());
            }

        }
        get { return customerinwave; }
    }

 


    public float TimeResponseCustomer = 40;
    public float TimeBetweenEverySpawn = 1;
    public Char_Agent[] Humen_prefab;

    public Transform FruitSpwanPlace;

    public int QueueCapacity = 4;
    public PlaceShopper[] ShopperServicePlace;
    public DestroyPlace DestroyPositionAgent;

    public Button SpawnFruit_Button;
    public Button Cut_button;
    public Button Pickup_Button;
    public Button Shop_Button;
    public PickedUpFruitData PickedUpFruit;



    [SerializeField]
    public List<FruitInShop> fruitInShops = new List<FruitInShop>();
    private List<int> PercentFruits = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };
    private List<ShopperIndicatorUI> list_indicatorShopper = new List<ShopperIndicatorUI>();
    private CustomerData customerData;
    private int PerviousChoose = 0;
  
    private bool InResetWave = false;
    public void Start()
    {
        Application.targetFrameRate = 60;
        customerData = new CustomerData();
        customerData.customers = new Queue<Customer>();
        GenerationWave(10);

        Cut_button.onClick.AddListener(() =>
        {

            //  cameraController.SwitchCamera(1);
            Handler_OnChangePhase(PhaseGame.Cut);
        });
        Pickup_Button.onClick.AddListener(() =>
        {
            // cameraController.SwitchCamera(1);
            Handler_OnChangePhase(PhaseGame.Pickup);
        });
       /* Shop_Button.onClick.AddListener(() =>
        {
           // StartCoroutine(ResetWave());
        });*/
        StartCoroutine(SpawnCustomer(4));

        
       
    }



    public void CalculateScore(float pointoffset)
    {
        var tempscore = 1000;
        if(pointoffset <=2)
        {
            tempscore = 1000;
        }
        else if(pointoffset>2 && pointoffset<=5)
        {
            tempscore -= 300;
        }
        else if(pointoffset>=5 && pointoffset<11)
        {
            tempscore -= 500;
        }
        else if(pointoffset>15)
        {
            tempscore -= 0;
        }
        ScorePoint += tempscore;
        Point_Text.text = ScorePoint.ToString();
    }
   



    [Button("GenerateCustomer", ButtonSizes.Medium)]
    public void GenerationWave(int countwave)
    {
        for (int b = 0; b <= countwave; b++)
        {


            int shopperCount = UnityEngine.Random.Range(1, MaxShopperCount + 1);
           // ClearShopperUI();
            List<int> tempOfchoose = new List<int>(shopperCount);
            var fruit_spwaned_data = ChooseFruitForCustomer();
            var namefruit = fruit_spwaned_data.Item1;
            Sprite fruit_icon = fruit_spwaned_data.Item2;

            PickedUpFruit.Icon_image.sprite = fruit_icon;
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
                customerData.customers.Enqueue(new Customer { Fruit = namefruit, PercentFruit = e, logo = fruit_icon });
               // Debug.Log($"Data: FruitName:{namefruit}Percent:{e}");

            });


            // StartCoroutine(SpawnShopper(order, fruit_icon, ShopperServicePlace));
            //   CustomerInWave = order.Count;
            tempOfchoose.Clear();
            // Debug.Log("GenrationWave");
        }
    }


    public IEnumerator ClearFruitInScene()
    {
        var list_furit = GameObject.FindGameObjectsWithTag("furit");
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < list_furit.Length; i++)
        {
            Destroy(list_furit[i].gameObject);
        }

        yield return new WaitForSeconds(0.1f);



    }
    private bool inJob = false;
    public IEnumerator SpawFruit(string name, float price, float cashSlice)
    {
        if (inJob == false)
        {
            inJob = true;

            StartCoroutine(ClearFruitInScene());
            yield return new WaitForSeconds(0.2f);
            foreach (var fruit in fruitInShops)
            {
                if (fruit.Name == name)
                {
                    if (TotalCash > price)
                    {
                        Instantiate(fruit.prefab, FruitSpwanPlace.position, Quaternion.identity);
                        this.SliceCash = cashSlice;
                        TotalCash -= price;
                        dialogBox.Set("Ready For Cut");
                        Debug.Log("FruitSpawned");
                    }
                    else
                    {
                        dialogBox.Set("Ready For Cut");
                        Debug.Log("FruitSpawned");
                    }

                }

            }
            yield return new WaitForSeconds(0.2f);
            inJob = false;
        }
    }
    private Tuple<string, Sprite> ChooseFruitForCustomer()
    {

        int rand = UnityEngine.Random.Range(0, fruitInShops.Count);
        var pos = FruitSpwanPlace.position;
        pos.y = 1.18f;
      
        return new Tuple<string, Sprite>(fruitInShops[rand].Name, fruitInShops[rand].logo);

    }

    public IEnumerator SpawnCustomer(int repeatSpawn)
    {
        for (int i = 0; i < repeatSpawn; i++)
        {


            if (QueueCapacity <= 4)
            {

                var position_data = QueueCustomerControll();
                var idplace = position_data.Item1;
                var pos = position_data.Item2;
                var data = customerData.customers.Dequeue();
                var rand = UnityEngine.Random.Range(0, Humen_prefab.Length);
                yield return new WaitForSecondsRealtime(TimeBetweenEverySpawn);

                var shopper = Instantiate(Humen_prefab[rand], transform.position, Quaternion.identity);
                yield return new WaitForSecondsRealtime(0.1f);
                shopper.IDPlace = idplace;
                shopper.SetUI(null, data.logo, data.PercentFruit, TimeResponseCustomer);
                shopper.SetDestination(pos);
                //ShopperInWave++;
                Debug.Log("AAA");

            }
        }
    }
    
    public Tuple<int , Vector3> QueueCustomerControll()
    {
        Vector3 pos = new Vector3();
        int index = 0;
        for (int i = 0; i < ShopperServicePlace.Length; i++)
        {
            if(!ShopperServicePlace[i].HaveShopper)
            {
                pos = ShopperServicePlace[i].transform.position;
                index = i;
                QueueCapacity--;
                ShopperServicePlace[i].HaveShopper = true;
                Debug.Log("Find Place:" + pos);
                break;
            }
        }
        return new Tuple<int, Vector3>(index, pos);
    }
    public void FreePlaceAfterDestroyCustomer(int idPlace)
    {
        ShopperServicePlace[idPlace].HaveShopper = false;
    }
    public void SetPickedUpFruitData(float precent)
    {
        //100.65656565665
        PickedUpFruit.Percent_text.text = precent.ToString("0") + "%";
    }
 
    private void ClearShopperUI()
    {
        list_indicatorShopper.ForEach(e =>
        {
            Destroy(e.gameObject);
        });
        list_indicatorShopper.Clear();
    }

    public void DestroyIndicatorShopper(ShopperIndicatorUI shopper)
    {
        list_indicatorShopper.Remove(shopper);
        Destroy(shopper.gameObject);
    }




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
#endregion