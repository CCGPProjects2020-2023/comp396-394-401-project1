/*  Script Name:    OptionsMenu.cs
 *  Author:         Marcus Ngooi
 *  Creation Date:  October 23, 2023
 *  Modified Date:  October 24, 2023
 *  Description:    Manages the state of the Options Menu. Will notify
 *                  the presenter when its state has changed.
 */

using UnityEngine;

public class OptionsMenu : MonoBehaviour
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
