using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetSprite : MonoBehaviour
{
    public List<GameObject> prefList = new List<GameObject>();
    public int tileNo;
    // Start is called before the first frame update
    private void Awake()
    {
        tileNo = WorldData.allPallet;
        setSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setSprite()
    {
        if (tileNo<684)
        {
            GameObject spawnPref = Instantiate(prefList[WorldData.palletList[WorldData.allPallet]],gameObject.transform.position,gameObject.transform.rotation,gameObject.transform);
            spawnPref.name = spawnPref.name + WorldData.allPallet.ToString();
            //gameObject.GetComponent<SpriteRenderer>().sprite = prefList[WorldData.palletList[WorldData.allPallet]];
            WorldData.allPallet += 1;
        }
    }
}
