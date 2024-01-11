namespace Domain.Helpers
{
    public static class ObjectCreationHelper
    {
        public static object GenerateObject(Type input)
        {
            var objectInstance = Activator.CreateInstance(input);

            var objectProperties = input.GetProperties();
            foreach (var property in objectProperties)
            {
                if (property.PropertyType.IsPrimitive)
                {
                    var propertyType = property.PropertyType;
                    var defaultValue = GetDefaultPrimitiveValue(propertyType);
                    property.SetValue(objectInstance, defaultValue);
                }
            }

            return objectInstance;
        }

        private static object GetDefaultPrimitiveValue(Type type)
        {
            if (type == typeof(string))
            {
                return string.Empty;
            }
            else if (type == typeof(int))
            {
                return 0;
            }
            else if (type == typeof(float))
            {
                return 0.0f;
            }
            else if (type == typeof(double))
            {
                return 0.0;
            }
            else if (type == typeof(decimal))
            {
                return 0.0m;
            }
            else if (type == typeof(bool))
            {
                return false;
            }
            else if (type == typeof(char))
            {
                return '\0';
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}
