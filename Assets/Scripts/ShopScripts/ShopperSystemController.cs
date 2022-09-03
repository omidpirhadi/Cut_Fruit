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
    public int ShopperInWave = 0;
    public int ServiceCountInWave = 0;

    private int shopperinplace;
    public int ShopperInPlaceCount
    {
        set
        {
            shopperinplace = value;
            if (shopperinplace == ShopperInWave && fruitSpawned)
            {
                list_indicatorShopper.ForEach((e) =>
                {
                    e.gameObject.SetActive(true);
                });
                fruitSpawned.SetActive(true);
                cameraController.SwitchCamera(1);
                dialogBox.Set("Cut The Fruit", 5);
                DOVirtual.DelayedCall(2, () => { Handler_OnChangePhase(PhaseGame.Cut); });
                
                // Debug.Log("SET PLACE " + shopperinplace);
            }

        }
        get { return shopperinplace; }
    }
    public ShopperInWorldSpwner shopperInWorldSpwner;
    public Transform FruitSpwanPlace;

    
    public Transform[] ShopperServicePlace;
    public DestroyPlace DestroyPositionAgent;

    public Button Shop_Button;
    public Button Cut_button;
    public Button Pickup_Button;

    public PickedUpFruitData PickedUpFruit;

    private GameObject fruitSpawned;


    private FuritSliceManager sliceManager;
    private DragAndDropItem DragAndDrop;
    private int PerviousChoose = 0;

    [SerializeField]
    public List<FruitInShop> fruitInShops = new List<FruitInShop>();
    private List<int> PercentFruits = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };
    private List<ShopperIndicatorUI> list_indicatorShopper = new List<ShopperIndicatorUI>();


    
    public void Start()
    {
        Application.targetFrameRate = 60;
        Shop_Button.onClick.AddListener(() => {
            cameraController.SwitchCamera(0);
            Handler_OnChangePhase(PhaseGame.Wait);
        });
        Cut_button.onClick.AddListener(() => {

            cameraController.SwitchCamera(1);
            Handler_OnChangePhase(PhaseGame.Cut);
        });
        Pickup_Button.onClick.AddListener(() => {
            cameraController.SwitchCamera(1);
            Handler_OnChangePhase(PhaseGame.Pickup);
        });
        DragAndDrop = FindObjectOfType<DragAndDropItem>();
        sliceManager = GetComponent<FuritSliceManager>();
        StartCoroutine(ResetWave());

    }
 
    public IEnumerator CheckSlice()
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
                var percent_shopper = list_indicatorShopper[i].PercentValue;

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
            dialogBox.Set("Doesn't Match", 2);
            Handler_OnChangePhase(PhaseGame.Lose);
            DOVirtual.DelayedCall(2.5f, () =>
            {
                StartCoroutine(ResetWave());

            });
            // Debug.Log("There is not exist slice and Wave Reset");
        }
        else
        {
            Handler_OnChangePhase(PhaseGame.Pickup);
            Debug.Log("Check Slice Exist :" + find);
        }
    }
    public void CalculateScoreAndCheckExistServicInWave(float PersonPercent, float SelectedFuritPercent)

    {


        //Debug.Log("Point Person" + ServiceCountInWave);
        if(PersonPercent <= SelectedFuritPercent)
        {
            ServiceCountInWave++;
           
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
            DOVirtual.DelayedCall(2, () => {

               
                StartCoroutine(CheckSlice());

            });
        }

        DragAndDrop.Percent_text.text = 0 + "%";
        
    
        
    }

    /*[Button("RESET WAVE")]
    public void Wave()
    {

        DOVirtual.DelayedCall(3, () => { StartCoroutine(ResetWave()); });

    }*/
    public IEnumerator ResetWave()
    {

        dialogBox.Set("Wait For The Customers");
        Handler_OnChangePhase(PhaseGame.Wait);
        Debug.Log("ResetWave");
        Handler_OnAgentMove(DestroyPositionAgent.transform.position);
        var list_furit = GameObject.FindGameObjectsWithTag("furit");
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < list_furit.Length; i++)
        {
            Destroy(list_furit[i].gameObject);
        }
        ShopperInWave = 0;
        ServiceCountInWave = 0;
        ShopperInPlaceCount = 0;
        fruitSpawned = null;

        yield return new WaitForSeconds(0.3f);
        Handler_OnReset();
        GenerationWave();
        cameraController.SwitchCamera(0);
       
    }
    public void GenerationWave()
    {
        int shopperCount = UnityEngine.Random.Range(1, MaxShopperCount + 1);
        ClearShopperUI();
        List<int> tempOfchoose = new List<int>(shopperCount);
        var fruit_spwaned_data = SpawnFruit();
        var namefruit = fruit_spwaned_data.Item1;
        Sprite fruit_icon = fruit_spwaned_data.Item2;
        DragAndDrop.Icon_image.sprite = fruit_icon;
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
            SpwanShopperIndicator_UI(null, fruit_icon, e);
            ShopperInWave++;
        });
        StartCoroutine(shopperInWorldSpwner.SpawnShopper(order.Count, ShopperServicePlace));
        tempOfchoose.Clear();
       // Debug.Log("GenrationWave");
    }
    private Tuple<string, Sprite> SpawnFruit()
    {

        int rand = UnityEngine.Random.Range(0, fruitInShops.Count);
        var pos = FruitSpwanPlace.position;
        pos.y = 1.18f;
        fruitSpawned = Instantiate(fruitInShops[rand].prefab, FruitSpwanPlace.position, Quaternion.identity);
        fruitSpawned.SetActive(false);
        return new Tuple<string, Sprite>(fruitInShops[rand].Name, fruitInShops[rand].logo);

    }
    private void SpwanShopperIndicator_UI(Sprite profile, Sprite iconFruit, float percent)
    {
        var shopper = Instantiate(shopperIndicatorUI, Contents);
        shopper.gameObject.SetActive(false);
        shopper.Set(null, iconFruit, percent);
        list_indicatorShopper.Add(shopper);
    }

    private void ClearShopperUI()
    {
        list_indicatorShopper.ForEach(e =>
        {
            Destroy(e.gameObject);
        });
        list_indicatorShopper.Clear();
    }

    public void DestroyIndicatorShopper( ShopperIndicatorUI shopper)
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

    
    public enum PhaseGame { None = 0 ,  Wait = 1, Cut = 2, Pickup = 3,Win = 4, Lose = 5}
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
[Serializable]
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
}
[Serializable]
public struct FruitInShop
{
    public string Id;
   
    public string Name;
    public float Vloume;
    public GameObject prefab;
    public Sprite logo;

}

[Serializable]
public struct PickedUpFruitData
{
    public GameObject FruitSliceRefrence;   
    public Image Icon_image;
    public TMPro.TMP_Text Percent_text;
    public float PickedUpFuritPercent;
}
