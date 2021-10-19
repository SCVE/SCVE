using System;
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
                throw new ScveException($"Values must contain 16 elements, but was {values.Length}");
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
        /// Implicit cinversion to float array
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
            result._values[3 * 4 + 0] = -(right + left) * invWidth;
            result._values[3 * 4 + 1] = -(top + bottom) * invHeight;
            result._values[3 * 4 + 2] = -(depthFar + depthNear) * invDepth;

            return result;
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