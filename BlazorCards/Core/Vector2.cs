namespace BlazorCards;

public readonly struct Vector2(double x, double y)
{
    public Vector2() : this(0, 0) {}

    public double X => x;
    public double Y => y;

    public static Vector2 operator -(Vector2 v1, Vector2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);
}
