using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpriteSettings : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSettings(Sprite sprite, Vector2 offset)
    {
        transform.localPosition = offset;
        spriteRenderer.sprite = sprite;
    }
}
