using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform linkedPortal;
    public float cooldownTime = 2f;

    private bool isCoolingDown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!linkedPortal || isCoolingDown) return;

        if (other.CompareTag("Ball"))
        {
            // Teleport ball to linked portal
            other.transform.position = linkedPortal.position + linkedPortal.forward * 1.5f;

            // Optional: maintain velocity
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = linkedPortal.forward * rb.linearVelocity.magnitude;

            // Start cooldown on both portals
            StartCoroutine(StartCooldown());
            linkedPortal.GetComponent<PortalController>().StartCooldownFromOther();
        }
    }

    private System.Collections.IEnumerator StartCooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCoolingDown = false;
    }

    public void StartCooldownFromOther()
    {
        StartCoroutine(StartCooldown());
    }
}
