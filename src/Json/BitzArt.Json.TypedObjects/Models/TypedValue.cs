using System.Diagnostics;
using System.Text.Json.Serialization;

namespace BitzArt.Json;

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
    public TypedValue(object value)
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
}
