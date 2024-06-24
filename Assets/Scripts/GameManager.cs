using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private SoundEffect palletSound;
    [SerializeField] private SoundEffect countSound;
    [SerializeField] private Transform pellets;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text successText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text pauseText;
    [SerializeField] private TMP_Text liveInHead;
    [SerializeField] private Image powerSprite;
    [SerializeField] private Image lightSprite;
    [SerializeField] private Image speedSprite;
    private int ghostMultiplier = 1;
    private int lives = 3;
    private int score = 0;
    private int time = 0;
    private float sec = 0;
    public int Lives => lives;
    public int Score => score;

    private bool isStarted = false;
    public bool isPowered = false;
    public bool isLighted = false;
    public bool isSpeeded = false;
    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PauseGame();
        SetEnemy();
        NewGame();
    }

    private void Update()
    {
        sec += Time.deltaTime;
        if (sec >= 1 && pacman.gameObject.activeSelf == true)
        {
            sec = 0;
            TimeCount();
            Debug.Log("Time Count");
        }
        if (Input.anyKeyDown && isStarted == false)
        {
            PressToStart();
        }
        /*if (lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }*/
    }
    private void PauseGame()
    {
        isStarted = false;
        pauseText.enabled = true;
        Time.timeScale = 0;
    }
    private void PressToStart()
    {
        isStarted = true;
        pauseText.enabled = false;
        Time.timeScale = 1;
    }
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        SetTime(WorldData.time);
        NewRound();
    }
    private void SetEnemy()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        ghosts = new Ghost[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            ghosts[i] = enemys[i].GetComponent<Ghost>();
        }
    }
    private void NewRound()
    {
        gameOverText.enabled = false;
        successText.enabled = false;
        powerSprite.gameObject.GetComponent<AnimatedSprite>().enabled = false;
        speedSprite.gameObject.GetComponent<AnimatedSprite>().enabled = false;
        lightSprite.gameObject.GetComponent<AnimatedSprite>().enabled = false;
        timeText.color = Color.white;
        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        liveInHead.gameObject.transform.parent.parent.gameObject.SetActive(false);
        statusDeactive(powerSprite);
        statusDeactive(lightSprite);
        statusDeactive(speedSprite);
        for (int i = 0; i < ghosts.Length; i++) {
            if (ghosts[i].gameObject.activeSelf == true)
            {
               ghosts[i].ResetState();
            }
        }
        
        pacman.ResetState();
    }

    private void GameOver(string text)
    {
        gameOverText.enabled = true;
        gameOverText.text = text;
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
        liveInHead.text = "x" + lives.ToString();
        liveInHead.gameObject.transform.parent.parent.gameObject.SetActive(true);
    }
    private void SetLiveByDecrease()
    {
        SetLives(lives - 1);
    }
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }
    private void SetTime(int time)
    {
        this.time = time;
        timeText.text = time.ToString();
    }
    private void TimeCount()
    {
        if (time < 1)
        {
            SetLives(0);
            GameOver("TIME'S UP");
        }
        else
        {
            if (time == 11)
            {
                countSound.playSound(0);
                timeText.color = Color.red;
            }
            SetTime(time - 1);
        }
    }
    public void PacmanEaten()
    {
        pacman.DeathSequence();
        Invoke(nameof(DeathSound), 1f);
        
       
        if (lives > 1) {
            SetLives(lives);
            Invoke(nameof(ResetState), 3f);
            Invoke(nameof(SetLiveByDecrease), 1.5f);
        } else {
            SetLives(0);
            GameOver("GAME OVER");
        }
    }
    private void DeathSound()
    {
        palletSound.playSound(1);
    }
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        palletSound.playSound(0);
        if(CSV.Instance != null)
        { CSV.Instance.SaveData("", "Pallet"); }
        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            successText.enabled = true;
            //Time.timeScale = 0;
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(pellet.duration);
        }
        statusActive(powerSprite);
        isPowered = true;
        if(CSV.Instance != null)
        {
            CSV.Instance.SaveData("", "Power");
        }
        PelletEaten(pellet);
        NotifyPower(false);
        CancelInvoke(nameof(ResetGhostMultiplier));
        CancelInvoke(nameof(NotifyPower));
        Invoke(nameof(preNotifyPower), pellet.duration / 2);
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }
    public void preNotifyPower()
    {
        NotifyPower();
    }
    public void NotifyPower(bool isEnabled = true)
    {
        AnimatedSprite animatedSprite = powerSprite.gameObject.GetComponent<AnimatedSprite>();
        animatedSprite.enabled = isEnabled;
    }
    public void SpeedPelletEaten(SpeedPellet pellet)
    {
        statusActive(speedSprite);
        pacman.SetSpeed(1.5f);
        isSpeeded = true;
        if (CSV.Instance != null)
        {
            CSV.Instance.SaveData("", "Speed");
        }
        NotifySpeed(false);
        CancelInvoke(nameof(ResetSpeed));
        CancelInvoke(nameof(NotifySpeed));
        Invoke(nameof(preNotifySpeed), pellet.duration / 2);
        Invoke(nameof(ResetSpeed), pellet.duration);
        PelletEaten(pellet);
    }
    public void preNotifySpeed()
    {
        NotifySpeed();
    }
    public void NotifySpeed(bool isEnabled = true)
    {
        AnimatedSprite animatedSprite = speedSprite.gameObject.GetComponent<AnimatedSprite>();
        animatedSprite.enabled = isEnabled;
    }
    public void ResetSpeed()
    {
        NotifySpeed(false);
        pacman.SetSpeed(1f);
        isSpeeded = false;
        statusDeactive(speedSprite);
    }
    public void LightPelletEaten(LightPellet pellet)
    {
        statusActive(lightSprite);
        pacman.SetLight(WorldData.light * 2);
        isLighted = true;
        if (CSV.Instance != null)
        {
            CSV.Instance.SaveData("", "Light");
        }
        NotifyLight(false);
        CancelInvoke(nameof(ResetLight));
        CancelInvoke(nameof(NotifyLight));
        Invoke(nameof(preNotifyLight), pellet.duration / 2);
        Invoke(nameof(ResetLight), pellet.duration);
        PelletEaten(pellet);
    }
    public void preNotifyLight()
    {
        NotifyLight();
    }
    public void NotifyLight(bool isEnabled = true)
    {
       AnimatedSprite animatedSprite = lightSprite.gameObject.GetComponent<AnimatedSprite>();
        animatedSprite.enabled = isEnabled;
    }
    public void ResetLight()
    {
        NotifyLight(false);
        pacman.SetLight(WorldData.light);
        statusDeactive(lightSprite);
        isLighted = false;
    }
    private void statusActive(Image image)
    {
        image.enabled = true;
    }
    private void statusDeactive(Image image)
    {
        image.enabled = false;
    }
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.childCount != 0)
            {
                if (pellet.GetChild(0).gameObject.activeSelf && pellet.GetChild(0).gameObject.tag != "Enemy")
                {
                    return true;
                }
            }
            else
            {
                if (pellet.gameObject.activeSelf && pellet.gameObject.tag != "Enemy")
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        NotifyPower(false);
        ghostMultiplier = 1;
        statusDeactive(powerSprite);
        isPowered = false;
    }

}
