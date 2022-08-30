using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopperInWorldSpwner : MonoBehaviour
{
   
    public float TimeBetweenEverySpawn = 1;
    public Char_Agent[] Humen_prefab;


    public IEnumerator SpawnShopper(int count , Transform [] ShopperPlaceService)
    {
       
        for (int i = 0; i < count; i++)
        {
            var rand = UnityEngine.Random.Range(0, Humen_prefab.Length);
            var shopper = Instantiate(Humen_prefab[rand], transform.position, Quaternion.identity);
           
            yield return new WaitForSecondsRealtime(TimeBetweenEverySpawn);
            shopper.SetDestination(ShopperPlaceService[i].position);
        }
    }


}
