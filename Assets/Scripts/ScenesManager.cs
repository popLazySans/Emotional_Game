using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour
{
    [SerializeField]
    private List<string> sceneName = new List<string>();
    public void play()
    {
        Debug.Log("Downed");
        CSV.Instance.CreatePlayerCsv();
        SceneManager.LoadScene(sceneName[WorldData.mapType - 1]);
    }
    public void start()
    {
        WorldData.allPallet = 0;
        Destroy(GameObject.Find("GameManager"));
        CancelInvoke();
        SceneManager.LoadScene("Start");
    }
    public void menu()
    {
        WorldData.allPallet = 0;
        Destroy(GameObject.Find("GameManager"));
        CancelInvoke();
        SceneManager.LoadScene("Menu");
    }
    public void howToPlay()
    {
        CancelInvoke();
        SceneManager.LoadScene("HowToPlay");
    }
    public void detail()
    {
        CancelInvoke();
        SceneManager.LoadScene("Detail");
    }
    public void tutorial()
    {
        CancelInvoke();
        WorldData.time = 180;
        WorldData.light = 10;
        SceneManager.LoadScene("Tutorial");
    }
    public void exit()
    {
        Application.Quit();
    }
}
