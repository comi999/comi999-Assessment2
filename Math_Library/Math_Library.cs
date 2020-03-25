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

        public Vector3 Normalise()
        {
            float magnitude = (float)Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
            i /= magnitude;
            j /= magnitude;
            k /= magnitude;
            return new Vector3(i, j, k);
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
            i1 = 0; j1 = 0; k1 = 0;
            i2 = 0; j2 = 0; k2 = 0;
            i3 = 0; j3 = 0; k3 = 0;
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
            i2 = v2.i; j2 = v2.j; k2 = v3.k;
            i3 = v3.i; j3 = v3.j; k3 = v3.k;
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

        public static Vector3 operator *(Vector3 lhs, Matrix3 rhs)
        {
            Vector3 output = new Vector3();

            Vector3 rhsCol1 = new Vector3(rhs.i1, rhs.i2, rhs.i3);
            Vector3 rhsCol2 = new Vector3(rhs.j1, rhs.j2, rhs.j3);
            Vector3 rhsCol3 = new Vector3(rhs.k1, rhs.k2, rhs.k3);

            output.i = lhs * rhsCol1;
            output.j = lhs * rhsCol2;
            output.k = lhs * rhsCol3;

            return output;
        }


        public override string ToString()
        {
            return i1 + ", " + j1 + ", " + k1 + ", " + i2 + ", " + j2 + ", " + k2 + ", " + i3 + ", " + j3 + ", " + k3;
        }
    }
}
