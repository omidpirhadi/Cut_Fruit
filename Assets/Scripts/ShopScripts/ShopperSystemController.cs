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

    
    public int ShopperCount = 2;
    [SerializeField] public Shopper ShopperLeft;
    [SerializeField] public Shopper ShopperRight;
    public ImageContainer FruitsImage_Container;
    [SerializeField] public string[] Fruits = new string[4] { "Apple ", "Orange ", "Lemon ", "Watermelon" };
    public List<int> PercentFruits = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
    


    public void Start()
    {
        ImageContainer.InitializeTexture();
      ///  ServiceShopper();
    }
    [Button("ServiceShopper")]
    public void ServiceShopper()
    {
        
        var nameleft = SelectFruit();
        var nameright = SelectFruit();

      //  Debug.Log(nameleft+"..........." + nameright);

        var image_left = FruitsImage_Container.LoadImage(nameleft);

        ShopperLeft.set(nameleft, PercentFruit(), image_left);

        var image_right = FruitsImage_Container.LoadImage(nameright);

        ShopperRight.set(nameright, PercentFruit(), image_right);
    }
    private string SelectFruit()
    {
        int rand = UnityEngine.Random.Range(0, Fruits.Length);
      //  Debug.Log(Fruits[rand]);
        return Fruits[rand];
        
    }
    private int  PercentFruit()
    {
        int rand = UnityEngine.Random.Range(0, PercentFruits.Count );
       // Debug.Log(PercentFruits[rand]);
        return PercentFruits[rand];
    }
    [Button("USE")]
    public void Use(int shopperCount)
    {
        int tempshopperCount = shopperCount;
        List<int> percent = new List<int>();
        List<int> temparray = new List<int>(PercentFruits);
        for (int i = 0; i < shopperCount; i++)
        {
            
            var t = PercentFruit2(temparray, tempshopperCount);
            percent.Add(t.Item1);
            temparray.Clear();
            temparray.InsertRange(0, t.Item2);
            tempshopperCount--;
            
        }
      
       
    }
    private Tuple<int , List<int>> PercentFruit2(List<int> precents ,  int shopperCount )
    {
        var len = precents.Count;// 9 
        var max = precents[len - 1];// 90

        /*
        //////////////////////////// all numbers
        var delta = shopperCount * precents[0];
        var endRand = 0;
        for (var i = 0; i< precents.Count; i++)
        {
            if(precents[i]<= max - delta)
            {
                endRand = i;
            }
        }
        int rand = UnityEngine.Random.Range(0, endRand + 1); /// (0,<rang_chose)
        ////////////////////
        */
        // just 10 numbers
        var range_chose = (len - shopperCount) + 1;/// (9-n)+1
        int rand = UnityEngine.Random.Range(0, range_chose); /// (0,<rang_chose)
        

        List<int> temparray = new List<int>();
        //Debug.Log("--------- rand:" + precents[rand]);
        for (int i = 0; i < precents.Count; i++)
        {
            if (precents[i] <= max - precents[rand])
                temparray.Add(precents[i]);
        }


        Debug.Log("Shopper------------------------" + precents[rand]);
        return new Tuple<int, List<int>>(precents[rand], temparray);
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