using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class NodeAttach : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public int index;
    public bool useNormals = false;
    public bool runtimeUpdate = false;

    [Header("Offset")]
    public float yOffset = 0.0f;
    public bool localOffset = false;

    private Spline spline;
    private int lastSpritePointCount;
    private Vector3 lastPosition;

    void Awake()
    {
        if (spriteShapeController != null)
            spline = spriteShapeController.spline;
    }

    void Update()
    {
        if (spriteShapeController == null)
            return;

        if (!Application.isPlaying || runtimeUpdate)
        {
            spline = spriteShapeController.spline;
            if ((spline.GetPointCount() != 0) && (lastSpritePointCount != 0))
            {
                index = Mathf.Clamp(index, 0, spline.GetPointCount() - 1);
                if (spline.GetPointCount() != lastSpritePointCount)
                {
                    if (spline.GetPosition(index) != lastPosition)
                    {
                        index += spline.GetPointCount() - lastSpritePointCount;
                    }
                }
                if ((index <= spline.GetPointCount() - 1) && (index >= 0))
                {
                    if (useNormals)
                    {
                        if (spline.GetTangentMode(index) != ShapeTangentMode.Linear)
                        {
                            Vector3 lt = Vector3.Normalize(spline.GetLeftTangent(index) - spline.GetRightTangent(index));
                            Vector3 rt = Vector3.Normalize(spline.GetLeftTangent(index) - spline.GetRightTangent(index));
                            float a = Angle(Vector3.left, lt);
                            float b = Angle(lt, rt);
                            float c = a + (b * 0.5f);
                            if (b > 0)
                                c = (180 + c);
                            transform.rotation = Quaternion.Euler(0, 0, c);
                        }
                    }

                    // Get spline node position relative to its scale
                    Vector3 offsetVector = Vector3.Scale(spline.GetPosition(index), spriteShapeController.transform.localScale);

                    if (localOffset)
                    {
                        offsetVector += (Vector3)Rotate(Vector2.up, transform.localEulerAngles.z) * yOffset;
                    }
                    else
                    {
                        offsetVector += new Vector3(0, yOffset, 0);
                    }


                    transform.position = spriteShapeController.transform.position + offsetVector;
                    lastPosition = spline.GetPosition(index);
                }
            }
        }
        lastSpritePointCount = spline.GetPointCount();
    }

    private float Angle(Vector3 a, Vector3 b)
    {
        float dot = Vector3.Dot(a, b);
        float det = (a.x * b.y) - (b.x * a.y);
        return Mathf.Atan2(det, dot) * Mathf.Rad2Deg;
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float tx = v.x;
        float ty = v.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
