/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Smoothly scrolls the background in the main menu scene.
 *  Revision History:       October 23, 2023: Initial BackgroundScrolling script.
 *                          October 24, 2023: Added documentation.
 */

using UnityEngine;

/// <summary>
/// A script to smoothly scroll the background in main menu, options menu, and instructions screen.
/// </summary>
public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.05f;

    private Renderer renderer;
    private float offset = 0f;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        offset = (Time.deltaTime * scrollSpeed) % 1;
        renderer.material.mainTextureOffset += new Vector2(offset, 0);
    }
}
