using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteToImage : MonoBehaviour
{
    private Image image;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (image.sprite == spriteRenderer.sprite) return;
        image.sprite = spriteRenderer.sprite;
    }
}
