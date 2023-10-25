/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Manages the state of the Instructions screen. Will notify
 *                          the presenter when its state has changed.
 *  Revision History:       October 23, 2023: Initial Instructions script.
 *                          October 24, 2023: Added documentation.
 */

using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField] private bool isVisible;
    public bool IsVisible { get => isVisible; set => isVisible = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
