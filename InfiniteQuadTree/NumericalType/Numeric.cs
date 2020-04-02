using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Diagnostics.Contracts;

/// <summary>
/// A struct to represent a numerical value that can handle a large number of significant digits to a very large or small exponent (2^-2048?)
/// but for now we will just copy the double implementation
/// </summary>
namespace InfiniteQuadTree.NumericalType
{


    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [System.Runtime.InteropServices.ComVisible(true)]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct Numeric : IComparable, IFormattable, IConvertible, IComparable<Numeric>, IEquatable<Numeric>
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // TODO: when updating to custom numeric type, implement the following interfaces as well.
    //
    // , IDeserializationCallback
    {
        internal double m_value;
        //
        // Public Constants
        //
        public const double MinValue = -1.7976931348623157E+308;
        public const double MaxValue = 1.7976931348623157E+308;

        // Note Epsilon should be a double whose hex representation is 0x1
        // on little endian machines.
        public const double Epsilon = 4.9406564584124654E-324;
        public const double NegativeInfinity = (double)-1.0 / (double)(0.0);
        public const double PositiveInfinity = (double)1.0 / (double)(0.0);
        public const double NaN = (double)0.0 / (double)0.0;
        internal static double NegativeZero = BitConverter.Int64BitsToDouble(unchecked((long)0x8000000000000000));


        public Numeric(object v) : this()
        {
            this.m_value = Convert.ToDouble(v);
        }

        [Pure]
        [System.Security.SecuritySafeCritical]  // auto-generated
        public unsafe static bool IsInfinity(double d)
        {
            return (*(long*)(&d) & 0x7FFFFFFFFFFFFFFF) == 0x7FF0000000000000;
        }

        [Pure]
        public static bool IsPositiveInfinity(double d)
        {
            //Jit will generate inlineable code with this
            if (d == double.PositiveInfinity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Pure]
        public static bool IsNegativeInfinity(double d)
        {
            //Jit will generate inlineable code with this
            if (d == double.NegativeInfinity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Pure]
        [System.Security.SecuritySafeCritical]  // auto-generated
        internal unsafe static bool IsNegative(double d)
        {
            return (*(UInt64*)(&d) & 0x8000000000000000) == 0x8000000000000000;
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Security.SecuritySafeCritical]
        public unsafe static bool IsNaN(double d)
        {
            return (*(UInt64*)(&d) & 0x7FFFFFFFFFFFFFFFL) > 0x7FF0000000000000L;
        }

        // Compares this object to another object, returning an instance of System.Relation.
        // Null is considered less than any instance.
        //
        // If object is not of type Double, this method throws an ArgumentException.
        //
        // Returns a value less than zero if this  object
        //
        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (obj is Double || obj is Numeric)
            {
                double d = (double)obj;
                if (m_value < d) return -1;
                if (m_value > d) return 1;
                if (m_value == d) return 0;

                // At least one of the values is NaN.
                if (IsNaN(m_value))
                    return (IsNaN(d) ? 0 : -1);
                else
                    return 1;
            }
            throw new ArgumentException("Arg Must Be Double, or Numeric");

        }

        public int CompareTo(Numeric other)
        {
            if (m_value < other.m_value) return -1;
            if (m_value > other.m_value) return 1;
            if (m_value == other.m_value) return 0;

            // At least one of the values is NaN.
            if (IsNaN(m_value))
                return (IsNaN(other.m_value) ? 0 : -1);
            else
                return 1;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return m_value.ToString(format, formatProvider);
        }

        public TypeCode GetTypeCode()
        {
            return m_value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToBoolean(provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToChar(provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToSByte(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToByte(provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToInt16(provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToUInt16(provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToInt32(provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToUInt32(provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToInt64(provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToUInt64(provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToSingle(provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToDouble(provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToDecimal(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToDateTime(provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return m_value.ToString(provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)m_value).ToType(conversionType, provider);
        }
        public static bool operator ==(Numeric n1, object n2)
        {
            return n1.m_value.Equals(n2);
        }

        [System.Security.SecuritySafeCritical]  // auto-generated
        public static bool operator !=(Numeric n1, object n2)
        {
  
              return !n1.m_value.Equals(n2);
        }

        public override bool Equals(object obj)
        {
            try
            {
                Numeric val = (Numeric)obj;
                return m_value == val.m_value;
            } catch (Exception)
            {
                return false;
            }
        }

        public bool Equals(Numeric other)
        {
            if (other.m_value == m_value)
            {
                return true;
            }
            return IsNaN(other.m_value) && IsNaN(m_value);
        }

        // bool, char, sbyte, byte, 
        // int, double, decimal, float, long, short, uint, ulong, ushort
        public static implicit operator Numeric(int v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(double v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(decimal v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(float v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(long v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(short v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(uint v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(ulong v)
        {
            return new Numeric(v);
        }
        public static implicit operator Numeric(ushort v)
        {
            return new Numeric(v);
        }

    }

}
