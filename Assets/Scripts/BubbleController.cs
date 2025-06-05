using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public float effectDuration = 5f; // ברירת מחדל למקרה חירום


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Racket"))
        {
            BallController ball = FindObjectOfType<BallController>();
            if (ball != null)
            {
                // הפעלה עם או בלי נפעום (רנדומלית לדוגמה)
                bool shouldPulse = Random.value > 0.5f;
                ball.TriggerSizeEffect(shouldPulse);
            }

            Destroy(gameObject);
            Debug.Log("[Bubble] הבועה נאספה!");
        }
    }

}
