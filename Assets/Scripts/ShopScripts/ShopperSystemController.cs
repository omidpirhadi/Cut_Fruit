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


    public PlaceShopper[] ShopperServicePlace;
    public DestroyPlace DestroyPositionAgent;

    public Button SpawnFruit_Button;
    public Button Cut_button;
    public Button Pickup_Button;
    public Button Reset_Button;
    public PickedUpFruitData PickedUpFruit;
    //public TMPro.TMP_Text CutCount_Text;
    //private Cutter cutter;
    //private int CutCount = 0;
    //private GameObject fruitSpawned;


   // private FuritSliceManager sliceManager;
   // private DragAndDropItem DragAndDrop;
    private int PerviousChoose = 0;

    [SerializeField]
    public List<FruitInShop> fruitInShops = new List<FruitInShop>();
    private List<int> PercentFruits = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };
    private List<ShopperIndicatorUI> list_indicatorShopper = new List<ShopperIndicatorUI>();
    private CustomerData customerData;
    public int QueueCapacity = 4;
    private bool InResetWave = false;
    public void Start()
    {
        Application.targetFrameRate = 60;
        customerData = new CustomerData();
        customerData.customers = new Queue<Customer>();
        GenerationWave(10);
        /*SpawnFruit_Button.onClick.AddListener(() =>
        {

            StartCoroutine(OnlySpawnFruit());
            
        });*/
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
        Reset_Button.onClick.AddListener(() =>
        {
           // StartCoroutine(ResetWave());
        });
        StartCoroutine(SpawnCustomer(4));
        /* DragAndDrop = FindObjectOfType<DragAndDropItem>();*/
       // sliceManager = GetComponent<FuritSliceManager>();
        // cutter = GetComponent<Cutter>();
        //   cutter.OnCut += Cutter_OnCut;
      //  StartCoroutine(ResetWave());
        
       
    }

    /* private void Cutter_OnCut()
     {

        /* var S = this.CutCount - 1;
         this.CutCount = Mathf.Clamp(S, 0, CutCount+1);
         this.CutCount_Text.text = this.CutCount.ToString();
         if(CutCount == 0)
         {
             Handler_OnChangePhase(PhaseGame.Wait);
             StartCoroutine(CaluclateScore());



         }
     }*/

    /*public IEnumerator CheckSlice()
    {
        sliceManager.AddToListSlicedFruit();
        var fruit_in_world = GameObject.FindGameObjectsWithTag("furit");
        yield return new WaitForSeconds(0.1f);

        var shopper_count = list_indicatorShopper.Count;
        // var sliced_count = sliceManager.FuritsInGame.Furits[0].slicedPieces.Count;
        yield return new WaitForSeconds(0.1f);
        int find = 0;
        if (shopper_count > 0)
        {
            for (int i = 0; i < shopper_count; i++)
            {
                var percent_shopper = list_indicatorShopper[i].NeedPercentValue;

                for (int j = 0; j < fruit_in_world.Length; j++)
                {
                    float percent_fruit = 0.0f;
                    if (fruit_in_world[j].GetComponent<Furit>())
                    {
                        percent_fruit = fruit_in_world[j].GetComponent<Furit>().PercentVolume;
                    }
                    else
                    {
                        percent_fruit = fruit_in_world[j].GetComponent<FruitPiece>().PercentVolume;
                    }
                    //Debug.Log($" shopper{percent_shopper} slice{percent_slice}");
                    if (percent_shopper <= percent_fruit)
                    {

                        find++;
                    }
                }
            }
        }
        if (find == 0)
        {

            DOVirtual.DelayedCall(2.5f, () =>
            {
                StartCoroutine(ResetWave());

            });
            // Debug.Log("There is not exist slice and Wave Reset");
        }
        else
        {

            Debug.Log("Check Slice Exist :" + find);
        }
    }*/


    /* public void CalculateScoreAndCheckExistServicInWave(float PersonPercent, float SelectedFuritPercent)

     {


         //Debug.Log("Point Person" + ServiceCountInWave);
         if (PersonPercent <= SelectedFuritPercent)
         {
             //ServiceCountInWave++;

             dialogBox.Set("Well Done", 2);

         }
        if (ServiceCountInWave == ShopperInWave)
         {
             dialogBox.Set("Good job Ready For Next Level", 2);

             StartCoroutine(ResetWave());
             // Debug.Log("Finish Service To This Wave and Reset");
         }
         else
         {

             Handler_OnChangePhase(PhaseGame.Wait);
             DOVirtual.DelayedCall(2, () =>
             {


                 StartCoroutine(CheckSlice());

             });
         }

        /// DragAndDrop.Percent_text.text = 0 + "%";



     }*/

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
   
  /*  public IEnumerator ResetWave()
    {
        dialogBox.Set("Wait For The Customers", 5);
        yield return new WaitForSeconds(5f);
        
        Handler_OnChangePhase(PhaseGame.Wait);
       
       // Handler_OnAgentMove(DestroyPositionAgent.transform.position);
        var list_furit = GameObject.FindGameObjectsWithTag("furit");
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < list_furit.Length; i++)
        {
            Destroy(list_furit[i].gameObject);
        }
        
        yield return new WaitForSeconds(0.3f);
        //fruitSpawned = null;
       // CustomerInWave = -1;
        Handler_OnReset();
        //GenerationWave();
        cameraController.SwitchCamera(0);
        

    }*/


    [Button ("DebugCustomer", ButtonSizes.Medium)]
    public void logwave()
    {
        foreach(var x in customerData.customers)
        {
            Debug.Log($"Data: FruitName:{x.Fruit}Percent:{x.PercentFruit}");
        }
    }
    [Button("GenerateCustomer", ButtonSizes.Medium)]
    public void GenerationWave(int countwave)
    {
        for (int b = 0; b <= countwave; b++)
        {


            int shopperCount = UnityEngine.Random.Range(1, MaxShopperCount + 1);
            ClearShopperUI();
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
                Debug.Log($"Data: FruitName:{namefruit}Percent:{e}");

            });


            // StartCoroutine(SpawnShopper(order, fruit_icon, ShopperServicePlace));
            //   CustomerInWave = order.Count;
            tempOfchoose.Clear();
            // Debug.Log("GenrationWave");
        }
    }
    private Tuple<string, Sprite> ChooseFruitForCustomer()
    {

        int rand = UnityEngine.Random.Range(0, fruitInShops.Count);
        var pos = FruitSpwanPlace.position;
        pos.y = 1.18f;
      //  Instantiate(fruitInShops[rand].prefab, FruitSpwanPlace.position, Quaternion.identity);
        //   fruitSpawned.SetActive(false);
        return new Tuple<string, Sprite>(fruitInShops[rand].Name, fruitInShops[rand].logo);

    }
    [Button("SpawnCustomer", ButtonSizes.Medium)]
    public void StartSSSS()
    {
        
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
   /* private void SpwanShopperIndicator_UI(Sprite profile, Sprite iconFruit, float percent)
    {
        var shopper = Instantiate(shopperIndicatorUI, Contents);
        shopper.gameObject.SetActive(false);
        shopper.Set(null, iconFruit, percent);
        list_indicatorShopper.Add(shopper);
        InResetWave = false;
       
    }
    public void SetShopperPrograssbar_UIIndicator(int index , float amount)
    {
        list_indicatorShopper[index].PrograssbarSet(amount);
    }*/
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
}
/*[Serializable]
public struct Shopper
{
    public string Fruit;
    public int PercentFruit;
    public TMPro.TMP_Text PrecentFruit_text;
    public Image ImageFruit_image;
    public void set(string f, int p , Sprite image)
    {
        this.Fruit = f;
        this.PercentFruit = p;
        this.ImageFruit_image.sprite = image;
        PrecentFruit_text.text = "%"+p;

    }
}*/
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
