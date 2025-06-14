namespace XO.Console.Cli.Model;

/// <summary>
/// Represents a delegate that parses and assigns the value of a command parameter.
/// </summary>
/// <param name="context">The command execution context. This delegate is expected to set a value on <see cref="CommandContext.Parameters"/> or the context itself.</param>
/// <param name="values">The sequence of string values to convert.</param>
/// <param name="converters">A collection of runtime-configured converters.</param>
public delegate void CommandParameterSetter(
    CommandContext context,
    IEnumerable<string> values,
    IReadOnlyDictionary<Type, Delegate> converters);

/// <summary>
/// Represents an argument or option to a command-line command.
/// </summary>
public abstract class AbstractCommandParameter : IEquatable<AbstractCommandParameter>
{
    /// <summary>
    /// Initializes a new instance of <see cref="AbstractCommandParameter"/>.
    /// </summary>
    /// <param name="declaringType">The type that declares the target property.</param>
    /// <param name="propertyName">The name of the target property.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="setter">A delegate that parses and assigns the value of the argument.</param>
    /// <param name="valueType">The type of value the parameter accepts. (If the argument accepts multiple values, this is the type of each individually.)</param>
    /// <param name="description">A description of this parameter, which is used in generated help.</param>
    protected AbstractCommandParameter(
        Type declaringType,
        string propertyName,
        string name,
        CommandParameterSetter setter,
        Type valueType,
        string? description)
    {
        this.DeclaringType = declaringType;
        this.PropertyName = propertyName;
        this.Name = name;
        this.Setter = setter;
        this.ValueType = valueType;
        this.Description = description;
    }

    /// <summary>
    /// Gets the type that declares the target property.
    /// </summary>
    /// <remarks>
    /// This value is used to identify equivalent parameters across derived parameters types.
    /// </remarks>
    public Type DeclaringType { get; }

    /// <summary>
    /// Gets the name of the target property.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the parameter name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a delegate that sets the value of this parameter.
    /// </summary>
    public CommandParameterSetter Setter { get; }

    /// <summary>
    /// Get the type of value this parameter accepts.
    /// </summary>
    public Type ValueType { get; }

    /// <summary>
    /// Gets a description of this parameter, used in generated help.
    /// </summary>
    public string? Description { get; }

    /// <inheritdoc/>
    /// <remarks>
    /// Two parameters are considered equal if they are of the same type and have equal <see cref="DeclaringType"/>,
    /// <see cref="PropertyName"/>, and <see cref="Name"/>.
    /// </remarks>
    public bool Equals(AbstractCommandParameter? other)
    {
        return other?.GetType() == this.GetType()
            && other.DeclaringType == this.DeclaringType
            && other.PropertyName == this.PropertyName
            && other.Name == this.Name;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
        => Equals(obj as AbstractCommandParameter);

    /// <inheritdoc/>
    public override int GetHashCode()
        => HashCode.Combine(this.DeclaringType, this.PropertyName);

    /// <inheritdoc/>
    public override string ToString()
        => this.Name;

    /// <inheritdoc/>
    public static bool operator ==(AbstractCommandParameter? left, AbstractCommandParameter? right)
        => left?.Equals(right) == true;

    /// <inheritdoc/>
    public static bool operator !=(AbstractCommandParameter? left, AbstractCommandParameter? right)
        => left?.Equals(right) == false;
}
