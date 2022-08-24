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
    public ImageContainer FruitsImage_Container;
    public string[] Fruits = new string[4] { "Apple ", "Orange ", "Lemon ", "Watermelon" };
    private  List<int> PercentFruits = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };

    private List<ShopperIndicatorUI> list_indicatorShopper = new List<ShopperIndicatorUI>();
    private int PerviousChoose = 0;
    public void Start()
    {
        ImageContainer.InitializeTexture();
      ///  ServiceShopper();
    }


    [Button("PercentFruit")]
    public void PercentOfFruit(int shopperCount)
    {
        ClearShopperUI();
        List<int> tempOfchoose = new List<int>(shopperCount);
        var fruit_image = SelectFruit();
        Sprite fruit_icon = FruitsImage_Container.LoadImage(fruit_image);

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
        tempOfchoose.ForEach(e =>
        {
            SpwanShopper(null, fruit_icon, e);

        });
        tempOfchoose.Clear();
    }
    private string SelectFruit()
    {
        int rand = UnityEngine.Random.Range(0, Fruits.Length);
        //  Debug.Log(Fruits[rand]);
        return Fruits[rand];

    }
    private void SpwanShopper(Sprite profile, Sprite iconFruit, int percent)
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