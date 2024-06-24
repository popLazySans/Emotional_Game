using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField] private AnimatedSprite deathSequence;
    [SerializeField] private List<AnimatedSprite> bodySequence;
    [SerializeField] private Light2D lightSource;
    //private SpriteRenderer spriteRenderer;
    private Movement movement;
    private new Collider2D collider; 
    private int currentAnim;

    public static Transform playerTranform;
    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
        SetBodyAnim(3);
    }
    private void Start()
    {
        movement.toTopDirection();
    }
    private void Update()
    {
        playerTranform = transform;
        // Set the new direction based on the current input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {

            movement.SetDirection(Vector2.up);
            if (CSV.Instance == null) return;
            CSV.Instance.SaveData("Up","");
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {

            movement.SetDirection(Vector2.down);
            if (CSV.Instance == null) return;
            CSV.Instance.SaveData("Down", "");
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {

            movement.SetDirection(Vector2.left);
            if (CSV.Instance == null) return;
            CSV.Instance.SaveData("Left", "");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {

            movement.SetDirection(Vector2.right);
            if (CSV.Instance == null) return;
            CSV.Instance.SaveData("Right", "");
        }

        // Rotate pacman to face the movement direction
        //float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        //transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        int directionNumber = calculateDirection();
        SetBodyAnim(directionNumber);
    }
    private void SetBodyAnim(int animNumber)
    {
        if (animNumber != currentAnim)
        {
            bodySequence[currentAnim].enabled = false;
        }
        bodySequence[animNumber].enabled = true;
        currentAnim = animNumber;
    }
    private int calculateDirection()
    {
        if (movement.direction.y == 0)
        {
            return (int)(movement.direction.x/2 + 2.5);
        }
        else
        {
            return (int)(0.5 - movement.direction.y / 2);
        }
    }
    public void ResetState()
    {
        enabled = true;
        //spriteRenderer.enabled = true;
        collider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
        SetLight(WorldData.light);
        SetBodyAnim(3);
    }
    public void SetLight(int light)
    {
        lightSource.pointLightOuterRadius = light;
        lightSource.pointLightInnerRadius = light / 2;
    }
    public void SetSpeed(float speedMultiply)
    {
        movement.speedMultiplier = speedMultiply;
    }
    public void DeathSequence()
    {
        enabled = false;
        bodySequence[currentAnim].enabled = false;
        //spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }

}
