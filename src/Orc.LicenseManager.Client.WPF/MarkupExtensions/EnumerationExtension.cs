namespace Orc.LicenseManager;

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using Catel.Logging;

internal class EnumerationExtension : MarkupExtension
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly Type _enumType;

    public EnumerationExtension(Type enumType)
    {
        ArgumentNullException.ThrowIfNull(enumType);

        var underlyingType = Nullable.GetUnderlyingType(enumType) ?? enumType;

        if (!underlyingType.IsEnum)
        {
            throw Log.ErrorAndCreateException<ArgumentException>("Type must be an Enum.");
        }

        _enumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var enumValues = Enum.GetValues(_enumType);

        return (
            from object enumValue in enumValues
            select new EnumerationMember
            {
                Value = enumValue,
                Description = GetDescription(enumValue)
            }).ToArray();
    }

    private string? GetDescription(object enumValue)
    {
        var fieldName = enumValue.ToString();
        if (string.IsNullOrEmpty(fieldName))
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Cannot get field with null or empty name");
        }

        return _enumType
            .GetField(fieldName)?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() is DescriptionAttribute descriptionAttribute ? descriptionAttribute.Description
            : enumValue.ToString();
    }

    private class EnumerationMember
    {
        public string? Description { get; set; }
        public object? Value { get; set; }
    }
}
