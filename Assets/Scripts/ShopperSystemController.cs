using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Diaco.ImageContainerTool;
//public enum FRUITS { Apple = 0 , Orange = 1, Lemon = 2, Watermelon =3 }
public class ShopperSystemController : MonoBehaviour
{

    [SerializeField] public Shopper ShopperLeft;
    [SerializeField] public Shopper ShopperRight;
    public ImageContainer FruitsImage_Container;
    [SerializeField] public string[] Fruits = new string[4] { "Apple ", "Orange ", "Lemon ", "Watermelon" };
    public int[] PercentFruits = new int[] { 10, 20 , 30 , 40, 50};

   

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
    private int PercentFruit()
    {
        int rand = UnityEngine.Random.Range(0, PercentFruits.Length);
       // Debug.Log(PercentFruits[rand]);
        return PercentFruits[rand];
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