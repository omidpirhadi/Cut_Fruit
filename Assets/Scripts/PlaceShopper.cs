
using UnityEngine;

public class PlaceShopper : MonoBehaviour
{

    public int ID;
    public Transform TargetViweCharacter;

    public bool HaveShopper = false;
    private void Start()
    {

    }



    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "shopper" )
        {
            
           
            
            
            var char_agent = obj.GetComponent<Char_Agent>();
            if (char_agent.IDPlace == ID)
            {
                char_agent.CustomerInPlace();
            }

            
        }
    }
}
