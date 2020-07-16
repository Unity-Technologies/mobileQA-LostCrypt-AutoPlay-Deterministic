using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightColorSetter : MonoBehaviour, ColorSetterInterface
{
    [SerializeField] Gradient gradient = null;

    private Light2D[] lights;

    public void Refresh()
    {
        lights = GetComponentsInChildren<Light2D>();
    }

    public void SetColor(float time)
    {
        foreach (var light in lights)
            light.color = gradient.Evaluate(time);
    }
}
