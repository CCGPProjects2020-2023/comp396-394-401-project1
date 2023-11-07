using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public float speed = 4;
    public Rigidbody bullet;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 bulletMovement = transform.up;

        //bullet.velocity = new Vector3((bulletMovement.x * speed * 200 * Time.deltaTime), bulletMovement.y, (bulletMovement.z * speed * 200 * Time.deltaTime));
        this.transform.position += transform.forward * speed * Time.deltaTime;
    
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
