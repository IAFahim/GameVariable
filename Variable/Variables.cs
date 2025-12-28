using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Variable
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedByte :
        IEquatable<BoundedByte>,
        IComparable<BoundedByte>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public byte Current;
        public byte Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedByte(byte max, byte current)
        {
            Max = max;
            Current = current > max ? max : current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedByte(byte max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out byte current, out byte max)
        {
            current = Current;
            max = Max;
        }

        private BoundedByte(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetByte(nameof(Max));
            byte raw = info.GetByte(nameof(Current));
            Current = raw > Max ? Max : raw;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Max == 0 ? 0.0 : (double)Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == Max;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte(BoundedByte value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return ToString();
            }
        }

        public override bool Equals(object obj) => obj is BoundedByte other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedByte other) => Current == other.Current && Max == other.Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedByte other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedByte other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedByte)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        public TypeCode GetTypeCode() => TypeCode.Byte;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => Current;
        int IConvertible.ToInt32(IFormatProvider provider) => Current;
        long IConvertible.ToInt64(IFormatProvider provider) => Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedByte left, BoundedByte right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedByte left, BoundedByte right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedByte left, BoundedByte right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedByte left, BoundedByte right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedByte left, BoundedByte right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedByte left, BoundedByte right) => left.CompareTo(right) >= 0;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedShort :
        IEquatable<BoundedShort>,
        IComparable<BoundedShort>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public short Current;
        public short Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedShort(short max, short current)
        {
            Max = max;
            Current = current > max ? max : (current < 0 ? (short)0 : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedShort(short max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < 0 ? (short)0 : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out short current, out short max)
        {
            current = Current;
            max = Max;
        }

        private BoundedShort(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetInt16(nameof(Max));
            short raw = info.GetInt16(nameof(Current));
            Current = raw > Max ? Max : (raw < 0 ? (short)0 : raw);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Max == 0 ? 0.0 : (double)Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == Max;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator short(BoundedShort value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return ToString();
            }
        }

        public override bool Equals(object obj) => obj is BoundedShort other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedShort other) => Current == other.Current && Max == other.Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedShort other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedShort other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedShort)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        public TypeCode GetTypeCode() => TypeCode.Int16;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => Current;
        int IConvertible.ToInt32(IFormatProvider provider) => Current;
        long IConvertible.ToInt64(IFormatProvider provider) => Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedShort left, BoundedShort right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedShort left, BoundedShort right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedShort left, BoundedShort right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedShort left, BoundedShort right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedShort left, BoundedShort right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedShort left, BoundedShort right) => left.CompareTo(right) >= 0;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedInt :
        IEquatable<BoundedInt>,
        IComparable<BoundedInt>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public int Current;
        public int Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max, int current)
        {
            Max = max;
            Current = current > max ? max : (current < 0 ? 0 : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < 0 ? 0 : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out int current, out int max)
        {
            current = Current;
            max = Max;
        }

        private BoundedInt(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetInt32(nameof(Max));
            int raw = info.GetInt32(nameof(Current));
            Current = raw > Max ? Max : (raw < 0 ? 0 : raw);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Max == 0 ? 0.0 : (double)Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == Max;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(BoundedInt value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return ToString();
            }
        }

        public override bool Equals(object obj) => obj is BoundedInt other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedInt other) => Current == other.Current && Max == other.Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedInt other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedInt other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedInt)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        public TypeCode GetTypeCode() => TypeCode.Int32;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;
        int IConvertible.ToInt32(IFormatProvider provider) => Current;
        long IConvertible.ToInt64(IFormatProvider provider) => Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedInt left, BoundedInt right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedInt left, BoundedInt right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedInt left, BoundedInt right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedInt left, BoundedInt right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedInt left, BoundedInt right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedInt left, BoundedInt right) => left.CompareTo(right) >= 0;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedLong :
        IEquatable<BoundedLong>,
        IComparable<BoundedLong>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public long Current;
        public long Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedLong(long max, long current)
        {
            Max = max;
            Current = current > max ? max : (current < 0 ? 0L : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedLong(long max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < 0 ? 0L : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out long current, out long max)
        {
            current = Current;
            max = Max;
        }

        private BoundedLong(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetInt64(nameof(Max));
            long raw = info.GetInt64(nameof(Current));
            Current = raw > Max ? Max : (raw < 0 ? 0L : raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Max == 0 ? 0.0 : (double)Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == Max;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator long(BoundedLong value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return ToString();
            }
        }

        public override bool Equals(object obj) => obj is BoundedLong other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedLong other) => Current == other.Current && Max == other.Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedLong other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedLong other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedLong)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        public TypeCode GetTypeCode() => TypeCode.Int64;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;
        int IConvertible.ToInt32(IFormatProvider provider) => (int)Current;
        long IConvertible.ToInt64(IFormatProvider provider) => Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedLong left, BoundedLong right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedLong left, BoundedLong right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedLong left, BoundedLong right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedLong left, BoundedLong right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedLong left, BoundedLong right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedLong left, BoundedLong right) => left.CompareTo(right) >= 0;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedFloat :
        IEquatable<BoundedFloat>,
        IComparable<BoundedFloat>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public float Current;
        public float Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedFloat(float max, float current)
        {
            Max = max;
            Current = current > max ? max : (current < 0f ? 0f : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedFloat(float max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < 0f ? 0f : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out float current, out float max)
        {
            current = Current;
            max = Max;
        }

        private BoundedFloat(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetSingle(nameof(Max));
            float raw = info.GetSingle(nameof(Current));
            Current = raw > Max ? Max : (raw < 0f ? 0f : raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Math.Abs(Max) < float.Epsilon ? 0.0 : Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Max) < float.Epsilon;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current) < float.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float(BoundedFloat value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return $"{Current.ToString(format, formatProvider)}/{Max.ToString(format, formatProvider)}";
            }
        }

        public override bool Equals(object obj) => obj is BoundedFloat other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedFloat other) => Current.Equals(other.Current) && Max.Equals(other.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedFloat other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedFloat other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedFloat)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        public TypeCode GetTypeCode() => TypeCode.Single;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => (decimal)Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;
        int IConvertible.ToInt32(IFormatProvider provider) => (int)Current;
        long IConvertible.ToInt64(IFormatProvider provider) => (long)Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedFloat left, BoundedFloat right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedFloat left, BoundedFloat right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) >= 0;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedDouble :
        IEquatable<BoundedDouble>,
        IComparable<BoundedDouble>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public double Current;
        public double Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedDouble(double max, double current)
        {
            Max = max;
            Current = current > max ? max : (current < 0.0 ? 0.0 : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedDouble(double max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < 0.0 ? 0.0 : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double current, out double max)
        {
            current = Current;
            max = Max;
        }

        private BoundedDouble(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetDouble(nameof(Max));
            double raw = info.GetDouble(nameof(Current));
            Current = raw > Max ? Max : (raw < 0.0 ? 0.0 : raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Math.Abs(Max) < double.Epsilon ? 0.0 : Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Max) < double.Epsilon;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current) < double.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double(BoundedDouble value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return $"{Current.ToString(format, formatProvider)}/{Max.ToString(format, formatProvider)}";
            }
        }

        public override bool Equals(object obj) => obj is BoundedDouble other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedDouble other) => Current.Equals(other.Current) && Max.Equals(other.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedDouble other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedDouble other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedDouble)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        public TypeCode GetTypeCode() => TypeCode.Double;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => (decimal)Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;
        int IConvertible.ToInt32(IFormatProvider provider) => (int)Current;
        long IConvertible.ToInt64(IFormatProvider provider) => (long)Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => (float)Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        public static bool operator ==(BoundedDouble left, BoundedDouble right) => left.Equals(right);
        public static bool operator !=(BoundedDouble left, BoundedDouble right) => !left.Equals(right);
        public static bool operator <(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) < 0;
        public static bool operator <=(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) <= 0;
        public static bool operator >(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) > 0;
        public static bool operator >=(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) >= 0;
    }
}