
public static class WorldData
{
    public static int enemyCount1 { get; set; }
    public static int enemyCount2 { get; set; }
    public static int enemyCount3 { get; set; }
    public static int itemCount1 { get; set; }
    public static int itemCount2 { get; set; }
    public static int itemCount3 { get; set; }
    public static int light { get; set; }
    public static int time { get; set; }
    public static int mapType { get; set; }
    public static int allPallet { get; set; } = 0;
    public static int[] palletList { get; set; }
    public static string name { get; set; }
    public static string getCode()
    {
        string code =
            name.ToString() + "_" +
            enemyCount1.ToString() + "_" +
            enemyCount2.ToString() + "_" +
            enemyCount3.ToString() + "_" +
            itemCount1.ToString() + "_" +
            itemCount2.ToString() + "_" +
            itemCount3.ToString() + "_" +
            light.ToString() + "_" +
            time.ToString() + "_" +
            mapType.ToString();
        return code;
    }
   
}
