using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotImageAlphaController : MonoBehaviour
{
    [SerializeField] InventorySlotController _controller;
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _controller.OnItemChanged += ChangeAlpha;
    }

    private void OnDisable()
    {
        _controller.OnItemChanged -= ChangeAlpha;
    }

    private void ChangeAlpha(FishData data)
    {
        if (data.fishSprite == "")
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
        }
        else
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        }
    }
}
