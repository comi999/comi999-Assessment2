using System;

namespace Math_Library
{
    public class Vector3
    {
        public float i, j, k;
        public Vector3()
        {
            i = 0;
            j = 0;
            k = 0;
        }

        public Vector3(float i, float j, float k)
        {
            this.i = i;
            this.j = j;
            this.k = k;
        }

        //Operator overloads +,-,*,/
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.i + rhs.i, lhs.j + rhs.j, lhs.k + rhs.k);
        }

        /// <summary>
        /// Adds RHS to every value in LHS then returns LHS.
        /// </summary>
        public static Vector3 operator +(Vector3 lhs, float rhs)
        {
            return lhs + new Vector3(rhs, rhs, rhs);
        }

        public static Vector3 operator *(float lhs, Vector3 rhs)
        {
            return new Vector3(lhs * rhs.i, lhs * rhs.j, lhs * rhs.k);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return lhs + -1 * rhs;
        }

        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            return rhs * lhs;
        }

        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.i / rhs, lhs.j / rhs, lhs.k / rhs);
        }

        /// <summary>
        /// Finds the cross product of LHS and RHS.
        /// </summary>
        public static Vector3 operator %(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.j * rhs.k - lhs.k * rhs.j, lhs.k * rhs.i - lhs.i * rhs.k, lhs.i * rhs.j - lhs.j * rhs.i);
        }

        /// <summary>
        /// Finds the dot product of LHS and RHS.
        /// </summary>
        public static float operator *(Vector3 lhs, Vector3 rhs)
        {
            return lhs.i * rhs.i + lhs.j * rhs.j + lhs.k * rhs.k;
        }

        public override string ToString()
        {
            return i + ", " + j + ", " + k;
        }

        //Vector functions
        public float Magnitude()
        {
            return (float)Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
        }

        public float MagnitudeSqrd()
        {
            return (float)(Math.Pow(i, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
        }

        public void Normalise()
        {
            float magnitude = (float)Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
            i /= magnitude;
            j /= magnitude;
            k /= magnitude;
        }

        public void RotateX(double theta)
        {
            Vector3 temp = (Vector3)MemberwiseClone();
            temp = new Matrix3(1, 0, 0, 0, (float)Math.Cos(theta), (float)Math.Sin(theta), 0, -(float)Math.Sin(theta), (float)Math.Cos(theta)) * temp;
            i = temp.i;
            j = temp.j;
            k = temp.k;
        }

        public void RotateY(double theta)
        {
            Vector3 temp = (Vector3)MemberwiseClone();
            temp = new Matrix3((float)Math.Cos(theta), 0, -(float)Math.Sin(theta), 0, 1, 0, (float)Math.Sin(theta), 0, (float)Math.Cos(theta)) * temp;
            i = temp.i;
            j = temp.j;
            k = temp.k;
        }

        public void RotateZ(double theta)
        {
            Vector3 temp = (Vector3)MemberwiseClone();
            temp = new Matrix3((float)Math.Cos(theta), (float)Math.Sin(theta), 0, -(float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 1) * temp;
            i = temp.i;
            j = temp.j;
            k = temp.k;
        }
    }

    public class Matrix3
    {
        //Matrix entries
        public float i1, j1, k1,
                     i2, j2, k2,
                     i3, j3, k3;

        public Matrix3()
        {
            i1 = 1; j1 = 0; k1 = 0;
            i2 = 0; j2 = 1; k2 = 0;
            i3 = 0; j3 = 0; k3 = 1;
        }

        public Matrix3(float i1, float j1, float k1, float i2, float j2, float k2, float i3, float j3, float k3)
        {
            this.i1 = i1; this.j1 = j1; this.k1 = k1;
            this.i2 = i2; this.j2 = j2; this.k2 = k2;
            this.i3 = i3; this.j3 = j3; this.k3 = k3;
        }

        public Matrix3(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            i1 = v1.i; j1 = v1.j; k1 = v1.k;
            i2 = v2.i; j2 = v2.j; k2 = v2.k;
            i3 = v3.i; j3 = v3.j; k3 = v3.k;
        }



        public static Matrix3 operator +(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(lhs.i1 + rhs.i1, lhs.j1 + rhs.j1, lhs.k1 + rhs.k1,
                               lhs.i2 + rhs.i2, lhs.j2 + rhs.j2, lhs.k2 + rhs.k2,
                               lhs.i3 + rhs.i3, lhs.j3 + rhs.j3, lhs.k3 + rhs.k3);
        }

        public static Matrix3 operator -(Matrix3 lhs, Matrix3 rhs)
        {
            return lhs + -1 * rhs;
        }

        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            Matrix3 output = new Matrix3();

            Vector3 lhsRow1 = new Vector3(lhs.i1, lhs.j1, lhs.k1);
            Vector3 lhsRow2 = new Vector3(lhs.i2, lhs.j2, lhs.k2);
            Vector3 lhsRow3 = new Vector3(lhs.i3, lhs.j3, lhs.k3);

            Vector3 rhsCol1 = new Vector3(rhs.i1, rhs.i2, rhs.i3);
            Vector3 rhsCol2 = new Vector3(rhs.j1, rhs.j2, rhs.j3);
            Vector3 rhsCol3 = new Vector3(rhs.k1, rhs.k2, rhs.k3);

            output.i1 = lhsRow1 * rhsCol1;
            output.j1 = lhsRow1 * rhsCol2;
            output.k1 = lhsRow1 * rhsCol3;
            output.i2 = lhsRow2 * rhsCol1;
            output.j2 = lhsRow2 * rhsCol2;
            output.k2 = lhsRow2 * rhsCol3;
            output.i3 = lhsRow3 * rhsCol1;
            output.j3 = lhsRow3 * rhsCol2;
            output.k3 = lhsRow3 * rhsCol3;

            return output;
        }

        public static Vector3 operator *(Matrix3 lhs, Vector3 rhs)
        {
            Vector3 output = new Vector3();

            Vector3 lhsCol1 = new Vector3(lhs.i1, lhs.i2, lhs.i3);
            Vector3 lhsCol2 = new Vector3(lhs.j1, lhs.j2, lhs.j3);
            Vector3 lhsCol3 = new Vector3(lhs.k1, lhs.k2, lhs.k3);

            output.i = rhs * lhsCol1;
            output.j = rhs * lhsCol2;
            output.k = rhs * lhsCol3;

            return output;
        }

        public static Matrix3 operator *(float lhs, Matrix3 rhs)
        {
            return new Matrix3(lhs * rhs.i1, lhs * rhs.j1, lhs * rhs.k1,
                               lhs * rhs.i2, lhs * rhs.j2, lhs * rhs.k2,
                               lhs * rhs.i3, lhs * rhs.j3, lhs * rhs.k3);
        }

        public void Transpose()
        {
            //Classes are reference types, so I used The following function to copy individual members into a new Matrix3 temp.
            Matrix3 temp = (Matrix3)MemberwiseClone();

            i1 = temp.i1;
            j1 = temp.i2;
            k1 = temp.i3;
            i2 = temp.j1;
            j2 = temp.j2;
            k2 = temp.j3;
            i3 = temp.k1;
            j3 = temp.k2;
            k3 = temp.k3;
        }

        public void RotateX(float theta)
        {
            Matrix3 temp = (Matrix3)MemberwiseClone();
            temp = new Matrix3(1, 0, 0, 0, (float)Math.Cos(theta), (float)Math.Sin(theta), 0, -(float)Math.Sin(theta), (float)Math.Cos(theta)) * temp;
            i1 = temp.i1;
            j1 = temp.j1;
            k1 = temp.k1;
            i2 = temp.i2;
            j2 = temp.j2;
            k2 = temp.k2;
            i3 = temp.i3;
            j3 = temp.j3;
            k3 = temp.k3;
        }

        public void RotateY(float theta)
        {
            Matrix3 temp = (Matrix3)MemberwiseClone();
            temp = new Matrix3((float)Math.Cos(theta), 0, -(float)Math.Sin(theta), 0, 1, 0, (float)Math.Sin(theta), 0, (float)Math.Cos(theta)) * temp;
            i1 = temp.i1;
            j1 = temp.j1;
            k1 = temp.k1;
            i2 = temp.i2;
            j2 = temp.j2;
            k2 = temp.k2;
            i3 = temp.i3;
            j3 = temp.j3;
            k3 = temp.k3;
        }

        public void RotateZ(float theta)
        {
            Matrix3 temp = (Matrix3)MemberwiseClone();
            temp = new Matrix3((float)Math.Cos(theta), (float)Math.Sin(theta), 0, -(float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 1) * temp;
            i1 = temp.i1;
            j1 = temp.j1;
            k1 = temp.k1;
            i2 = temp.i2;
            j2 = temp.j2;
            k2 = temp.k2;
            i3 = temp.i3;
            j3 = temp.j3;
            k3 = temp.k3;
        }

        public override string ToString()
        {
            return i1 + ", " + j1 + ", " + k1 + ", " + i2 + ", " + j2 + ", " + k2 + ", " + i3 + ", " + j3 + ", " + k3;
        }
    }

    public static class NumberExtensions
    {
        public static float ConvertToRadians(this float degrees)
        {
            return degrees / 180 * (float)Math.PI ;
        }

        public static float ConvertToDegrees(this float radians)
        {
            return radians * 180 / (float)Math.PI;
        }
    }
}
