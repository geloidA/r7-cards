using System.Diagnostics.CodeAnalysis;

namespace BlazorCards;

public readonly struct Vector2(double x, double y)
{
    public Vector2() : this(0, 0) {}

    public double X => x;
    public double Y => y;

    public static Vector2 operator -(Vector2 v1)
    {
        return new Vector2(-v1.X, -v1.Y);
    }

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static Vector2 operator *(Vector2 v1, double scalar)
    {
        return new Vector2(v1.X * scalar, v1.Y * scalar);
    }

    public static Vector2 operator /(Vector2 v1, double scalar)
    {
        return new Vector2(v1.X / scalar, v1.Y / scalar);
    }

    public static implicit operator Vector2((double X, double Y) input)
    {
        return new Vector2(input.X, input.Y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not Vector2 other) return false;
        return other.X == X && other.Y == Y;
    }
}
