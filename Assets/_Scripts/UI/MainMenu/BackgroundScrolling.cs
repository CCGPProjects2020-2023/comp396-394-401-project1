/*  Script Name:    BackgroundScrolling.cs
 *  Author:         Marcus Ngooi
 *  Creation Date:  October 23, 2023
 *  Modified Date:  October 24, 2023
 *  Description:    Smoothly scrolls the background in the main menu scene.
 */

using UnityEngine;

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
