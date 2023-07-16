
using Circle.Shared.Enums;
using Circle.Shared.Security.Permission;
using System.ComponentModel;
using System.Reflection;

namespace Circle.Shared.Helpers
{
    public static class PermissionHelper
    {

        public static IEnumerable<PermissionProperties> GetAllPermissions(this Permission[] value)
        {
            var perms = value
                .Select(p => new PermissionProperties
                {
                    Id = p.ToString(),
                    Name = p.ToString(),
                    Description = p.GetPermissionDescription(),
                    Category = p.GetPermissionCategory()
                });

            return perms;
        }

        public static string GetPermissionDescription(this Permission value)
        {
            Type type = value.GetType();

            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute? attr = Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }

        public static string GetPermissionCategory(this Permission value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    CategoryAttribute? attr = Attribute.GetCustomAttribute(field,
                             typeof(CategoryAttribute)) as CategoryAttribute;

                    if (attr != null)
                    {
                        return attr.Category;
                    }
                }
            }

            return null;
        }
    }
}
