using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Xml.Schema;

namespace Math_Library
{

    //---Vector3 (2D vectors held in z = 0 plane, in 3D space)---
    public class Vector3
    {
        //Vector entries. i, j, k represent unit vector components pointing in x+, y+, z+ directions
        public float i, j, k;

        //For unit tests
        public float x, y, z;

        #region ---'Vector3' CONSTRUCTORS---
        //Default constructor makes 3D 0 vector
        public Vector3()
        {
            i = 0;
            j = 0;
            k = 0;

            //For unit tests
            x = 0;
            y = 0;
            z = 0;
        }

        //Custom vector constructor
        public Vector3(float i, float j, float k)
        {
            this.i = i;
            this.j = j;
            this.k = k;

            //For unit tests
            x = i;
            y = j;
            z = k;
        }
        #endregion

        #region ---'Vector3' OPERATOR OVERLOADS (+,-,*,/,%)---
        //Vector addition
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

        //Scalar multiplication
        public static Vector3 operator *(float lhs, Vector3 rhs)
        {
            return new Vector3(lhs * rhs.i, lhs * rhs.j, lhs * rhs.k);
        }

        //For unit testing
        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            return rhs * lhs;
        }

        //Vector subtraction
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return lhs + -1 * rhs;
        }

        //Scalar division
        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            float divisor = 1 / rhs;
            return new Vector3(divisor * lhs.i, divisor * lhs.j, divisor * lhs.k);
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

        //ToString overload to print Vectors
        public override string ToString()
        {
            return i + ", " + j + ", " + k;
        }
        #endregion

        #region ---'Vector3' FUNCTIONS---
        //Returns the vectors magnitude
        public float Magnitude()
        {
            return (float)Math.Sqrt(i * i + j * j + k * k);
        }

        //Returns the square of a vectors magnitude
        public float MagnitudeSqrd()
        {
            return i * i + j * j + k * k;
        }

        //Changes attached vector to a unit vector pointing in the same direction
        public Vector3 Normalise()
        {
            float inverse_magnitude = 1 / (float)Math.Sqrt(i * i + j * j + k * k);
            i *= inverse_magnitude;
            j *= inverse_magnitude;
            k *= inverse_magnitude;

            //For unit tests
            x = i;
            y = j;
            z = k;
            return this;
        }

        //For unit testing
        public void Normalize()
        {
            Vector3 temp = (Vector3)MemberwiseClone();
            temp = temp.Normalise();

            i = temp.i;
            j = temp.j;
            k = temp.k;
            x = temp.x;
            y = temp.y;
            z = temp.z;
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

        public void Reset()
        {
            i = 0;
            j = 0;
            k = 0;
        }

        //For unit testing
        public float Dot(Vector3 rhs)
        {
            return this * rhs;
        }

        public Vector3 Cross(Vector3 rhs)
        {
            return this % rhs;
        }
        #endregion
    }

    //--Vector4 (3D vectors held in w = 0 sub-space, in 4D space)---
    public class Vector4
    {
        //Vector entries. i, j, k, w represent unit vector components pointing in x+, y+, z+, w+ directions
        public float i, j, k, w;

        //For unit tests
        public float x, y, z;

        #region ---'Vector4' CONSTRUCTORS---
        //Default constructor makes 4D 0 vector
        public Vector4()
        {
            i = 0;
            j = 0;
            k = 0;
            w = 1;

            //For unit tests
            x = 0;
            y = 0;
            z = 0;
        }

        //Custom vector constructor
        public Vector4(float i, float j, float k, float w)
        {
            this.i = i;
            this.j = j;
            this.k = k;
            this.w = w;

            //For unit tests
            x = i;
            y = j;
            z = k;
        }
        #endregion

        #region ---'Vector4' OPERATOR OVERLOADS (+,-,*,/,%)---
        //Vector addition
        public static Vector4 operator +(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.i + rhs.i, lhs.j + rhs.j, lhs.k + rhs.k, lhs.w + rhs.w);
        }

        /// <summary>
        /// Adds RHS to every value in LHS then returns LHS.
        /// </summary>
        public static Vector4 operator +(Vector4 lhs, float rhs)
        {
            return lhs + new Vector4(rhs, rhs, rhs, rhs);
        }

        //Scalar multiplication
        public static Vector4 operator *(float lhs, Vector4 rhs)
        {
            return new Vector4(lhs * rhs.i, lhs * rhs.j, lhs * rhs.k, lhs * rhs.w);
        }

        //For unit testing
        public static Vector4 operator *(Vector4 lhs, float rhs)
        {
            return rhs * lhs;
        }

        //Vector subtraction
        public static Vector4 operator -(Vector4 lhs, Vector4 rhs)
        {
            return lhs + -1 * rhs;
        }

        //Scalar division
        public static Vector4 operator /(Vector4 lhs, float rhs)
        {
            float divisor = 1 / rhs;
            return new Vector4(divisor * lhs.i, divisor * lhs.j, divisor * lhs.k, divisor * lhs.w);
        }

        //Vector4's don't have a defined cross product, so can only be used for homogenous Vector3's.
        /// <summary>
        /// Finds the cross product of LHS and RHS.
        /// </summary>
        public static Vector4 operator %(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.j * rhs.k - lhs.k * rhs.j, lhs.k * rhs.i - lhs.i * rhs.k, lhs.i * rhs.j - lhs.j * rhs.i, 0);
        }

        /// <summary>
        /// Finds the dot product of LHS and RHS.
        /// </summary>
        public static float operator *(Vector4 lhs, Vector4 rhs)
        {
            return lhs.i * rhs.i + lhs.j * rhs.j + lhs.k * rhs.k + lhs.w * rhs.w;
        }

        //ToString overload to print Vectors
        public override string ToString()
        {
            return i + ", " + j + ", " + k + ", " + w;
        }
        #endregion

        #region ---'Vector4' FUNCTIONS---
        //Returns the vectors magnitude
        public float Magnitude()
        {
            return (float)Math.Sqrt(i * i + j * j + k * k + w * w);
        }

        //Returns the square of a vectors magnitude
        public float MagnitudeSqrd()
        {
            return i * i + j * j + k * k + w * w;
        }

        //Changes attached vector to a unit vector pointing in the same direction
        public Vector4 Normalise()
        {
            float inverse_magnitude = 1 / (float)Math.Sqrt(i * i + j * j + k * k + w * w);
            i *= inverse_magnitude;
            j *= inverse_magnitude;
            k *= inverse_magnitude;
            w *= inverse_magnitude;

            //For unit testing
            x = i;
            y = j;
            z = k;
            return this;
        }

        //For unit Testing
        public void Normalize()
        {
            Vector4 temp = (Vector4)MemberwiseClone();
            temp = temp.Normalise();

            i = temp.i;
            j = temp.j;
            k = temp.k;
            w = temp.w;
            x = temp.x;
            y = temp.y;
            z = temp.z;
        }

        public void RotateX(double theta)
        {
            Vector4 temp = (Vector4)MemberwiseClone();
            temp = new Matrix4(1, 0, 0, 0, 0, (float)Math.Cos(theta), -(float)Math.Sin(theta), 0, 0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 0, 1) * temp;
            i = temp.i;
            j = temp.j;
            k = temp.k;
            w = temp.w;
        }

        public void RotateY(double theta)
        {
            Vector4 temp = (Vector4)MemberwiseClone();
            temp = new Matrix4((float)Math.Cos(theta), 0, (float)Math.Sin(theta), 0, 0, 1, 0, 0, -(float)Math.Sin(theta), 0, (float)Math.Cos(theta), 0, 0, 0, 0, 1) * temp;
            i = temp.i;
            j = temp.j;
            k = temp.k;
            w = temp.w;
        }

        public void RotateZ(double theta)
        {
            Vector4 temp = (Vector4)MemberwiseClone();
            temp = new Matrix4((float)Math.Cos(theta), -(float)Math.Sin(theta), 0, 0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1) * temp;
            i = temp.i;
            j = temp.j;
            k = temp.k;
            w = temp.w;
        }

        //For unit testing
        public float Dot(Vector4 rhs)
        {
            return this * rhs;
        }

        public Vector4 Cross(Vector4 rhs)
        {
            return this % rhs;
        }
        #endregion
    }

    //---Matrix3 (Represents 2D plane at z = 0, in 3D space)---
    public class Matrix3
    {
        //Matrix entries. i, j, k represent unit vector components pointing in x+, y+, z+ directions
        public float
            i1, j1, k1,
            i2, j2, k2,
            i3, j3, k3;

        //For unit tests
        public float
            m1, m2, m3,
            m4, m5, m6,
            m7, m8, m9;

        #region ---'Matrix3' CONSTRUCTORS---
        //Default constructor creates 3x3 Identity Matrix
        public Matrix3()
        {
            i1 = 1; j1 = 0; k1 = 0;
            i2 = 0; j2 = 1; k2 = 0;
            i3 = 0; j3 = 0; k3 = 1;

            m1 = 1; m2 = 0; m3 = 0; 
            m4 = 0; m5 = 1; m6 = 0; 
            m7 = 0; m8 = 0; m9 = 1;
        }

        //Makes custom 3x3 Matrix with 9 custom floats
        public Matrix3(float i1, float j1, float k1, float i2, float j2, float k2, float i3, float j3, float k3)
        {
            this.i1 = i1; this.j1 = j1; this.k1 = k1;
            this.i2 = i2; this.j2 = j2; this.k2 = k2;
            this.i3 = i3; this.j3 = j3; this.k3 = k3;

            //For unit testing
            m1 = i1; m2 = j1; m3 = k1;
            m4 = i2; m5 = j2; m6 = k2;
            m7 = i3; m8 = j3; m9 = k3;
        }

        //Makes 3x3 Matrix with 3 custom vectors
        public Matrix3(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            i1 = v1.i; j1 = v1.j; k1 = v1.k;
            i2 = v2.i; j2 = v2.j; k2 = v2.k;
            i3 = v3.i; j3 = v3.j; k3 = v3.k;
        }
        #endregion

        #region ---'Matrix3' OPERATOR OVERLOADS (+,-,*,/)---
        //Matrix addition
        public static Matrix3 operator +(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3
                (lhs.i1 + rhs.i1, lhs.j1 + rhs.j1, lhs.k1 + rhs.k1,
                 lhs.i2 + rhs.i2, lhs.j2 + rhs.j2, lhs.k2 + rhs.k2,
                 lhs.i3 + rhs.i3, lhs.j3 + rhs.j3, lhs.k3 + rhs.k3);
        }

        //Matrix subtraction
        public static Matrix3 operator -(Matrix3 lhs, Matrix3 rhs)
        {
            return lhs + -1 * rhs;
        }

        //Matrix multiplication
        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            Matrix3 temp = new Matrix3(
                lhs.i1 * rhs.i1 + lhs.j1 * rhs.i2 + lhs.k1 * rhs.i3,
                lhs.i1 * rhs.j1 + lhs.j1 * rhs.j2 + lhs.k1 * rhs.j3,
                lhs.i1 * rhs.k1 + lhs.j1 * rhs.k2 + lhs.k1 * rhs.k3,

                lhs.i2 * rhs.i1 + lhs.j2 * rhs.i2 + lhs.k2 * rhs.i3,
                lhs.i2 * rhs.j1 + lhs.j2 * rhs.j2 + lhs.k2 * rhs.j3,
                lhs.i2 * rhs.k1 + lhs.j2 * rhs.k2 + lhs.k2 * rhs.k3,

                lhs.i3 * rhs.i1 + lhs.j3 * rhs.i2 + lhs.k3 * rhs.i3,
                lhs.i3 * rhs.j1 + lhs.j3 * rhs.j2 + lhs.k3 * rhs.j3,
                lhs.i3 * rhs.k1 + lhs.j3 * rhs.k2 + lhs.k3 * rhs.k3);

                temp.m1 = rhs.m1* lhs.m1 + rhs.m2 * lhs.m4 + rhs.m3 * lhs.m7;
                temp.m2 = rhs.m1* lhs.m2 + rhs.m2 * lhs.m5 + rhs.m3 * lhs.m8;
                temp.m3 = rhs.m1* lhs.m3 + rhs.m2 * lhs.m6 + rhs.m3 * lhs.m9;
                temp.m4 = rhs.m4* lhs.m1 + rhs.m5 * lhs.m4 + rhs.m6 * lhs.m7;
                temp.m5 = rhs.m4* lhs.m2 + rhs.m5 * lhs.m5 + rhs.m6 * lhs.m8;
                temp.m6 = rhs.m4* lhs.m3 + rhs.m5 * lhs.m6 + rhs.m6 * lhs.m9;
                temp.m7 = rhs.m7* lhs.m1 + rhs.m8 * lhs.m4 + rhs.m9 * lhs.m7;
                temp.m8 = rhs.m7* lhs.m2 + rhs.m8 * lhs.m5 + rhs.m9 * lhs.m8;
                temp.m9 = rhs.m7* lhs.m3 + rhs.m8 * lhs.m6 + rhs.m9 * lhs.m9;
            
            return temp;
        }

        //Matrix and vector multiplication
        public static Vector3 operator *(Matrix3 lhs, Vector3 rhs)
        {
            lhs.i3 = lhs.m7;
            lhs.j3 = lhs.m8;
            lhs.k3 = lhs.m9;

            Vector3 temp = new Vector3();
            temp.i = lhs.i1 * rhs.i + lhs.j1 * rhs.j + lhs.k1 * rhs.k;
            temp.j = lhs.i2 * rhs.i + lhs.j2 * rhs.j + lhs.k2 * rhs.k;
            temp.k = lhs.i3 * rhs.i + lhs.j3 * rhs.j + lhs.k3 * rhs.k;

            temp.x = rhs.i * lhs.i1 + rhs.j * lhs.i2 + rhs.k * lhs.i3;
            temp.y = rhs.i * lhs.j1 + rhs.j * lhs.j2 + rhs.k * lhs.j3;
            temp.z = rhs.i * lhs.k1 + rhs.j * lhs.k2 + rhs.k * lhs.k3;
            return temp;
        }

        //Matrix scalar multiplication
        public static Matrix3 operator *(float lhs, Matrix3 rhs)
        {
            return new Matrix3(lhs * rhs.i1, lhs * rhs.j1, lhs * rhs.k1,
                               lhs * rhs.i2, lhs * rhs.j2, lhs * rhs.k2,
                               lhs * rhs.i3, lhs * rhs.j3, lhs * rhs.k3);
        }

        //ToString overload to print matrices
        public override string ToString()
        {
            return i1 + ", " + j1 + ", " + k1 + ", " + i2 + ", " + j2 + ", " + k2 + ", " + i3 + ", " + j3 + ", " + k3;
        }
        #endregion

        #region ---'Matrix3' FUNCTIONS---
        //Transposes attached matrix
        public Matrix3 Transpose()
        {
            return new Matrix3(i1, i2, i3, j1, j2, j3, k1, k2, k3);
        }

        //Rotate matrix around X axis by theta radians (not useful for 3x3 homogenous matrix)
        public Matrix3 RotateX(float theta)
        {
            return new Matrix3(1, 0, 0, 0, (float)Math.Cos(theta), -(float)Math.Sin(theta), 0, (float)Math.Sin(theta), (float)Math.Cos(theta)) * this;
        }

        //Rotate matrix around Y axis by theta radians (not useful for 3x3 homogenous matrix)
        public Matrix3 RotateY(float theta)
        {
            return new Matrix3((float)Math.Cos(theta), 0, (float)Math.Sin(theta), 0, 1, 0, -(float)Math.Sin(theta), 0, (float)Math.Cos(theta)) * this;
        }

        //Rotate matrix around Z axis by theta radians
        public Matrix3 RotateZ(float theta)
        {
            return new Matrix3((float)Math.Cos(theta), -(float)Math.Sin(theta), 0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 1) * this;
        }

        //For unit testing
        public void SetRotateX(float radians)
        {
            Matrix3 temp = (Matrix3)MemberwiseClone();
            temp = temp.RotateX(radians).Transpose();

            i1 = temp.m1;
            j1 = temp.m2;
            k1 = temp.m3;
            i2 = temp.m4;
            j2 = temp.m5;
            k2 = temp.m6;
            i3 = temp.m7;
            j3 = temp.m8;
            k3 = temp.m9;

            m1 = temp.m1;
            m2 = temp.m2;
            m3 = temp.m3;
            m4 = temp.m4;
            m5 = temp.m5;
            m6 = temp.m6;
            m7 = temp.m7;
            m8 = temp.m8;
            m9 = temp.m9;
        }

        public void SetRotateY(float radians)
        {
            Matrix3 temp = (Matrix3)MemberwiseClone();
            temp = temp.RotateY(radians).Transpose();

            i1 = temp.m1;
            j1 = temp.m2;
            k1 = temp.m3;
            i2 = temp.m4;
            j2 = temp.m5;
            k2 = temp.m6;
            i3 = temp.m7;
            j3 = temp.m8;
            k3 = temp.m9;

            m1 = temp.m1;
            m2 = temp.m2;
            m3 = temp.m3;
            m4 = temp.m4;
            m5 = temp.m5;
            m6 = temp.m6;
            m7 = temp.m7;
            m8 = temp.m8;
            m9 = temp.m9;
        }

        public void SetRotateZ(float radians)
        {
            Matrix3 temp = (Matrix3)MemberwiseClone();
            temp = temp.RotateZ(radians).Transpose();

            i1 = temp.m1;
            j1 = temp.m2;
            k1 = temp.m3;
            i2 = temp.m4;
            j2 = temp.m5;
            k2 = temp.m6;
            i3 = temp.m7;
            j3 = temp.m8;
            k3 = temp.m9;

            m1 = temp.m1;
            m2 = temp.m2;
            m3 = temp.m3;
            m4 = temp.m4;
            m5 = temp.m5;
            m6 = temp.m6;
            m7 = temp.m7;
            m8 = temp.m8;
            m9 = temp.m9;
        }

        //Translate matrix by x units in x axis, y units in y axis, 0 units in z axis
        public Matrix3 Translate(float x, float y)
        {
            i3 += x;
            j3 += y;
            return this;
        }

        //Finds the determinant of a Matrix3
        public float Determinant()
        {
            return i1 * (j2 * k3 - j3 * k2) - j1 * (i2 * k3 - i3 * k2) + k1 * (i2 * j3 - i3 * j2);
        }

        //Finds the inverse of a Matrix3
        public Matrix3 Inverse()
        {
            return 1 / Determinant() *
                new Matrix3(
                +j2 * k3 - j3 * k2,
                -j1 * k3 + j3 * k1,
                +j1 * k2 - j2 * k1,

                -i2 * k3 + i3 * k2,
                +i1 * k3 - i3 * k1,
                -i1 * k2 + i2 * k1,

                +i2 * j3 - i3 * j2,
                -i1 * j3 + i3 * j1,
                +i1 * j2 - i2 * j1);
        }

        //Creates a new translation matrix of x units in x axis, y units in y axis, 0 units in z axis
        public static Matrix3 CreateTranslation(float x, float y)
        {
            return new Matrix3(1, 0, 0, 0, 1, 0, x, y, 1);
        }

        //Creates new x axis rotation matrix of theta radians
        public static Matrix3 CreateRotateX(float theta)
        {
            return new Matrix3(1, 0, 0, 0, (float)Math.Cos(theta), -(float)Math.Sin(theta), 0, (float)Math.Sin(theta), (float)Math.Cos(theta));
        }

        //Creates new y axis rotation of theta radians
        public static Matrix3 CreateRotateY(float theta)
        {
            return new Matrix3((float)Math.Cos(theta), 0, (float)Math.Sin(theta), 0, 1, 0, -(float)Math.Sin(theta), 0, (float)Math.Cos(theta));
        }

        //Creates new z axis rotation of theta radians
        public static Matrix3 CreateRotateZ(float theta)
        {
            return new Matrix3((float)Math.Cos(theta), -(float)Math.Sin(theta), 0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 1);
        }

        //Creates a new scale matrix of x units in x+,  y units in y+, 1 units in z+
        public static Matrix3 CreateScale(float x, float y)
        {
            return new Matrix3(x, 0, 0, 0, y, 0, 0, 0, 1);
        }

        //Resets a Matrix3 back to an identity Matrix
        public void Reset()
        {
            i1 = 1; j1 = 0; k1 = 0;
            i2 = 0; j2 = 1; k2 = 0;
            i3 = 0; j3 = 0; k3 = 1;
        }
        #endregion
    }

    //---Matrix4 (Represents 3D plane at w = 0, in 4D space)---
    public class Matrix4
    {
        //Matrix entries. i, j, k, w represent unit vector components pointing in x+, y+, z+, w+ directions
        public float
            i1, j1, k1, w1,
            i2, j2, k2, w2,
            i3, j3, k3, w3,
            i4, j4, k4, w4;

        //For unit tests
        public float
            m1, m2, m3, m4,
            m5, m6, m7, m8,
            m9, m10, m11, m12,
            m13, m14, m15, m16;

        #region ---'Matrix4' CONSTRUCTORS---
        //Default constructor creates 4x4 Identity Matrix
        public Matrix4()
        {
            i1 = 1; j1 = 0; k1 = 0; w1 = 0;
            i2 = 0; j2 = 1; k2 = 0; w2 = 0;
            i3 = 0; j3 = 0; k3 = 1; w3 = 0;
            i4 = 0; j4 = 0; k4 = 0; w4 = 1;

            m1 = i1; m2 = j1; m3 = k1; m4 = w1;
            m5 = i2; m6 = j2; m7 = k2; m8 = w2;
            m9 = i3; m10 = j3; m11 = k3; m12 = w3;
            m13 = i4; m14 = j4; m15 = k4; m16 = w4;
        }

        //Makes custom 4x4 Matrix with 16 custom floats
        public Matrix4(float i1, float j1, float k1, float w1, float i2, float j2, float k2, float w2, float i3, float j3, float k3, float w3, float i4, float j4, float k4, float w4)
        {
            this.i1 = i1; this.j1 = j1; this.k1 = k1; this.w1 = w1;
            this.i2 = i2; this.j2 = j2; this.k2 = k2; this.w2 = w2;
            this.i3 = i3; this.j3 = j3; this.k3 = k3; this.w3 = w3;
            this.i4 = i4; this.j4 = j4; this.k4 = k4; this.w4 = w4;

            m1 = i1; m2 = j1; m3 = k1; m4 = w1;
            m5 = i2; m6 = j2; m7 = k2; m8 = w2;
            m9 = i3; m10 = j3; m11 = k3; m12 = w3;
            m13 = i4; m14 = j4; m15 = k4; m16 = w4;
        }

        //Makes 4x4 Matrix with 4 custom vectors
        public Matrix4(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 v4)
        {
            i1 = v1.i; j1 = v1.j; k1 = v1.k; w1 = v1.w;
            i2 = v2.i; j2 = v2.j; k2 = v2.k; w2 = v2.w;
            i3 = v3.i; j3 = v3.j; k3 = v3.k; w3 = v3.w;
            i4 = v4.i; j4 = v4.j; k4 = v4.k; w4 = v4.w;
        }
        #endregion

        #region ---'Matrix4' OPERATOR OVERLOADS (+,-,*,/)---
        //Matrix addition
        public static Matrix4 operator +(Matrix4 lhs, Matrix4 rhs)
        {
            return new Matrix4
                (lhs.i1 + rhs.i1, lhs.j1 + rhs.j1, lhs.k1 + rhs.k1, lhs.w1 + rhs.w1,
                 lhs.i2 + rhs.i2, lhs.j2 + rhs.j2, lhs.k2 + rhs.k2, lhs.w2 + rhs.w2,
                 lhs.i3 + rhs.i3, lhs.j3 + rhs.j3, lhs.k3 + rhs.k3, lhs.w3 + rhs.w3,
                 lhs.i4 + rhs.i4, lhs.j4 + rhs.j4, lhs.k4 + rhs.k4, lhs.w4 + rhs.w4);
        }

        //Matrix subtraction
        public static Matrix4 operator -(Matrix4 lhs, Matrix4 rhs)
        {
            return lhs + -1 * rhs;
        }

        //Matrix multiplication
        public static Matrix4 operator *(Matrix4 lhs, Matrix4 rhs)
        {
            Matrix4 temp = new Matrix4(
                lhs.i1 * rhs.i1 + lhs.j1 * rhs.i2 + lhs.k1 * rhs.i3 + lhs.w1 * rhs.i4,
                lhs.i1 * rhs.j1 + lhs.j1 * rhs.j2 + lhs.k1 * rhs.j3 + lhs.w1 * rhs.j4,
                lhs.i1 * rhs.k1 + lhs.j1 * rhs.k2 + lhs.k1 * rhs.k3 + lhs.w1 * rhs.k4,
                lhs.i1 * rhs.w1 + lhs.j1 * rhs.w2 + lhs.k1 * rhs.w3 + lhs.w1 * rhs.w4,

                lhs.i2 * rhs.i1 + lhs.j2 * rhs.i2 + lhs.k2 * rhs.i3 + lhs.w2 * rhs.i4,
                lhs.i2 * rhs.j1 + lhs.j2 * rhs.j2 + lhs.k2 * rhs.j3 + lhs.w2 * rhs.j4,
                lhs.i2 * rhs.k1 + lhs.j2 * rhs.k2 + lhs.k2 * rhs.k3 + lhs.w2 * rhs.k4,
                lhs.i2 * rhs.w1 + lhs.j2 * rhs.w2 + lhs.k2 * rhs.w3 + lhs.w2 * rhs.w4,

                lhs.i3 * rhs.i1 + lhs.j3 * rhs.i2 + lhs.k3 * rhs.i3 + lhs.w3 * rhs.i4,
                lhs.i3 * rhs.j1 + lhs.j3 * rhs.j2 + lhs.k3 * rhs.j3 + lhs.w3 * rhs.j4,
                lhs.i3 * rhs.k1 + lhs.j3 * rhs.k2 + lhs.k3 * rhs.k3 + lhs.w3 * rhs.k4,
                lhs.i3 * rhs.w1 + lhs.j3 * rhs.w2 + lhs.k3 * rhs.w3 + lhs.w3 * rhs.w4,

                lhs.i4 * rhs.i1 + lhs.j4 * rhs.i2 + lhs.k4 * rhs.i3 + lhs.w4 * rhs.i4,
                lhs.i4 * rhs.j1 + lhs.j4 * rhs.j2 + lhs.k4 * rhs.j3 + lhs.w4 * rhs.j4,
                lhs.i4 * rhs.k1 + lhs.j4 * rhs.k2 + lhs.k4 * rhs.k3 + lhs.w4 * rhs.k4,
                lhs.i4 * rhs.w1 + lhs.j4 * rhs.w2 + lhs.k4 * rhs.w3 + lhs.w4 * rhs.w4);

            //i1 j1 k1 w1
            //i2 j2 k2 w2
            //i3 j3 k3 w3
            //i4 j4 k4 w4
            temp.m1 =  rhs.i1 * lhs.i1 + rhs.j1 * lhs.i2 + rhs.k1 * lhs.i3 + rhs.w1 * lhs.i4;
            temp.m2 =  rhs.i1 * lhs.j1 + rhs.j1 * lhs.j2 + rhs.k1 * lhs.j3 + rhs.w1 * lhs.j4;
            temp.m3 =  rhs.i1 * lhs.k1 + rhs.j1 * lhs.k2 + rhs.k1 * lhs.k3 + rhs.w1 * lhs.k4;
            temp.m4 =  rhs.i1 * lhs.w1 + rhs.j1 * lhs.w2 + rhs.k1 * lhs.w3 + rhs.w1 * lhs.w4;

            temp.m5 =  rhs.i2 * lhs.i1 + rhs.j2 * lhs.i2 + rhs.k2 * lhs.i3 + rhs.w2 * lhs.i4;
            temp.m6 =  rhs.i2 * lhs.j1 + rhs.j2 * lhs.j2 + rhs.k2 * lhs.j3 + rhs.w2 * lhs.j4;
            temp.m7 =  rhs.i2 * lhs.k1 + rhs.j2 * lhs.k2 + rhs.k2 * lhs.k3 + rhs.w2 * lhs.k4;
            temp.m8 =  rhs.i2 * lhs.w1 + rhs.j2 * lhs.w2 + rhs.k2 * lhs.w3 + rhs.w2 * lhs.w4;

            temp.m9 =  rhs.i3 * lhs.i1 + rhs.j3 * lhs.i2 + rhs.k3 * lhs.i3 + rhs.w3 * lhs.i4;
            temp.m10 = rhs.i3 * lhs.j1 + rhs.j3 * lhs.j2 + rhs.k3 * lhs.j3 + rhs.w3 * lhs.j4; 
            temp.m11 = rhs.i3 * lhs.k1 + rhs.j3 * lhs.k2 + rhs.k3 * lhs.k3 + rhs.w3 * lhs.k4; 
            temp.m12 = rhs.i3 * lhs.w1 + rhs.j3 * lhs.w2 + rhs.k3 * lhs.w3 + rhs.w3 * lhs.w4;

            temp.m13 = rhs.i4 * lhs.i1 + rhs.j4 * lhs.i2 + rhs.k4 * lhs.i3 + rhs.w4 * lhs.i4; 
            temp.m14 = rhs.i4 * lhs.j1 + rhs.j4 * lhs.j2 + rhs.k4 * lhs.j3 + rhs.w4 * lhs.j4; 
            temp.m15 = rhs.i4 * lhs.k1 + rhs.j4 * lhs.k2 + rhs.k4 * lhs.k3 + rhs.w4 * lhs.k4;
            temp.m16 = rhs.i4 * lhs.w1 + rhs.j4 * lhs.w2 + rhs.k4 * lhs.w3 + rhs.w4 * lhs.w4;

            return temp;
        }

        //Matrix and vector multiplication
        public static Vector4 operator *(Matrix4 lhs, Vector4 rhs)
        {
            lhs.i4 = lhs.m13;
            lhs.j4 = lhs.m14;
            lhs.k4 = lhs.m15;

            Vector4 temp = new Vector4();

            temp.i = lhs.i1* rhs.i + lhs.j1 * rhs.j + lhs.k1 * rhs.k + lhs.w1 * rhs.w;
            temp.j = lhs.i2* rhs.i + lhs.j2 * rhs.j + lhs.k2 * rhs.k + lhs.w2 * rhs.w;
            temp.k = lhs.i3* rhs.i + lhs.j3 * rhs.j + lhs.k3 * rhs.k + lhs.w3 * rhs.w;
            temp.w = lhs.i4* rhs.i + lhs.j4 * rhs.j + lhs.k4 * rhs.k + lhs.w4 * rhs.w;

            temp.x = rhs.i * lhs.i1 + rhs.j * lhs.i2 + rhs.k * lhs.i3 + rhs.w * lhs.i4;
            temp.y = rhs.i * lhs.j1 + rhs.j * lhs.j2 + rhs.k * lhs.j3 + rhs.w * lhs.j4;
            temp.z = rhs.i * lhs.k1 + rhs.j * lhs.k2 + rhs.k * lhs.k3 + rhs.w * lhs.k4;
            temp.w = rhs.i * lhs.w1 + rhs.j * lhs.w2 + rhs.k * lhs.w3 + rhs.w * lhs.w4;

            return temp;
        }

        //Matrix scalar multiplication
        public static Matrix4 operator *(float lhs, Matrix4 rhs)
        {
            return new Matrix4(lhs * rhs.i1, lhs * rhs.j1, lhs * rhs.k1, lhs * rhs.w1,
                               lhs * rhs.i2, lhs * rhs.j2, lhs * rhs.k2, lhs * rhs.w2,
                               lhs * rhs.i3, lhs * rhs.j3, lhs * rhs.k3, lhs * rhs.w3,
                               lhs * rhs.i4, lhs * rhs.j4, lhs * rhs.k4, lhs * rhs.w4);
        }

        //ToString overload to print matrices
        public override string ToString()
        {
            return i1 + ", " + j1 + ", " + k1 + ", " + w1 + ", " +
                   i2 + ", " + j2 + ", " + k2 + ", " + w2 + ", " +
                   i3 + ", " + j3 + ", " + k3 + ", " + w3 + ", " +
                   i4 + ", " + j4 + ", " + k4 + ", " + w4;
        }
        #endregion

        #region ---'Matrix4' FUNCTIONS---
        //Transposes attached matrix
        public Matrix4 Transpose()
        {
            return new Matrix4(i1, i2, i3, i4, j1, j2, j3, j4, k1, k2, k3, k4, w1, w2, w3, w4);
        }

        //Rotate matrix around X axis by theta radians
        public Matrix4 RotateX(float theta)
        {
            return new Matrix4(1, 0, 0, 0, 0, (float)Math.Cos(theta), -(float)Math.Sin(theta), 0, 0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 0, 1) * this;
        }

        //Rotate matrix around Y axis by theta radians
        public Matrix4 RotateY(float theta)
        {
            return new Matrix4((float)Math.Cos(theta), 0, (float)Math.Sin(theta), 0, 0, 1, 0, 0, -(float)Math.Sin(theta), 0, (float)Math.Cos(theta), 0, 0, 0, 0, 1) * this;
        }

        //Rotate matrix around Z axis by theta radians
        public Matrix4 RotateZ(float theta)
        {
            Matrix4 temp = (Matrix4)MemberwiseClone();
            temp = CreateRotateZ(theta) * temp;

            temp.m1 = temp.i1;
            temp.m2 = temp.j1;
            temp.m3 = temp.k1;
            temp.m4 = temp.w1;
            temp.m5 = temp.i2;
            temp.m6 = temp.j2;
            temp.m7 = temp.k2;
            temp.m8 = temp.w2;
            temp.m9 = temp.i3;
            temp.m10 = temp.j3;
            temp.m11 = temp.k3;
            temp.m12 = temp.w3;
            temp.m13 = temp.i4;
            temp.m14 = temp.j4;
            temp.m15 = temp.k4;
            temp.m16 = temp.w4;

            return temp;
        }

        //For unit testing
        public void SetRotateX(float radians)
        {
            Matrix4 temp = (Matrix4)MemberwiseClone();
            temp = temp.RotateX(radians).Transpose();

            i1 = temp.m1;
            j1 = temp.m2;
            k1 = temp.m3;
            w1 = temp.m4;
            i2 = temp.m5;
            j2 = temp.m6;
            k2 = temp.m7;
            w2 = temp.m8;
            i3 = temp.m9;
            j3 = temp.m10;
            k3 = temp.m11;
            w3 = temp.m12;
            i4 = temp.m13;
            j4 = temp.m14;
            k4 = temp.m15;
            w4 = temp.m16;


            m1 = temp.m1;
            m2 = temp.m2;
            m3 = temp.m3;
            m4 = temp.m4;
            m5 = temp.m5;
            m6 = temp.m6;
            m7 = temp.m7;
            m8 = temp.m8;
            m9 = temp.m9;
            m10 = temp.m10;
            m11 = temp.m11;
            m12 = temp.m12;
            m13 = temp.m13;
            m14 = temp.m14;
            m15 = temp.m15;
            m16 = temp.m16;
        }

        public void SetRotateY(float radians)
        {
            Matrix4 temp = (Matrix4)MemberwiseClone();
            temp = temp.RotateY(radians).Transpose();

            i1 = temp.i1;
            j1 = temp.j1;
            k1 = temp.k1;
            w1 = temp.w1;
            i2 = temp.i2;
            j2 = temp.j2;
            k2 = temp.k2;
            w2 = temp.w2;
            i3 = temp.i3;
            j3 = temp.j3;
            k3 = temp.k3;
            w3 = temp.w3;
            i4 = temp.i4;
            j4 = temp.j4;
            k4 = temp.k4;
            w4 = temp.w4;


            m1 = temp.i1;
            m2 = temp.j1;
            m3 = temp.k1;
            m4 = temp.w1;
            m5 = temp.i2;
            m6 = temp.j2;
            m7 = temp.k2;
            m8 = temp.w2;
            m9 = temp.i3;
            m10 = temp.j3;
            m11 = temp.k3;
            m12 = temp.w3;
            m13 = temp.i4;
            m14 = temp.j4;
            m15 = temp.k4;
            m16 = temp.w4;
        }

        public void SetRotateZ(float radians)
        {
            Matrix4 temp = (Matrix4)MemberwiseClone();
            temp = temp.RotateZ(radians).Transpose();

            i1 = temp.i1;
            j1 = temp.j1;
            k1 = temp.k1;
            w1 = temp.w1;
            i2 = temp.i2;
            j2 = temp.j2;
            k2 = temp.k2;
            w2 = temp.w2;
            i3 = temp.i3;
            j3 = temp.j3;
            k3 = temp.k3;
            w3 = temp.w3;
            i4 = temp.i4;
            j4 = temp.j4;
            k4 = temp.k4;
            w4 = temp.w4;


            m1 =  temp.i1;
            m2 =  temp.j1;
            m3 =  temp.k1;
            m4 =  temp.w1;
            m5 =  temp.i2;
            m6 =  temp.j2;
            m7 =  temp.k2;
            m8 =  temp.w2;
            m9 =  temp.i3;
            m10 = temp.j3;
            m11 = temp.k3;
            m12 = temp.w3;
            m13 = temp.i4;
            m14 = temp.j4;
            m15 = temp.k4;
            m16 = temp.w4;
        }
        
        //Translate matrix by x units in x axis, y units in y axis, z units in z axis, 0 units in w axis
        public Matrix4 Translate(float x, float y, float z)
        {
            i4 += x;
            j4 += y;
            k4 += z;
            return this;
        }

        //Finds the determinant of a Matrix4
        public float Determinant()
        {
            return +i1 * (j2 * (k3 * w4 - k4 * w3) - k2 * (j3 * w4 - j4 * w3) + w2 * (j3 * k4 - k4 * j3))
                   - j1 * (i2 * (k3 * w4 - k4 * w3) - k2 * (i3 * w4 - i4 * w3) + w2 * (i3 * k4 - i4 * k3))
                   + k1 * (i2 * (j3 * w4 - j4 * w3) - j2 * (i3 * w4 - i4 * w3) + w2 * (i3 * j4 - i4 * j3))
                   - w1 * (i2 * (j3 * k4 - j4 * k3) - j2 * (i3 * k4 - i4 * k3) + k2 * (i3 * j4 - i4 * j3));
        }

        //Finds the inverse of a Matrix4
        public Matrix4 Inverse()
        {
            float invDet = 1 / Determinant();
            return invDet * new Matrix4(
                +j2 * (k3 * w4 - k4 * w3) - j3 * (k2 * w4 - k4 * w2) + j4 * (k2 * w3 - k3 * w2),
                -j1 * (k3 * w4 - k4 * w3) + j3 * (k1 * w4 - k4 * w1) - j4 * (k1 * w3 - k3 * w1),
                +j1 * (k2 * w4 - k4 * w2) - j2 * (k1 * w4 - k4 * w1) + j4 * (k1 * w2 - k2 * w1),
                -j1 * (k2 * w3 - k3 * w2) + j2 * (k1 * w3 - k3 * w1) - j3 * (k1 * w2 - k2 * w1),

                -i2 * (k3 * w4 - k4 * w3) + i3 * (k2 * w4 - k4 * w2) - i4 * (k2 * w3 - k3 * w2),
                +i1 * (k3 * w4 - k4 * w3) - i3 * (k1 * w4 - k4 * w1) + i4 * (k1 * w3 - k3 * w1),
                -i1 * (k2 * w4 - k4 * w2) + i2 * (k1 * w4 - k4 * w1) - i4 * (k1 * w2 - k2 * w1),
                +i1 * (k2 * w3 - k3 * w2) - i2 * (k1 * w3 - k3 * w1) + i3 * (k1 * w2 - k2 * w1),

                +i2 * (j3 * w4 - j4 * w3) - i3 * (j2 * w4 - j4 * w2) + i4 * (j2 * w3 - j3 * w2),
                -i1 * (j3 * w4 - j4 * w3) + i3 * (j1 * w4 - j4 * w1) - i4 * (j1 * w3 - j3 * w1),
                +i1 * (j2 * w4 - j4 * w2) - i2 * (j1 * w4 - j4 * w1) + i4 * (j1 * w2 - j2 * w1),
                -i1 * (j2 * w3 - j3 * w2) + i2 * (j1 * w3 - j3 * w1) - i3 * (j1 * w2 - j2 * w1),

                -i2 * (j3 * k4 - j4 * k3) + i3 * (j2 * k4 - j4 * k2) - i4 * (j2 * k3 - j3 * k2),
                +i1 * (j3 * k4 - j4 * k3) - i3 * (j1 * k4 - j4 * k1) + i4 * (j1 * k3 - j3 * k1),
                -i1 * (j2 * k4 - j4 * k2) + i2 * (j1 * k4 - j4 * k1) - i4 * (j1 * k2 - j2 * k1),
                +i1 * (j2 * k3 - j3 * k2) - i2 * (j1 * k3 - j3 * k1) + i3 * (j1 * k2 - j2 * k1));
        }

        //Creates a new translation matrix of x units in x axis, y units in y axis, z units in z axis, 0 units in w axis
        public static Matrix4 CreateTranslation(float x, float y, float z)
        {
            return new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, x, y, z, 1);
        }

        //Creates new x axis rotation matrix of theta radians
        public static Matrix4 CreateRotateX(float theta)
        {
            return new Matrix4(1, 0, 0, 0, 0, (float)Math.Cos(theta), -(float)Math.Sin(theta), 0, 0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 0, 1);
        }

        //Creates new y axis rotation of theta radians
        public static Matrix4 CreateRotateY(float theta)
        {
            return new Matrix4((float)Math.Cos(theta), 0, (float)Math.Sin(theta), 0, 0, 1, 0, 0, -(float)Math.Sin(theta), 0, (float)Math.Cos(theta), 0, 0, 0, 0, 1);
        }

        //Creates new z axis rotation of theta radians
        public static Matrix4 CreateRotateZ(float theta)
        {
            Matrix4 temp = new Matrix4((float)Math.Cos(theta), -(float)Math.Sin(theta), 0, 0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            temp.m1 = temp.i1;
            temp.m2 = temp.j1;
            temp.m3 = temp.k1;
            temp.m4 = temp.w1;
            temp.m5 = temp.i2;
            temp.m6 = temp.j2;
            temp.m7 = temp.k2;
            temp.m8 = temp.w2;
            temp.m9 = temp.i3;
            temp.m10 = temp.j3;
            temp.m11 = temp.k3;
            temp.m12 = temp.w3;
            temp.m13 = temp.i4;
            temp.m14 = temp.j4;
            temp.m15 = temp.k4;
            temp.m16 = temp.w4;
            return temp;
        }

        //Creates a new scale matrix of x units in x+,  y units in y+, z units in z+, 1 units in w+
        public static Matrix4 CreateScale(float x, float y, float z)
        {
            return new Matrix4(x, 0, 0, 0, 0, y, 0, 0, 0, 0, z, 0, 0, 0, 0, 1);
        }

        //Resets a Matrix4 back to an identity Matrix
        public void Reset()
        {
            i1 = 1; j1 = 0; k1 = 0; w1 = 0;
            i2 = 0; j2 = 1; k2 = 0; w2 = 0;
            i3 = 0; j3 = 0; k3 = 1; w3 = 0;
            i4 = 0; j4 = 0; k4 = 0; w4 = 1;
        }
        #endregion
    }

    //---Colour (32 bit colour methods and functions)---
    public class Colour
    {
        public int value;

        //For unit testing
        public uint colour;

        public Colour()
        {
            value = 0;
            colour = 0;
        }

        public Colour(int red, int green, int blue, int alpha)
        {
            value = (red << 24) | (green << 16) | (blue << 8) | alpha;
            colour = (uint)value;
        }

        public byte GetRed()
        {
            return (byte)(value >> 24);
        }
        public void SetRed(int red)
        {
            value = (int)(value & 0x00FFFFFF) | (red << 24);
            colour = (uint)value;
        }
        public byte GetGreen()
        {
            return (byte)(value << 8 >> 24);
        }
        public void SetGreen(int green)
        {
            value = (int)(value & 0xFF00FFFF) | (green << 16);
            colour = (uint)value;
        }
        public byte GetBlue()
        {
            return (byte)(value << 16 >> 24);
        }
        public void SetBlue(int blue)
        {
            value = (int)(value & 0xFFFF00FF) | (blue << 8);
            colour = (uint)value;
        }
        public byte GetAlpha()
        {
            return (byte)(value << 24 >> 24);
        }
        public void SetAlpha(int alpha)
        {
            value = (int)(value & 0xFFFFFF00) | alpha;
            colour = (uint)value;
        }
    }

    //Radian and degree conversion extension functions added to float type
    public static class NumberExtensions
    {
        public static float ConvertToRadians(this float degrees)
        {
            return degrees / 180 * (float)Math.PI;
        }

        public static float ConvertToRadians(this double degrees)
        {
            return (float)degrees / 180 * (float)Math.PI;
        }

        public static float ConvertToDegrees(this float radians)
        {
            return radians * 180 / (float)Math.PI;
        }

        public static float ConvertToDegrees(this double radians)
        {
            return (float)radians * 180 / (float)Math.PI;
        }
    }
}


