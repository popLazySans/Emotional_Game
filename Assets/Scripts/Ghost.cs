using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    //public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        //home = GetComponent<GhostHome>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);

        //Set spawn pos when reset 
        //movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        /*if (home != initialBehavior) {
            home.Disable();
        }*/

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                GameManager.Instance.GhostEaten(this);
                gameObject.SetActive(false);
                SoundEffect soundEffect = gameObject.GetComponent<SoundEffect>();
                soundEffect.playSound(0);
            } else {
                GameManager.Instance.PacmanEaten();
            }
        }
    }

}
