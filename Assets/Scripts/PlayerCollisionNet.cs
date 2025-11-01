using PurrNet;
using UnityEngine;

public class PlayerCollisionNet : NetworkIdentity
{
    private PlayerProfileNet playerProfile;

    void Awake()
    {
        playerProfile = GetComponent<PlayerProfileNet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Traps"))
        {
            Debug.Log("Player entered trap: " + other.gameObject.name);
            playerProfile.TakeDamage(10);
        }
    }
    
    
}
