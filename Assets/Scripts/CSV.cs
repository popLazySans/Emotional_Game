using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class CSV : MonoBehaviour
{
    public static CSV Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void CreatePlayerCsv()
    {
        List<string[]> rowData = new List<string[]>();
        // Creating First row of titles manually..
        string[] columnNameList = new string[11];
        columnNameList[0] = "Date";
        columnNameList[1] = "time_from_start";
        columnNameList[2] = "character_Input";
        columnNameList[3] = "character_position.x";
        columnNameList[4] = "character_position.y";
        columnNameList[5] = "character_get";
        columnNameList[6] = "status_power";
        columnNameList[7] = "status_lighter";
        columnNameList[8] = "status_faster";
        columnNameList[9] = "Live";
        columnNameList[10] = "score";
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

        string playerFilePath = Application.dataPath + "/CSV/Player/" + WorldData.getCode() + ".csv";
        string enemyFilePath = Application.dataPath + "/CSV/Enemy/" + WorldData.getCode();
        // Debug.Log(filePath);

        StreamWriter outStream = File.CreateText(playerFilePath);
        Directory.CreateDirectory(enemyFilePath);
        outStream.Write(sb);
        outStream.Close();
        LoopSaveData();
    }
    private void LoopSaveData()
    {
        InvokeRepeating(nameof(SaveEmpty),1f,0.2f);
    }
    private void SaveEmpty()
    {
        SaveData("","");
    }
    public void SaveData(string action, string item)
    {
    string[] rowDataTemp = new string[11];
    List<string[]> rowData = new List<string[]>();

        // Creating First row of titles manually..
        Transform transform = Pacman.playerTranform;
        if (transform == null)
        {
            CancelInvoke(nameof(SaveEmpty));
            return;
        }
        DateTime serverTime = DateTime.Now;
        long unixTime = ((DateTimeOffset)serverTime).ToUnixTimeMilliseconds();
        rowDataTemp[0] = unixTime.ToString();
        rowDataTemp[1] = Time.time.ToString();
        rowDataTemp[2] = action;
        rowDataTemp[3] = transform.position.x.ToString();
        rowDataTemp[4] = transform.position.y.ToString();
        rowDataTemp[5] = item;
        rowDataTemp[6] = GameManager.Instance.isPowered.ToString();
        rowDataTemp[7] = GameManager.Instance.isLighted.ToString();
        rowDataTemp[8] = GameManager.Instance.isSpeeded.ToString();
        rowDataTemp[9] = GameManager.Instance.Lives.ToString();
        rowDataTemp[10] = GameManager.Instance.Score.ToString();
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


        string filePath = Application.dataPath + "/CSV/Player/" + WorldData.getCode() + ".csv";


        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.Write(sb);
        outStream.Close();

        //        Debug.Log(filePath);
    }
}
