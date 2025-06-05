using UnityEngine;

public class BallController : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float speed;
    public float minDirection = 0.5f;
    public float accelerationAmount = 0.01f;
    public float longAccelerationAmount = 1f;
    public float maxAccelerationAmount = 2.5f;
    public float accelerationInterval = 10f;
    public float longAccelerationInterval = 200f;
    public float maxAccelerationInterval = 1000f;
    private bool menuMode = false;
    public GameObject sparksVFX;

    private Vector3 direction;
    private Rigidbody rb;
    private bool stopped = true;
    private int frameCounter = 0;

    private Vector3 originalScale;
    public Vector3 smallScale = new Vector3(0.5f, 0.5f, 0.5f); // s1
    private bool isSizeModified = false;
    private float sizeEffectDuration = 5f;
    private float sizeEffectTimer = 0f;
    private bool pulseEffect = true; // true = נפעם, false = קטן קבוע

    private Renderer ballRenderer;
    public Material powerupMaterial;       // החומר שיראה בזמן ההשפעה
    private Material originalMaterial;     // החומר המקורי



    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.speed = baseSpeed;
        this.originalScale = transform.localScale;
        ballRenderer = GetComponent<Renderer>();
        originalMaterial = ballRenderer.material;
    }

    public void MenuGo()
    {
        speed = baseSpeed / 2f; // Slow speed for visual effect
        frameCounter = 0;
        ChooseDirection();
        menuMode = true;
        stopped = false;
    }

    // This method will stop the ball movement when the menu ends
    public void MenuStop()
    {
        menuMode = false;
        stopped = true;
    }

    void FixedUpdate()
    {
        if (stopped)
            return;

        frameCounter++;

        if (!menuMode)
        {
            if (frameCounter % accelerationInterval == 0)
            {
                speed += accelerationAmount;
                speed = Mathf.Round(speed * 100f) / 100f;
            }
            if (frameCounter % longAccelerationInterval == 0)
            {
                speed += longAccelerationAmount;
                speed = Mathf.Round(speed * 100f) / 100f;
            }
            if (frameCounter >= maxAccelerationInterval)
            {
                speed += maxAccelerationAmount;
                frameCounter = 0;
            }
        }
        if (isSizeModified)
        {
            sizeEffectTimer += Time.fixedDeltaTime;

            if (pulseEffect)
            {
                float pulse = Mathf.PingPong(Time.time * 2f, 1f); // כל חצי שנייה
                transform.localScale = Vector3.Lerp(originalScale, smallScale, pulse);
            }
            else
            {
                transform.localScale = smallScale;
            }

            if (sizeEffectTimer >= sizeEffectDuration)
            {
                transform.localScale = originalScale;
                isSizeModified = false;
                sizeEffectTimer = 0f;
                ballRenderer.material = originalMaterial;
                Debug.Log("[Ball] ball back to origin size");
            }
        }

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void TriggerSizeEffect(bool pulse = true)
    {
        isSizeModified = true;
        sizeEffectTimer = 0f;
        pulseEffect = pulse;
        ballRenderer.material = powerupMaterial;
        Debug.Log("[Ball] שינוי גודל התחיל!");
    }


    public void OnTriggerEnter(Collider other)
    {
        bool hit = false;

        if (other.CompareTag("Wall"))
        {
            direction.z = -direction.z;
            hit = true;
        }

        if (other.CompareTag("Racket"))
        {
            Vector3 newDirection = (transform.position - other.transform.position).normalized;
            newDirection.x = Mathf.Sign(newDirection.x) * Mathf.Max(Mathf.Abs(newDirection.x), minDirection);
            newDirection.z = Mathf.Sign(newDirection.z) * Mathf.Max(Mathf.Abs(newDirection.z), minDirection);
            direction = newDirection;
            hit = true;
        }

        if (hit)
        {
            GameObject sparks = Instantiate(sparksVFX, transform.position, transform.rotation);
            Destroy(sparks, 4f);
        }
    }

    private void ChooseDirection()
    {
        float signX = Mathf.Sign(Random.Range(-1f, 1f));
        float signZ = Mathf.Sign(Random.Range(-1f, 1f));
        direction = new Vector3(0.5f * signX, 0, 0.5f * signZ);
    }

    public void Stop()
    {
        stopped = true;
    }

    public void Go()
    {
        speed = baseSpeed;
        frameCounter = 0;
        ChooseDirection();
        stopped = false;
        Debug.Log("[Ball] Go called, ball should now be moving");
    }
}