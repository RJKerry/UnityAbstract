using UnityEngine;
using System.Collections;
using FMODUnity;


public class Bullet : MonoBehaviour
{
    // Sound for laser
    public string
    BulletFired = "event:/Turret/BulletFired";

    private void Start()
    {;

        // Start the coroutine to destroy the bullet after x seconds
        StartCoroutine(DestroyAfterDelay(3f));
        RuntimeManager.PlayOneShot(BulletFired, transform.position);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // check if of type iDamageable
        IDamageable damageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (damageableObj != null)
        {
            damageableObj.OnDamageRecieved(0.05f);
            Destroy(gameObject);
        }

        // Destroy the bullet when it hits something
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        // Destroy the bullet after the specified delay
        Destroy(gameObject);
    }

}






