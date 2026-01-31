using System.Runtime.InteropServices;

namespace Variable.Curve;

/// <summary>
///     Represents a 1D Cubic Bezier curve defined by four control points.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct BezierCurve
{
    /// <summary>The start point (P0).</summary>
    public float Start;

    /// <summary>The first control point (P1).</summary>
    public float Control1;

    /// <summary>The second control point (P2).</summary>
    public float Control2;

    /// <summary>The end point (P3).</summary>
    public float End;

    /// <summary>Constructs a new Cubic Bezier Curve.</summary>
    public BezierCurve(float start, float control1, float control2, float end)
    {
        Start = start;
        Control1 = control1;
        Control2 = control2;
        End = end;
    }
}
