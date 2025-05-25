using System.Diagnostics;
using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Persists actual type information for a value upon JSON serialization.
/// </summary>
/// <typeparam name="TBase">Base value type.</typeparam>
[JsonConverter(typeof(TypedValueJsonConverter))]
[DebuggerDisplay("{Value}")]
public sealed class TypedValue<TBase> : TypedValue
{
    /// <inheritdoc cref="TypedValue.Value"/>
    public new TBase Value { get; set; }

    private protected override object? GetValue() => Value;
    private protected override void SetValue(object? value)
    {
        if (value is not TBase typedValue)
        {
            var typeName = value is not null ? value!.GetType().FullName : "null";
            throw new InvalidCastException($"Cannot set a value of type '{typeName}' to a TypedValue of type '{typeof(TBase).FullName}'.");
        }

        Value = typedValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypedValue{T}"/> struct.
    /// </summary>
    /// <param name="value">The value that requires persisting type information.</param>
    public TypedValue(TBase value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new <see cref="TypedValuePayload{T}"/> instance with the value and its type information.
    /// </summary>
    /// <returns>A <see cref="TypedValuePayload{T}"/> instance.</returns>
    internal TypedValuePayload<TBase> ToPayload() => new(Value);

    /// <summary>
    /// Creates a new <see cref="TypedValue{T}"/> instance from the given payload.
    /// </summary>
    /// <typeparam name="TActual">Actual value type.</typeparam>
    /// <param name="payload">Payload containing the value and its type information.</param>
    /// <returns>The new <see cref="TypedValue{T}"/> instance.</returns>
    internal static TypedValue<TBase> FromPayload<TActual>(TypedValuePayload<TActual> payload)
        where TActual : TBase
        => payload.Value;

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="TBase"/> to a <see cref="TypedValue{T}"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator TypedValue<TBase>(TBase value)
        => new(value);

    /// <summary>
    /// Implicitly converts a <see cref="TypedValue{T}"/> to a value of type <typeparamref name="TBase"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator TBase(TypedValue<TBase> value)
        => value.Value;
}
