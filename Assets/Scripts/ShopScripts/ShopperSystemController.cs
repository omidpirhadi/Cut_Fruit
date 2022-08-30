using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Diaco.ImageContainerTool;
//public enum FRUITS { Apple = 0 , Orange = 1, Lemon = 2, Watermelon =3 }
public class ShopperSystemController : MonoBehaviour
{



    public ShopperIndicatorUI shopperIndicatorUI;
    public RectTransform Contents;
    public int MaxShopperCount;
    public int ShopperInWave = 0;
    public int ServiceCountInWave = 0;
    public  ShopperInWorldSpwner shopperInWorldSpwner;
    public Transform FruitSpwanPlace;
    
    [SerializeField] public List<FruitInShop> fruitInShops = new List<FruitInShop>();
    public Transform[] ShopperServicePlace;
    private  List<int> PercentFruits = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };

    private List<ShopperIndicatorUI> list_indicatorShopper = new List<ShopperIndicatorUI>();
   
    private int PerviousChoose = 0;
    public void Start()
    {
        
        GenerationWave();
      ///  ServiceShopper();
    }

    public void CalculateScore(float PersonPercent,   float SelectedFuritPercent)

    {


        Debug.Log("Point Person" + ServiceCountInWave);
        ServiceCountInWave++;
        if (ServiceCountInWave == ShopperInWave )
        {
            StartCoroutine(ResetWave());
            Debug.Log("Finish Service To This Wave");
        }
    }
    public IEnumerator ResetWave()
    {

        ShopperInWave = 0;
        ServiceCountInWave = 0;
        var list_furit = GameObject.FindGameObjectsWithTag("furit");
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < list_furit.Length; i++)
        {
            Destroy(list_furit[i].gameObject);
        }
 

        yield return new WaitForSeconds(0.3f);
        GenerationWave();
        Debug.Log("ResetWave");
    }
    public void GenerationWave()
    {
        int shopperCount = UnityEngine.Random.Range(1, MaxShopperCount + 1);
        ClearShopperUI();
        List<int> tempOfchoose = new List<int>(shopperCount);
        var fruit_spwaned_data = SpawnFruit();
        var namefruit = fruit_spwaned_data.Item1;
        Sprite fruit_icon = fruit_spwaned_data.Item2;

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
        Debug.Log("GenrationWave");
    }
    private Tuple<string, Sprite> SpawnFruit()
    {
        int rand = UnityEngine.Random.Range(0, fruitInShops.Count);
        var fruit = Instantiate(fruitInShops[rand].prefab, FruitSpwanPlace.position, Quaternion.identity);
        return new Tuple<string, Sprite>(fruitInShops[rand].Name, fruitInShops[rand].logo);

    }
    private void SpwanShopperIndicator_UI(Sprite profile, Sprite iconFruit, float percent)
    {
        var shopper = Instantiate(shopperIndicatorUI, Contents);
        shopper.Set(null, iconFruit, percent);
        list_indicatorShopper.Add(shopper);
    }

    private void ClearShopperUI()
    {
        list_indicatorShopper.ForEach(e => {
            Destroy(e.gameObject);
        });
        list_indicatorShopper.Clear();
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