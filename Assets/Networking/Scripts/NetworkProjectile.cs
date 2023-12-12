/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles network projectiles
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkProjectile : NetworkBehaviour
{
    public GameObject explosionParticleSystemPrefab;

    public LayerMask collisionLayers;
    
    PlayerRef thrownByPlayerRef;
    string thrownByPlayerName;

    TickTimer explodeTickTimer = TickTimer.None;

    List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

    NetworkObject networkObject;
    NetworkRigidbody networkRigidbody;

    /// <summary>
    /// Handles a projectile's movement and velocity when it is thrown
    /// </summary>
    /// <param name="throwForce"></param>
    /// <param name="thrownByPlayerRef"></param>
    /// <param name="thrownByPlayerName"></param>
    public void Throw(Vector3 throwForce, PlayerRef thrownByPlayerRef, string thrownByPlayerName) {
        networkObject = GetComponent<NetworkObject>();
        networkRigidbody = GetComponent<NetworkRigidbody>();

        networkRigidbody.Rigidbody.AddForce(throwForce, ForceMode.Impulse);

        this.thrownByPlayerRef = thrownByPlayerRef;
        this.thrownByPlayerName = thrownByPlayerName;

        explodeTickTimer = TickTimer.CreateFromSeconds(Runner, 2);

    }

    /// <summary>
    /// Similar to the update method from unity - this updates the player's hitcount 
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        if(Object.HasStateAuthority)
        {
            if (explodeTickTimer.Expired(Runner)) {
                int hitCount = Runner.LagCompensation.OverlapSphere(transform.position, 4, thrownByPlayerRef, hits, collisionLayers);

                for(int i = 0; i < hitCount; i++)
                {
                    HPHandler hpHandler = hits[i].Hitbox.transform.root.GetComponent<HPHandler>();
                    if(hpHandler != null)
                    {
                        hpHandler.OnTakeDamage(thrownByPlayerName, 100);
                    }
                }

                Runner.Despawn(networkObject);

                explodeTickTimer = TickTimer.None;
            }                                      
        }
    }

    /// <summary>
    /// Instantiates particle system when the object is despawned.
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="hasState"></param>
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        MeshRenderer projectileMesh = GetComponentInChildren<MeshRenderer>();
        Instantiate(explosionParticleSystemPrefab, projectileMesh.transform.position, Quaternion.identity);
    }
}