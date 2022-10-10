namespace Orc.LicenseManager
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Markup;
    using Catel;

    internal class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumerationExtension(Type enumType)
        {
            Argument.IsNotNull(() => enumType);

            EnumType = enumType;
        }

        private Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (_enumType == value)
                {
                    return;
                }

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (!enumType.IsEnum)
                {
                    throw new ArgumentException("Type must be an Enum.");
                }

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Argument.IsNotNull(() => serviceProvider);

            var enumValues = Enum.GetValues(EnumType);

            return (
                from object enumValue in enumValues
                select new EnumerationMember
                {
                    Value = enumValue,
                    Description = GetDescription(enumValue)
                }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof (DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute is not null ? descriptionAttribute.Description
                : enumValue.ToString();
        }

        private class EnumerationMember
        {
            public string Description { get; set; }
            public object Value { get; set; }
        }
    }
}
