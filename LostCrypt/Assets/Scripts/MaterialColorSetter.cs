using UnityEngine;

public class MaterialColorSetter : MonoBehaviour, ColorSetterInterface
{
    [GradientUsage(true)]
    [SerializeField] Gradient gradient = null;
    [SerializeField] string colorName = null;
    [SerializeField] Material[] materials = null;

    public void Refresh()
    {
    }

    public void SetColor(float time)
    {
        foreach (var material in materials)
            material.SetColor(colorName, gradient.Evaluate(time));
    }
}
