using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Specialized;
using Math_Library;

namespace Unit_Testing
{
    [TestClass]
    public class Unit_Testing
    {
        const float DEFAULT_TOLERANCE = 0.0001f;

        bool compare(float a, float b, float tolerance = DEFAULT_TOLERANCE)
        {
            if (Math.Abs(a - b) > tolerance)
                return false;
            return true;
        }

        bool compare(Vector3 a, Vector3 b, float tolerance = DEFAULT_TOLERANCE)
        {
            if (Math.Abs(a.i - b.i) > tolerance ||
                Math.Abs(a.j - b.j) > tolerance ||
                Math.Abs(a.k - b.k) > tolerance)
                return false;
            return true;
        }

        bool compare(Vector4 a, Vector4 b, float tolerance = DEFAULT_TOLERANCE)
        {
            if (Math.Abs(a.i - b.i) > tolerance ||
                Math.Abs(a.j - b.j) > tolerance ||
                Math.Abs(a.k - b.k) > tolerance ||
                Math.Abs(a.w - b.w) > tolerance)
                return false;
            return true;
        }

        bool compare(Matrix3 a, Matrix3 b, float tolerance = DEFAULT_TOLERANCE)
        {
            if (Math.Abs(a.i1 - b.i1) > tolerance || Math.Abs(a.j1 - b.j1) > tolerance || Math.Abs(a.k1 - b.k1) > tolerance ||
                Math.Abs(a.i2 - b.i2) > tolerance || Math.Abs(a.j2 - b.j2) > tolerance || Math.Abs(a.k2 - b.k2) > tolerance ||
                Math.Abs(a.i3 - b.i3) > tolerance || Math.Abs(a.j3 - b.j3) > tolerance || Math.Abs(a.k3 - b.k3) > tolerance)
                return false;
            return true;
        }

        bool compare(Matrix4 a, Matrix4 b, float tolerance = DEFAULT_TOLERANCE)
        {
            if (Math.Abs(a.i1 - b.i1) > tolerance || Math.Abs(a.j1 - b.j1) > tolerance || Math.Abs(a.k1 - b.k1) > tolerance || Math.Abs(a.w1 - b.w1) > tolerance ||
                Math.Abs(a.i2 - b.i2) > tolerance || Math.Abs(a.j2 - b.j2) > tolerance || Math.Abs(a.k2 - b.k2) > tolerance || Math.Abs(a.w2 - b.w2) > tolerance ||
                Math.Abs(a.i3 - b.i3) > tolerance || Math.Abs(a.j3 - b.j3) > tolerance || Math.Abs(a.k3 - b.k3) > tolerance || Math.Abs(a.w3 - b.w3) > tolerance ||
                Math.Abs(a.i4 - b.i4) > tolerance || Math.Abs(a.j4 - b.j4) > tolerance || Math.Abs(a.k4 - b.k4) > tolerance || Math.Abs(a.w4 - b.w4) > tolerance)
                return false;
            return true;
        }

        [TestMethod]
        public void Vector3Addition()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3b = new Vector3(5, 3.99f, -12);
            Vector3 v3c = v3a + v3b;

            Assert.IsTrue(compare(new Vector3(18.5f, -44.24f, 850), v3c));
        }

        [TestMethod]
        public void Vector4Addition()
        {
            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4b = new Vector4(5, 3.99f, -12, 1);
            Vector4 v4c = v4a + v4b;

            Assert.IsTrue(compare(new Vector4(18.5f, -44.24f, 850, 1), v4c));
        }

        [TestMethod]
        public void Vector3Subtraction()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3b = new Vector3(5, 3.99f, -12);
            Vector3 v3c = v3a - v3b;

            Assert.IsTrue(compare(new Vector3(8.5f, -52.22f, 874), v3c));
        }

        [TestMethod]
        public void Vector4Subtraction()
        {
            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4b = new Vector4(5, 3.99f, -12, 1);
            Vector4 v4c = v4a - v4b;

            Assert.IsTrue(compare(new Vector4(8.5f, -52.22f, 874, -1), v4c));
        }

        [TestMethod]
        public void Vector3PostScale()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3c = 0.256f * v3a;

            Assert.IsTrue(compare(new Vector3(3.45600008965f, -12.3468809128f, 220.672012329f), v3c));
        }

        [TestMethod]
        public void Vector4PostScale()
        {
            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4c = 4.89f * v4a;

            Assert.IsTrue(compare(new Vector4(66.0149993896f, -235.844696045f, 4215.1796875f, 0), v4c));
        }

        [TestMethod]
        public void Vector3PreScale()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3c = 0.256f * v3a;

            Assert.IsTrue(compare(new Vector3(3.45600008965f, -12.3468809128f, 220.672012329f), v3c));
        }

        [TestMethod]
        public void Vector4PreScale()
        {
            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4c = 4.89f * v4a;

            Assert.IsTrue(compare(new Vector4(66.0149993896f, -235.844696045f, 4215.1796875f, 0), v4c));
        }

        [TestMethod]
        public void Vector3Dot()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3b = new Vector3(5, 3.99f, -12);
            float dot3 = v3a * v3b;

            Assert.AreEqual(dot3, -10468.9375f, DEFAULT_TOLERANCE);
        }

        [TestMethod]
        public void Vector4Dot()
        {
            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4b = new Vector4(5, 3.99f, -12, 1);
            float dot4 = v4a * v4b;

            Assert.AreEqual(dot4, -10468.9375f, DEFAULT_TOLERANCE);
        }

        [TestMethod]
        public void Vector3Cross()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3b = new Vector3(5, 3.99f, -12);
            Vector3 v3c = v3a % v3b;

            Assert.IsTrue(compare(v3c, new Vector3(-2860.62011719f, 4472.00000000f, 295.01498413f)));
        }

        [TestMethod]
        public void Vector4Cross()
        {
            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4b = new Vector4(5, 3.99f, -12, 1);
            Vector4 v4c = v4a % v4b;

            Assert.IsTrue(compare(v4c, new Vector4(-2860.62011719f, 4472.00000000f, 295.01498413f, 0)));
        }

        [TestMethod]
        public void Vector3Magnitude()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            float mag3 = v3a.Magnitude();

            Assert.AreEqual(mag3, 863.453735352f, DEFAULT_TOLERANCE);
        }

        [TestMethod]
        public void Vector4Magnitude()
        {
            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            float mag4 = v4a.Magnitude();

            Assert.AreEqual(mag4, 863.453735352f, DEFAULT_TOLERANCE);
        }

        [TestMethod]
        public void Vector3Normalise()
        {
            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            v3a = v3a.Normalise();

            Assert.IsTrue(compare(v3a, new Vector3(0.0156349f, -0.0558571f, 0.998316f)));
        }

        [TestMethod]
        public void Vector4Normalise()
        {
            Vector4 v4a = new Vector4(243, -48.23f, 862, 0);
            v4a = v4a.Normalise();

            Assert.IsTrue(compare(v4a, new Vector4(0.270935f, -0.0537745f, 0.961094f, 0)));
        }

        [TestMethod]
        public void Matrix3SetRotateX()
        {
            Matrix3 m3a = new Matrix3();
            m3a = m3a.RotateX(3.98f);

            Assert.IsTrue(compare(m3a,
                new Matrix3(1, 0, 0, 0, -0.668648f, -0.743579f, 0, 0.743579f, -0.668648f).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Matrix4SetRotateX()
        {
            Matrix4 m4a = new Matrix4();
            m4a = m4a.RotateX(4.5f);

            Assert.IsTrue(compare(m4a,
                new Matrix4(1, 0, 0, 0, 0, -0.210796f, -0.97753f, 0, 0, 0.97753f, -0.210796f, 0, 0, 0, 0, 1).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Matrix3SetRotateY()
        {
            Matrix3 m3b = new Matrix3();
            m3b = m3b.RotateY(1.76f);

            Assert.IsTrue(compare(m3b,
                new Matrix3(-0.188077f, 0, -0.982154f, 0, 1, 0, 0.982154f, 0, -0.188077f).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Matrix4SetRotateY()
        {
            Matrix4 m4b = new Matrix4();
            m4b = m4b.RotateY(-2.6f);

            Assert.IsTrue(compare(m4b,
                new Matrix4(-0.856889f, 0, 0.515501f, 0, 0, 1, 0, 0, -0.515501f, 0, -0.856889f, 0, 0, 0, 0, 1).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Matrix3SetRotateZ()
        {
            Matrix3 m3c = new Matrix3();
            m3c = m3c.RotateZ(9.62f);

            Assert.IsTrue(compare(m3c,
                new Matrix3(-0.981005f, -0.193984f, 0, 0.193984f, -0.981005f, 0, 0, 0, 1).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Matrix4SetRotateZ()
        {
            Matrix4 m4c = new Matrix4();
            m4c = m4c.RotateZ(0.72f);

            Assert.IsTrue(compare(m4c,
                new Matrix4(0.751806f, 0.659385f, 0, 0, -0.659385f, 0.751806f, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Vector3MatrixTransform()
        {
            Matrix3 m3b = new Matrix3();
            m3b = m3b.RotateY(1.76f);

            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3b = m3b * v3a;

            Assert.IsTrue(compare(v3b,
                new Vector3(844.077941895f, -48.2299995422f, -175.38130188f)));
        }

        [TestMethod]
        public void Vector3MatrixTransform2()
        {
            Matrix3 m3c = new Matrix3();
            m3c = m3c.RotateZ(9.62f);

            Vector3 v3a = new Vector3(13.5f, -48.23f, 862);
            Vector3 v3c = m3c * v3a;

            Assert.IsTrue(compare(v3c,
                new Vector3(-22.5994224548f, 44.6950683594f, 862)));
        }

        [TestMethod]
        public void Vector4MatrixTransform()
        {
            Matrix4 m4b = new Matrix4();
            m4b = m4b.RotateY(-2.6f);

            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4b = m4b * v4a;

            Assert.IsTrue(compare(v4b,
                new Vector4(-455.930236816f, -48.2299995422f, -731.678771973f, 0)));
        }

        [TestMethod]
        public void Vector4MatrixTransform2()
        {
            Matrix4 m4c = new Matrix4();
            m4c = m4c.RotateZ(0.72f);

            Vector4 v4a = new Vector4(13.5f, -48.23f, 862, 0);
            Vector4 v4b = m4c * v4a;

            Assert.IsTrue(compare(v4b,
                new Vector4(41.951499939f, -27.3578968048f, 862, 0)));
        }

        [TestMethod]
        public void Matrix3Multiply()
        {
            Matrix3 m3a = new Matrix3();
            m3a = m3a.RotateX(3.98f);

            Matrix3 m3c = new Matrix3();
            m3c = m3c.RotateZ(9.62f);

            Matrix3 m3d = m3a * m3c;

            Assert.IsTrue(compare(m3d,
                new Matrix3(-0.981004655361f, 0.129707172513f, 0.14424264431f, 0.193984255195f, 0.655946731567f, 0.729454636574f, 0, 0.743579149246f, -0.668647944927f).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Matrix4Multiply()
        {
            Matrix4 m4b = new Matrix4();
            m4b = m4b.RotateY(-2.6f);

            Matrix4 m4c = new Matrix4();
            m4c = m4c.RotateZ(0.72f);

            Matrix4 m4d = m4c * m4b;

            Assert.IsTrue(compare(m4d,
                new Matrix4(-0.644213855267f, -0.565019249916f, 0.515501439571f, 0, -0.659384667873f, 0.751805722713f, 0, 0, -0.387556940317f, -0.339913755655f, -0.856888711452f, 0, 0, 0, 0, 1).Transpose()));
            //Result has been transposed to fit with row major math library.
        }

        [TestMethod]
        public void Vector3MatrixTranslation()
        {
            // homogeneous point translation
            Matrix3 m3b = new Matrix3(1, 0, 0,
                                      0, 1, 0,
                                      55, 44, 1);

            Vector3 v3a = new Vector3(13.5f, -48.23f, 1);

            Vector3 v3b = m3b.Transpose() * v3a;
            //Result has been transposed to fit with row major math library.

            Assert.IsTrue(compare(v3b, new Vector3(68.5f, -4.23f, 1)));
        }

        [TestMethod]
        public void Vector3MatrixTranslation2()
        {
            // homogeneous point translation
            Matrix3 m3c = new Matrix3();
            m3c = m3c.RotateZ(2.2f);
            
            m3c.k1 = 55; m3c.k2 = 44; m3c.k3 = 1;
            //Result has been transposed to fit with row major math library.
            Vector3 v3a = new Vector3(13.5f, -48.23f, 1);

            Vector3 v3c = m3c * v3a;

            Assert.IsTrue(compare(v3c, new Vector3(86.0490112305f, 83.2981109619f, 1)));
        }

        [TestMethod]
        public void Vector4MatrixTranslation()
        {
            // homogeneous point translation
            Matrix4 m4b = new Matrix4(1, 0, 0, 0,
                                      0, 1, 0, 0,
                                      0, 0, 1, 0,
                                      55, 44, 99, 1);

            Vector4 v4a = new Vector4(13.5f, -48.23f, -54, 1);

            Vector4 v4c = m4b.Transpose() * v4a;
            Assert.IsTrue(compare(v4c, new Vector4(68.5f, -4.23f, 45, 1)));
        }

        [TestMethod]
        public void Vector4MatrixTranslation2()
        {
            // homogeneous point translation
            Matrix4 m4c = new Matrix4();
            m4c = m4c.RotateZ(2.2f);
            m4c.w1 = 55; m4c.w2 = 44; m4c.w3 = 99; m4c.w4 = 1;
            //Result has been transposed to fit with row major math library.
            Vector4 v4a = new Vector4(13.5f, -48.23f, -54, 1);

            Vector4 v4c = m4c * v4a;
            Assert.IsTrue(compare(v4c, new Vector4(86.0490112305f, 83.2981109619f, 45, 1)));
        }

        [TestMethod]
        public void Vector3MatrixTranslation3()
        {
            // homogeneous point translation
            Matrix3 m3b = new Matrix3(1, 0, 0,
                                      0, 1, 0,
                                      55, 44, 1);

            Vector3 v3a = new Vector3(13.5f, -48.23f, 0);

            Vector3 v3b = m3b.Transpose() * v3a;

            Assert.IsTrue(compare(v3b, new Vector3(13.5f, -48.23f, 0)));
        }

        [TestMethod]
        public void Vector3MatrixTranslation4()
        {
            // homogeneous point translation
            Matrix3 m3c = new Matrix3();
            m3c = m3c.RotateZ(2.2f);
            m3c.k1 = 55; m3c.k2 = 44; m3c.k3 = 1;

            Vector3 v3a = new Vector3(13.5f, -48.23f, 0);

            Vector3 v3c = m3c * v3a;

            Assert.IsTrue(compare(v3c, new Vector3(31.0490131378f, 39.2981109619f, 0)));
        }

        [TestMethod]
        public void Vector4MatrixTranslation3()
        {
            // homogeneous point translation
            Matrix4 m4b = new Matrix4(1, 0, 0, 0,
                                      0, 1, 0, 0,
                                      0, 0, 1, 0,
                                      55, 44, 99, 1);

            Vector4 v4a = new Vector4(13.5f, -48.23f, -54, 0);

            Vector4 v4c = m4b.Transpose() * v4a;
            Assert.IsTrue(compare(v4c, new Vector4(13.5f, -48.23f, -54, 0)));
        }

        [TestMethod]
        public void Vector4MatrixTranslation4()
        {
            // homogeneous point translation
            Matrix4 m4c = new Matrix4();
            m4c = m4c.RotateZ(2.2f);
            m4c.w1 = 55; m4c.w2 = 44; m4c.w3 = 99; m4c.w4 = 1;

            Vector4 v4a = new Vector4(13.5f, -48.23f, -54, 0);

            Vector4 v4c = m4c * v4a;
            Assert.IsTrue(compare(v4c, new Vector4(31.0490131378f, 39.2981109619f, -54, 0)));
        }

        [TestMethod]
        public void ColourConstructor()
        {
            // homogeneous point translation
            Colour c = new Colour(0x12, 0x34, 0x56, 0x78);

            Assert.AreEqual<UInt32>((uint)c.value, 0x12345678);
        }

        [TestMethod]
        public void ColourGetRed()
        {
            // homogeneous point translation
            Colour c = new Colour(0x12, 0x34, 0x56, 0x78);

            Assert.AreEqual<byte>(c.GetRed(), 0x12);
        }

        [TestMethod]
        public void ColourGetGreen()
        {
            // homogeneous point translation
            Colour c = new Colour(0x12, 0x34, 0x56, 0x78);

            Assert.AreEqual<byte>(c.GetGreen(), 0x34);
        }


        [TestMethod]
        public void ColourGetBlue()
        {
            // homogeneous point translation
            Colour c = new Colour(0x12, 0x34, 0x56, 0x78);

            Assert.AreEqual<byte>(c.GetBlue(), 0x56);
        }

        [TestMethod]
        public void ColourGetAlpha()
        {
            // homogeneous point translation
            Colour c = new Colour(0x12, 0x34, 0x56, 0x78);

            Assert.AreEqual<byte>(c.GetAlpha(), 0x78);
        }

        [TestMethod]
        public void ColourSetRed()
        {
            // homogeneous point translation
            Colour c = new Colour();
            c.SetRed(0x12);

            Assert.AreEqual<UInt32>((uint)c.value, 0x12000000);
        }

        [TestMethod]
        public void ColourSetGreen()
        {
            // homogeneous point translation
            Colour c = new Colour();
            c.SetGreen(0x34);

            Assert.AreEqual<UInt32>((uint)c.value, 0x00340000);
        }

        [TestMethod]
        public void ColourSetBlue()
        {
            // homogeneous point translation
            Colour c = new Colour();
            c.SetBlue(0x56);

            Assert.AreEqual<UInt32>((uint)c.value, 0x00005600);
        }

        [TestMethod]
        public void ColourSetAlpha()
        {
            // homogeneous point translation
            Colour c = new Colour();
            c.SetAlpha(0x78);

            Assert.AreEqual<UInt32>((uint)c.value, 0x00000078);
        }
    }
}
