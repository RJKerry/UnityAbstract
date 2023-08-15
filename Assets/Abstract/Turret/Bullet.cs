using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        // Start the coroutine to destroy the bullet after x seconds
        StartCoroutine(DestroyAfterDelay(3f));
    }

    private void OnTriggerEnter(Collider collision)
    {
        // check if of type iDamageable
        IDamageable damageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (damageableObj != null)
        {
            Debug.Log("Player hit by bullet");
            damageableObj.OnDamageRecieved(1f);
        }

        // Destroy the bullet when it hits something
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        // Destroy the bullet after the specified delay
        Destroy(gameObject);
    }

    private void DamagePlayer(GameObject player)
    {
        return;
        // Implement your damage logic here
    }
}






