/*
    Author's Name: Alexander  Maynard
    Creation Date: November 3, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: November 10, 2023
    Program Description: This script controls layer collision for the bullet, bullet velocity and destruction of the bullet.

    Revision History: 
    -November 3, 2023 
        -> Added bullet velocity/Movement and bullet destruction called by invoke with delay.
    -November 6, 2023 
        -> Added ignoring for player and bullet layers.
        -> Added more comments
    -November 10, 2023 
        -> Remove Invoke and restructed the destroying of the bullet
 */


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
        //ignore layer collision for bullet 8 and player 7
        Physics.IgnoreLayerCollision(8, 7, true);
        //invoke the bullet destruction after 2 seconds (should be enough time for now).
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //bullet movement (with reference to the upward bullet position) this helps to not depend on other rotations relative to the player
        this.transform.position += transform.forward * speed * Time.deltaTime;
    }
}
