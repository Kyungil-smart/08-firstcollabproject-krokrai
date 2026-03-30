using UnityEngine;

public class CustomerRendererManager : MonoBehaviour
{
    SpriteRenderer[] _customerRenderer; 

    public void Init(byte length) => _customerRenderer = new SpriteRenderer[length];

    public void RastaurantManagerDI(in int i, SpriteRenderer render)
    {
        _customerRenderer[i] = render;
        _customerRenderer[i].enabled = false;
    }

    public void AllRendererDisable()
    {
        for(byte i = 0; i < _customerRenderer.Length; i++)
        {
            _customerRenderer[i].enabled = false;
        }
    }

    // 어떻게 검사 할 것 인지????

    public void RendererEnable(string s)
    {

    }
}
