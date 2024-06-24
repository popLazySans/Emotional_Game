using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SetWorldData : MonoBehaviour
{
    [SerializeField] private Slider enemy1;
    [SerializeField] private Slider enemy2;
    [SerializeField] private Slider enemy3;
    [SerializeField] private Slider item1;
    [SerializeField] private Slider item2;
    [SerializeField] private Slider item3;
    [SerializeField] private Slider lightRange;
    [SerializeField] private TMP_Dropdown time;
    [SerializeField] private TMP_Dropdown map;
    [SerializeField] private TMP_InputField inputField;
    
    private int[] pallet;
    private int palletCount;
    private List<int> specialItem;
    private int process;
    // Start is called before the first frame update
    
    public void set()
    {
        WorldData.enemyCount1 = (int)enemy1.value;
        WorldData.enemyCount2 = (int)enemy2.value;
        WorldData.enemyCount3 = (int)enemy3.value;
        WorldData.itemCount1 = (int)item1.value;
        WorldData.itemCount2 = (int)item2.value;
        WorldData.itemCount3 = (int)item3.value;
        WorldData.light = (int)lightRange.value;
        WorldData.time = timeValue();
        WorldData.mapType = mapValue();
        WorldData.palletList = setPalletList();
    }
    public void setName()
    {
        WorldData.name = inputField.text;
    }
    public int timeValue()
    {
        switch (time.value)
        {
            case 0:
                return 30;
            case 1:
                return 60;
            case 2:
                return 90;
            case 3:
                return 120;
            case 4:
                return 150;
            case 5:
                return 180;
            default:
                return 30;
        }
    }
    public int mapValue()
    {
        switch (map.value)
        {
            case 0:
                return 1;
            case 1:
                return 2;
            case 2:
                return 3;
            default:
                return 1;
        }
    }
    public int[] setPalletList()
    {
        process = 1;
        setCount();
        setItem();
        pallet = new int[palletCount];
        setCounttoPallet((int)enemy1.value);
        setCounttoPallet((int)enemy2.value);
        setCounttoPallet((int)enemy3.value);
        setCounttoPallet((int)item1.value);
        setCounttoPallet((int)item2.value);
        setCounttoPallet((int)item3.value);
        return pallet;
    }
    private void setCounttoPallet(int count)
    {
        for (int posItem = 0; posItem < count; posItem++)
        {
            pallet[specialItem[0]] = process;
            specialItem.RemoveAt(0);
        }
        process++;
    }
    
    private void setItem()
    {
        int allRandom = (int)(enemy1.value+enemy2.value+enemy3.value+item1.value+item2.value+item3.value);
        specialItem = GenerateRandomNumbers(0,palletCount-1,allRandom);
    }
    private void setCount()
    {
        switch (WorldData.mapType)
        {
            case 1:
                palletCount = 598;
                break;
            case 2:
                palletCount = 684;
                break;
            case 3:
                palletCount = 608;
                break;
        }
    }
    static List<int> GenerateRandomNumbers(int min, int max, int count)
    {
        if (count > (max - min + 1))
        {
            throw new ArgumentException("Count must be less than or equal to the range of numbers.");
        }

        List<int> result = new List<int>();
        HashSet<int> generatedNumbers = new HashSet<int>();

        System.Random rand = new System.Random();
        while (result.Count < count)
        {
            int randomNumber = rand.Next(min, max + 1);
            if (!generatedNumbers.Contains(randomNumber))
            {
                result.Add(randomNumber);
                generatedNumbers.Add(randomNumber);
            }
        }

        return result;
    }
}
