using UnityEngine;

[ExecuteInEditMode]
public class MaterialVectorSetter : MonoBehaviour
{
    [SerializeField] string valueName = null;
    [SerializeField] Material[] materials = null;

    private void Update()
    {
        foreach (var material in materials)
            material.SetVector(valueName, transform.position);
    }
}
