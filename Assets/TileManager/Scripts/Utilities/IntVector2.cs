using System;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class IntVector2
{
    public int x, y;

    public double magnitude
    {
        get
        {
            return Math.Sqrt((x * x + y * y));
        }
    }

    #region Static Properties
    public static IntVector2 up = new IntVector2(0, 1);
    public static IntVector2 down = new IntVector2(0, -1);
    public static IntVector2 left = new IntVector2(-1, 0);
    public static IntVector2 right = new IntVector2(1, 0);
    public static IntVector2 one = new IntVector2(1, 1);
    public static IntVector2 zero = new IntVector2(0, 0);
    public static IntVector2 positiveInfinity = new IntVector2(int.MaxValue, int.MaxValue);
    public static IntVector2 negativeInfinity = new IntVector2(int.MinValue, int.MinValue);
    #endregion

    public IntVector2(int a_x, int a_y)
    {
        x = a_x;
        y = a_y;
    }

    #region Arithmetic Operators
    // Vector + Vector
    public static IntVector2 operator +(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        return new IntVector2(a_lhs.x + a_rhs.x, a_lhs.y + a_rhs.y);
    }

    // Vector + Int
    public static IntVector2 operator +(IntVector2 a_lhs, int a_rhs)
    {
        return new IntVector2(a_lhs.x + a_rhs, a_lhs.y + a_rhs);
    }

    // Vector - Vector
    public static IntVector2 operator -(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        return new IntVector2(a_lhs.x - a_rhs.x, a_lhs.y - a_rhs.y);
    }

    // Vector - Int
    public static IntVector2 operator -(IntVector2 a_lhs, int a_rhs)
    {
        return new IntVector2(a_lhs.x - a_rhs, a_lhs.y - a_rhs);
    }

    // Vector * Vector
    public static IntVector2 operator *(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        return new IntVector2(a_lhs.x * a_rhs.x, a_lhs.y * a_rhs.y);
    }

    // Vector * Int
    public static IntVector2 operator *(IntVector2 a_lhs, int a_rhs)
    {
        return new IntVector2(a_lhs.x * a_rhs, a_lhs.y * a_rhs);
    }

    // Vector / Vector
    public static IntVector2 operator /(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        return new IntVector2(a_lhs.x / a_rhs.x, a_lhs.y / a_rhs.y);
    }

    // Vector * Int
    public static IntVector2 operator /(IntVector2 a_lhs, int a_rhs)
    {
        return new IntVector2(a_lhs.x / a_rhs, a_lhs.y / a_rhs);
    }

    // Vector == Vector
    public static bool operator ==(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        return (a_lhs.x == a_rhs.x && a_lhs.y == a_rhs.y);
    }

    // Vector != Vector
    public static bool operator !=(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        return !(a_lhs.x == a_rhs.x && a_lhs.y == a_rhs.y);
    }

    public override bool Equals(object obj)
    {
        return (this == obj);
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }
    #endregion

    // Vector.ToString()
    public override string ToString()
    {
        return (x.ToString() + ", " + y.ToString());
    }

    // Returns vector made of largest components of two vectors
    public static IntVector2 Max(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        int maxX;
        if (a_lhs.x > a_rhs.x)
            maxX = a_lhs.x;
        else
            maxX = a_rhs.x;

        int maxY;
        if (a_lhs.y > a_rhs.y)
            maxY = a_lhs.y;
        else
            maxY = a_rhs.y;

        return new IntVector2(maxX, maxY);
    }

    // Returns vector made of smallest components of two vectors
    public static IntVector2 Min(IntVector2 a_lhs, IntVector2 a_rhs)
    {
        int minX;
        if (a_lhs.x < a_rhs.x)
            minX = a_lhs.x;
        else
            minX = a_rhs.x;

        int minY;
        if (a_lhs.y < a_rhs.y)
            minY = a_lhs.y;
        else
            minY = a_rhs.y;

        return new IntVector2(minX, minY);
    }

    // Sets the x and y components of an existing IntVector2
    public void Set(int a_x, int a_y)
    {
        x = a_x;
        y = a_y;
    }
}
