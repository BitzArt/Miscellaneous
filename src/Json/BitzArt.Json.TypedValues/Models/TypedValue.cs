using System.Diagnostics;
using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Persists actual type information for a value upon JSON serialization.
/// </summary>
[JsonConverter(typeof(TypedValueJsonConverter))]
[DebuggerDisplay("{Value}")]
public abstract class TypedValue
{
    private object? _value;

    /// <summary>
    /// The value that requires persisting type information.
    /// </summary>
    public object? Value
    {
        get => GetValue();
        set => SetValue(value);
    }

    private protected virtual object? GetValue() => _value;
    private protected virtual void SetValue(object? value) => _value = value;

    internal TypedValue()
    {
        _value = null!;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypedValue"/> class.
    /// </summary>
    /// <param name="value">The value that requires persisting type information.</param>
    internal TypedValue(object value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        if (Value is null)
        {
            return 0;
        }

        return Value.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Compares two <see cref="TypedValue"/> instances for equality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns><see langword="true"/> if instances are equal; otherwise, <see langword="false"/>.</returns>"
    public static bool operator ==(TypedValue left, TypedValue right)
        => ReferenceEquals(left, right) || (left is not null && left.Equals(right));

    /// <summary>
    /// Compares two <see cref="TypedValue"/> instances for inequality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns><see langword="true"/> if instances are not equal; otherwise, <see langword="false"/>.</returns>"
    public static bool operator !=(TypedValue left, TypedValue right)
        => !(left == right);

    /// <summary>
    /// Compares two <see cref="TypedValue"/> instances for equality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns><see langword="true"/> if instances are equal; otherwise, <see langword="false"/>.</returns>"
    public static bool operator ==(TypedValue left, object right)
        => ReferenceEquals(left, right) || (left is not null && left.Equals(right));

    /// <summary>
    /// Compares two <see cref="TypedValue"/> instances for inequality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns><see langword="true"/> if instances are not equal; otherwise, <see langword="false"/>.</returns>"
    public static bool operator !=(TypedValue left, object right)
        => !(left == right);

    /// <summary>
    /// Compares two <see cref="TypedValue"/> instances for equality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns><see langword="true"/> if instances are equal; otherwise, <see langword="false"/>.</returns>"
    public static bool operator ==(object left, TypedValue right)
        => ReferenceEquals(left, right) || (right is not null && right.Equals(left));

    /// <summary>
    /// Compares two <see cref="TypedValue"/> instances for inequality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns><see langword="true"/> if instances are not equal; otherwise, <see langword="false"/>.</returns>"
    public static bool operator !=(object left, TypedValue right)
        => !(left == right);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (Value is null)
        {
            if (obj is null)
            {
                return true;
            }

            if (obj is TypedValue t)
            {
                return t.Value is null;
            }
        }

        if (obj is TypedValue typedValue)
        {
            return Equals(typedValue.Value);
        }

        return Value!.Equals(obj);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypedValue{T}"/> class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TypedValue<T> From<T>(T value)
     => new(value);

    /// <summary>
    /// Implicitly converts a value of type <see cref="ValueType"/> to a <see cref="TypedValue{ValueType}"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator TypedValue(ValueType value)
    {
        var resultingType = typeof(TypedValue<>).MakeGenericType(value.GetType());
        return (TypedValue)Activator.CreateInstance(resultingType, value)!;
    }

    /// <summary>
    /// Implicitly converts a value of type <see cref="ValueType"/> to a <see cref="TypedValue{ValueType}"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator TypedValue(string value)
        => new TypedValue<string>(value);
}
