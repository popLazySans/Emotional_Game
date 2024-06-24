using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class EnemyCSV : MonoBehaviour
{
    private void Start()
    {
        CreateEnemyCsv();
    }
    private void OnDisable()
    {
    }
    public void CreateEnemyCsv()
    {
        List<string[]> rowData = new List<string[]>();
        // Creating First row of titles manually..
        string[] columnNameList = new string[3];
        columnNameList[0] = "time";
        columnNameList[1] = "enemyPos.X";
        columnNameList[2] = "enemyPos.Y";
        //add row
        rowData.Add(columnNameList);

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        string filePath = Application.dataPath + $"/CSV/Enemy/{WorldData.getCode()}/" + gameObject.name + ".csv";
        // Debug.Log(filePath);

        StreamWriter outStream = File.CreateText(filePath);
        outStream.Write(sb);
        outStream.Close();
        LoopSaveData();
    }
    private void LoopSaveData()
    {
        InvokeRepeating(nameof(SaveData), 1f, 0.2f);
    }

    public void SaveData()
    {
        string[] rowDataTemp = new string[3];
        List<string[]> rowData = new List<string[]>();

        // Creating First row of titles manually..
        DateTime serverTime = DateTime.Now;
        long unixTime = ((DateTimeOffset)serverTime).ToUnixTimeMilliseconds();
        Vector3 objPos = gameObject.transform.position;
        rowDataTemp[0] = unixTime.ToString();
        rowDataTemp[1] = objPos.x.ToString();
        rowDataTemp[2] = objPos.y.ToString();
        //add row
        rowData.Add(rowDataTemp);

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = Application.dataPath + $"/CSV/Enemy/{WorldData.getCode()}/" + gameObject.name + ".csv";


        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.Write(sb);
        outStream.Close();

        //        Debug.Log(filePath);
    }
}
