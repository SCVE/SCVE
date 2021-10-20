using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace SCVE.Core.Primitives
{
    /// <summary>
    /// Straightforward implementation of a matrix, based on OpenTK.Mathematics.Matrix4 
    /// </summary>
    public class ScveMatrix4X4
    {
        private static readonly float[] IdentityFloats =
        {
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        };

        private float[] _values = new float[16];

        /// <summary>
        /// Initializes the matrix with zeros
        /// </summary>
        public ScveMatrix4X4()
        {
        }

        /// <summary>
        /// Initializes the matrix with an array of 16 elements
        /// </summary>
        public ScveMatrix4X4(float[] values)
        {
            if (values.Length != 16)
            {
                throw new ScveException($"Values must contain exactly 16 elements, but was {values.Length}");
            }

            Array.Copy(values, _values, 16);
        }

        /// <summary>
        /// Returns a new Identity matrix
        /// </summary>
        public static ScveMatrix4X4 Identity => new ScveMatrix4X4(
            IdentityFloats
        );

        /// <summary>
        /// Returns internal array of values, this should be used with caution
        /// </summary>
        public float[] GetRawValues()
        {
            return _values;
        }

        /// <summary>
        /// Implicit conversion to float array
        /// </summary>
        public static implicit operator float[](ScveMatrix4X4 matrix)
        {
            return matrix._values;
        }

        public float this[int i, int j]
        {
            get => _values[i * 4 + j];
            set => _values[i * 4 + j] = value;
        }

        public ScveMatrix4X4 Set(int i, int j, float value)
        {
            _values[i * 4 + j] = value;
            return this;
        }

        /// <summary>
        /// Calculates a determinant of current matrix
        /// <remarks>
        /// A direct copy from OpenTK
        /// </remarks>
        /// </summary>
        public float GetDeterminant()
        {
            float x1 = _values[0];
            float y1 = _values[1];
            float z1 = _values[2];
            float w1 = _values[3];
            float x2 = _values[4];
            float y2 = _values[5];
            float z2 = _values[6];
            float w2 = _values[7];
            float x3 = _values[8];
            float y3 = _values[9];
            float z3 = _values[10];
            float w3 = _values[11];
            float x4 = _values[12];
            float y4 = _values[13];
            float z4 = _values[14];
            float w4 = _values[15];
            return x1 * y2 * z3 * w4 -
                   x1 * y2 * w3 * z4 +
                   x1 * z2 * w3 * y4 -
                   x1 * z2 * y3 * w4 +
                   x1 * w2 * y3 * z4 -
                   x1 * w2 * z3 * y4 -
                   y1 * z2 * w3 * x4 +
                   y1 * z2 * x3 * w4 -
                   y1 * w2 * x3 * z4 +
                   y1 * w2 * z3 * x4 -
                   y1 * x2 * z3 * w4 +
                   y1 * x2 * w3 * z4 +
                   z1 * w2 * x3 * y4 -
                   z1 * w2 * y3 * x4 +
                   z1 * x2 * y3 * w4 -
                   z1 * x2 * w3 * y4 +
                   z1 * y2 * w3 * x4 -
                   z1 * y2 * x3 * w4 -
                   w1 * x2 * y3 * z4 +
                   w1 * x2 * z3 * y4 -
                   w1 * y2 * z3 * x4 +
                   w1 * y2 * x3 * z4 -
                   w1 * z2 * x3 * y4 +
                   w1 * z2 * y3 * x4;
        }

        public ScveMatrix4X4 MakeIdentity()
        {
            for (var i = 0; i < _values.Length; i++)
            {
                _values[i] = IdentityFloats[i];
            }

            return this;
        }

        public ScveMatrix4X4 MakeNormalized()
        {
            var determinant = GetDeterminant();

            for (var i = 0; i < _values.Length; i++)
            {
                _values[i] /= determinant;
            }

            return this;
        }

        public static ScveMatrix4X4 CreateOrthographicOffCenter(
            float left,
            float right,
            float bottom,
            float top,
            float depthNear,
            float depthFar)
        {
            ScveMatrix4X4 result = Identity;
            float invWidth = 1.0f / (right - left);
            float invHeight = 1.0f / (top - bottom);
            float invDepth = 1.0f / (depthFar - depthNear);

            result._values[0] = 2f * invWidth;
            result._values[5] = 2f * invHeight;
            result._values[10] = -2f * invDepth;
            result._values[3] = -(right + left) * invWidth;
            result._values[7] = -(top + bottom) * invHeight;
            result._values[11] = -(depthFar + depthNear) * invDepth;
            
            return result;
        }

        /// <summary>
        /// Multiplies current matrix with another
        /// </summary>
        public ScveMatrix4X4 Multiply(ScveMatrix4X4 right)
        {
            float leftM11 = _values[0];
            float leftM12 = _values[1];
            float leftM13 = _values[2];
            float leftM14 = _values[3];
            float leftM21 = _values[4];
            float leftM22 = _values[5];
            float leftM23 = _values[6];
            float leftM24 = _values[7];
            float leftM31 = _values[8];
            float leftM32 = _values[9];
            float leftM33 = _values[10];
            float leftM34 = _values[11];
            float leftM41 = _values[12];
            float leftM42 = _values[13];
            float leftM43 = _values[14];
            float leftM44 = _values[15];
            float rightM11 = right._values[0];
            float rightM12 = right._values[1];
            float rightM13 = right._values[2];
            float rightM14 = right._values[3];
            float rightM21 = right._values[4];
            float rightM22 = right._values[5];
            float rightM23 = right._values[6];
            float rightM24 = right._values[7];
            float rightM31 = right._values[8];
            float rightM32 = right._values[9];
            float rightM33 = right._values[10];
            float rightM34 = right._values[11];
            float rightM41 = right._values[12];
            float rightM42 = right._values[13];
            float rightM43 = right._values[14];
            float rightM44 = right._values[15];

            _values[0] = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31) + (leftM14 * rightM41);
            _values[1] = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32) + (leftM14 * rightM42);
            _values[2] = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33) + (leftM14 * rightM43);
            _values[3] = (leftM11 * rightM14) + (leftM12 * rightM24) + (leftM13 * rightM34) + (leftM14 * rightM44);
            _values[4] = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31) + (leftM24 * rightM41);
            _values[5] = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32) + (leftM24 * rightM42);
            _values[6] = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33) + (leftM24 * rightM43);
            _values[7] = (leftM21 * rightM14) + (leftM22 * rightM24) + (leftM23 * rightM34) + (leftM24 * rightM44);
            _values[8] = (leftM31 * rightM11) + (leftM32 * rightM21) + (leftM33 * rightM31) + (leftM34 * rightM41);
            _values[9] = (leftM31 * rightM12) + (leftM32 * rightM22) + (leftM33 * rightM32) + (leftM34 * rightM42);
            _values[10] = (leftM31 * rightM13) + (leftM32 * rightM23) + (leftM33 * rightM33) + (leftM34 * rightM43);
            _values[11] = (leftM31 * rightM14) + (leftM32 * rightM24) + (leftM33 * rightM34) + (leftM34 * rightM44);
            _values[12] = (leftM41 * rightM11) + (leftM42 * rightM21) + (leftM43 * rightM31) + (leftM44 * rightM41);
            _values[13] = (leftM41 * rightM12) + (leftM42 * rightM22) + (leftM43 * rightM32) + (leftM44 * rightM42);
            _values[14] = (leftM41 * rightM13) + (leftM42 * rightM23) + (leftM43 * rightM33) + (leftM44 * rightM43);
            _values[15] = (leftM41 * rightM14) + (leftM42 * rightM24) + (leftM43 * rightM34) + (leftM44 * rightM44);

            return this;
        }

        /// <summary>
        /// Sums current with another
        /// </summary>
        public ScveMatrix4X4 Add(ScveMatrix4X4 right)
        {
            for (var i = 0; i < _values.Length; i++)
            {
                _values[i] += right._values[i];
            }

            return this;
        }

        /// <summary>
        /// Subtracts another matrix from current
        /// </summary>
        public ScveMatrix4X4 Sub(ScveMatrix4X4 right)
        {
            for (var i = 0; i < _values.Length; i++)
            {
                _values[i] -= right._values[i];
            }

            return this;
        }

        public static ScveMatrix4X4 CreateTranslation(float x = 0, float y = 0, float z = 0)
        {
            ScveMatrix4X4 result = Identity;

            result._values[12] = x;
            result._values[13] = y;
            result._values[14] = z;

            return result;
        }

        public static ScveMatrix4X4 CreateScale(float x = 1, float y = 1, float z = 1)
        {
            ScveMatrix4X4 result = Identity;

            result._values[0] = x;
            result._values[5] = y;
            result._values[10] = z;

            return result;
        }

        /// <summary>Calculate the inverse of the given matrix.</summary>
        /// <param name="mat">The matrix to invert.</param>
        /// <param name="result">The inverse of the given matrix if it has one, or the input if it is singular.</param>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
        /// <footer><a href="https://www.google.com/search?q=OpenTK.Mathematics.Matrix4.Invert">`Matrix4.Invert` on google.com</a></footer>
        public ScveMatrix4X4 Invert()
        {
            if (Sse3.IsSupported)
            {
                InvertSse3(this);
            }
            else
            {
                throw new ScveException("SSE3 is not supported");
                // ScveMatrix4X4.InvertFallback(in mat, out result);
            }

            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void InvertSse3(ScveMatrix4X4 mat)
        {
            Vector128<float> vector128_1;
            Vector128<float> vector128_2;
            Vector128<float> vector128_3;
            Vector128<float> vector128_4;
            fixed (float* address = mat._values)
            {
                vector128_1 = Sse.LoadVector128(address);
                vector128_2 = Sse.LoadVector128(address + 4);
                vector128_3 = Sse.LoadVector128(address + 8);
                vector128_4 = Sse.LoadVector128(address + 12);
            }

            Vector128<float> high1 = Sse.MoveLowToHigh(vector128_1, vector128_2);
            Vector128<float> low1 = Sse.MoveHighToLow(vector128_2, vector128_1);
            Vector128<float> high2 = Sse.MoveLowToHigh(vector128_3, vector128_4);
            Vector128<float> low2 = Sse.MoveHighToLow(vector128_4, vector128_3);
            Vector128<float> vector1 = Sse.Subtract(Sse.Multiply(Sse.Shuffle(vector128_1, vector128_3, (byte)136), Sse.Shuffle(vector128_2, vector128_4, (byte)221)), Sse.Multiply(Sse.Shuffle(vector128_1, vector128_3, (byte)221), Sse.Shuffle(vector128_2, vector128_4, (byte)136)));
            Vector128<float> left1 = Sse2.Shuffle(vector1.AsInt32<float>(), (byte)0).AsSingle<int>();
            Vector128<float> left2 = Sse2.Shuffle(vector1.AsInt32<float>(), (byte)85).AsSingle<int>();
            Vector128<float> vector128_5 = Sse2.Shuffle(vector1.AsInt32<float>(), (byte)170).AsSingle<int>();
            Vector128<float> vector128_6 = Sse2.Shuffle(vector1.AsInt32<float>(), byte.MaxValue).AsSingle<int>();
            Vector128<float> vector2 = Sse.Subtract(Sse.Multiply(Sse2.Shuffle(low2.AsInt32<float>(), (byte)15).AsSingle<int>(), high2), Sse.Multiply(Sse2.Shuffle(low2.AsInt32<float>(), (byte)165).AsSingle<int>(), Sse2.Shuffle(high2.AsInt32<float>(), (byte)78).AsSingle<int>()));
            Vector128<float> vector128_7 = Sse.Subtract(Sse.Multiply(Sse2.Shuffle(high1.AsInt32<float>(), (byte)15).AsSingle<int>(), low1), Sse.Multiply(Sse2.Shuffle(high1.AsInt32<float>(), (byte)165).AsSingle<int>(), Sse2.Shuffle(low1.AsInt32<float>(), (byte)78).AsSingle<int>()));
            Vector128<float> left3 = Sse.Subtract(Sse.Multiply(vector128_6, high1), Sse.Add(Sse.Multiply(low1, Sse2.Shuffle(vector2.AsInt32<float>(), (byte)204).AsSingle<int>()), Sse.Multiply(Sse2.Shuffle(low1.AsInt32<float>(), (byte)177).AsSingle<int>(), Sse2.Shuffle(vector2.AsInt32<float>(), (byte)102).AsSingle<int>())));
            Vector128<float> left4 = Sse.Subtract(Sse.Multiply(left1, low2), Sse.Add(Sse.Multiply(high2, Sse2.Shuffle(vector128_7.AsInt32<float>(), (byte)204).AsSingle<int>()), Sse.Multiply(Sse2.Shuffle(high2.AsInt32<float>(), (byte)177).AsSingle<int>(), Sse2.Shuffle(vector128_7.AsInt32<float>(), (byte)102).AsSingle<int>())));
            Vector128<float> left5 = Sse.Multiply(left1, vector128_6);
            Vector128<float> left6 = Sse.Subtract(Sse.Multiply(left2, high2), Sse.Subtract(Sse.Multiply(low2, Sse2.Shuffle(vector128_7.AsInt32<float>(), (byte)51).AsSingle<int>()), Sse.Multiply(Sse2.Shuffle(low2.AsInt32<float>(), (byte)177).AsSingle<int>(), Sse2.Shuffle(vector128_7.AsInt32<float>(), (byte)102).AsSingle<int>())));
            Vector128<float> left7 = Sse.Subtract(Sse.Multiply(vector128_5, low1), Sse.Subtract(Sse.Multiply(high1, Sse2.Shuffle(vector2.AsInt32<float>(), (byte)51).AsSingle<int>()), Sse.Multiply(Sse2.Shuffle(high1.AsInt32<float>(), (byte)177).AsSingle<int>(), Sse2.Shuffle(vector2.AsInt32<float>(), (byte)102).AsSingle<int>())));
            Vector128<float> left8 = Sse.Add(left5, Sse.Multiply(left2, vector128_5));
            Vector128<float> vector128_8 = Sse.Multiply(vector128_7, Sse2.Shuffle(vector2.AsInt32<float>(), (byte)216).AsSingle<int>());
            Vector128<float> vector128_9 = Sse3.HorizontalAdd(vector128_8, vector128_8);
            Vector128<float> right1 = Sse3.HorizontalAdd(vector128_9, vector128_9);
            Vector128<float> vector128_10 = Sse.Subtract(left8, right1);
            Vector128<float> right2 = (double)MathF.Abs(vector128_10.GetElement<float>(0)) >= 9.99999997475243E-07 ? Sse.Divide(Vector128.Create(1f, -1f, -1f, 1f), vector128_10) : throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
            Vector128<float> left9 = Sse.Multiply(left3, right2);
            Vector128<float> right3 = Sse.Multiply(left6, right2);
            Vector128<float> left10 = Sse.Multiply(left7, right2);
            Vector128<float> right4 = Sse.Multiply(left4, right2);
            fixed (float* address = mat._values)
            {
                Sse.Store(address, Sse.Shuffle(left9, right3, (byte)119));
                Sse.Store(address + 4, Sse.Shuffle(left9, right3, (byte)34));
                Sse.Store(address + 8, Sse.Shuffle(left10, right4, (byte)119));
                Sse.Store(address + 12, Sse.Shuffle(left10, right4, (byte)34));
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 16; i += 4)
            {
                builder.AppendJoin(", ", _values[i], _values[i + 1], _values[i + 2], _values[i + 3]);
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}