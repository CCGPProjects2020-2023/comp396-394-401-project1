using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class WeaponHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    public ParticleSystem fireParticleSystem;
    public Transform aimPoint;
    public LayerMask collisionLayers;

    HPHandler hPHandler;

    float lastTimeFired = 0f;

    private void Awake()
    {
        hPHandler = GetComponent<HPHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Fire(Vector3 aimForwardVector) {
        if (Time.time - lastTimeFired < 0.15f) {
            return;
        }

        StartCoroutine(FireEffectCO());

        Runner.LagCompensation.Raycast(aimPoint.position, aimForwardVector, 100, Object.InputAuthority, out var hitinfo, collisionLayers, HitOptions.IncludePhysX);

        float hitDistance = 100;
        bool isHitOtherPlayer = false;

        if(hitinfo.Distance > 0)
        {
            hitDistance = hitinfo.Distance;
        }

        if (hitinfo.Hitbox != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit hitbox {hitinfo.Hitbox.transform.root.name}");

            if (Object.HasStateAuthority)
                hitinfo.Hitbox.transform.root.GetComponent<HPHandler>().OnTakeDamage(); //TODO: Need to do something like this but on the enemy weapon - Will need to add a HPHandler on the enemy as well

            isHitOtherPlayer = true;
        }
        else if (hitinfo.Collider != null) {
            Debug.Log($"{Time.time} {transform.name} hit hitbox {hitinfo.Collider.transform.root.name}");
        }

        if (isHitOtherPlayer)
        {
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.red, 1);
        }
        else { 
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.green, 1);

        }

        lastTimeFired = Time.time;
    }

    IEnumerator FireEffectCO() {     
        isFiring = true;

        fireParticleSystem.Play();

        yield return new WaitForSeconds(0.09f);

        isFiring = false;
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if(hPHandler.isDead) return;    

        if(GetInput(out NetworkInputData networkInputData))
        {
            if(networkInputData.isFirePressed)
            {
                Fire(networkInputData.aimForwardVector);
            }
        }
    }

    static void OnFireChanged(Changed<WeaponHandler> changed)
    {
        //Debug.Log($"{Time.time} OnFireChanged value {changed.Behaviour.isFiring}");

        bool isFiringCurrent = changed.Behaviour.isFiring;
        changed.LoadOld();

        bool isFiringOld = changed.Behaviour.isFiring;

        if( isFiringCurrent && !isFiringOld) { changed.Behaviour.OnFireRemote(); }
    }

    void OnFireRemote()
    {
        if(!Object.HasInputAuthority) fireParticleSystem.Play();
    }
}