using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;

    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction;//{ get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        nextDirection = Vector2.zero;
        if (gameObject.transform.parent != null)
        {
            transform.position = gameObject.transform.parent.transform.position;
        }
        else
        {
            transform.position = startingPosition;
        }
        direction = new Vector2(0, 0);
        do
        {
            switch ((int)Random.Range(1, 5))
            {
                case 1:
                    direction = new Vector2(0, 1);
                    break;
                case 2:
                    direction = new Vector2(0, -1);
                    break;
                case 3:
                    direction = new Vector2(-1, 0);
                    break;
                case 4:
                    direction = new Vector2(1, 0);
                    break;
            }
        } while (CheckAvailableDirection(direction) == false);
        //Debug.Log($"{gameObject.name} : Direction {direction} is {CheckAvailableDirection(direction)}");
        rigidbody.isKinematic = false;
        enabled = true;
    }
    public void toTopDirection()
    {
        direction = new Vector2(0, 1);
    }
    private bool CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one*0.5f, 0f, direction, 1f, obstacleLayer);

        // If no collider is hit then there is no obstacle in that direction
        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Update()
    {
        // Try to move in the next direction while it's queued to make movements
        // more responsive
        if (nextDirection != Vector2.zero) {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Only set the direction if the tile in that direction is available
        // otherwise we set it as the next direction so it'll automatically be
        // set when it does become available
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        // If no collider is hit then there is no obstacle in that direction
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

}
