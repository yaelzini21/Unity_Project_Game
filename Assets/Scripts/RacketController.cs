using UnityEngine;

public class RacketController : MonoBehaviour
{
    public float baseSpeed = 6.5f;
    public float speed;
    public float speedBoostAmount = 2f;
    public float speedBoostInterval = 1000f;

    public KeyCode up;
    public KeyCode down;
    public bool isPlayer = true;
    public float offset = 0.2f;

    private int frameCounter = 0;
    private Rigidbody rb;
    private Transform ball;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ball = GameObject.FindGameObjectWithTag("Ball").transform;
        speed = baseSpeed;
    }

    void Update()
    {
        frameCounter++;

        if (frameCounter >= speedBoostInterval)
        {
            speed += speedBoostAmount;
            frameCounter = 0;
        }

        if (this.isPlayer)
        {
            MoveByPlayer();
        }
        else
        {
            MoveByComputer();
        }
    }

    private void MoveByPlayer()
    {
        bool pressedUp = Input.GetKey(this.up);
        bool pressedDown = Input.GetKey(this.down);

        if (pressedUp)
        {
            rb.linearVelocity = Vector3.forward * speed;
        }
        else if (pressedDown)
        {
            rb.linearVelocity = Vector3.back * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void MoveByComputer()
    {
        if (this.ball.position.z > this.transform.position.z + offset)
        {
            rb.linearVelocity = Vector3.forward * speed;
        }
        else if (this.ball.position.z < this.transform.position.z - offset)
        {
            rb.linearVelocity = Vector3.back * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void ResetSpeed()
    {
        speed = baseSpeed;
        Debug.Log($"[Racket] ������ ����� �: {speed}");
    }
}