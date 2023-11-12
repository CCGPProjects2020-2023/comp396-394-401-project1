/*
    Author's Name: Alexander  Maynard
    Creation Date: November 11, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: November 11, 2023
    Program Description: This is changes the cursor mode for menus.
    
    Revision History: 
    -November 11, 2023
        -> Cursor mode setting for menu.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorModeMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Unlocks the cursor from the center after the player dies or if entering the game for the first time.
        Cursor.lockState = CursorLockMode.None;
    }
}
