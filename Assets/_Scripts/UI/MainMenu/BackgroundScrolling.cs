using System.Collections;
using System.Collections.Generic;
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
