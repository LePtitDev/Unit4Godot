#if TOOLS

#nullable enable

using System.Reflection;
using System;
using System.Linq;

namespace Unit4Godot.Internal.Helpers;

internal static class ReflectionHelpers
{
    public static T? GetAttribute<T>(this ICustomAttributeProvider member, bool inherit = true) where T : Attribute
    {
        return GetAttributes<T>(member, inherit).FirstOrDefault();
    }

    public static T[] GetAttributes<T>(this ICustomAttributeProvider member, bool inherit = true) where T : Attribute
    {
        return (T[])member.GetCustomAttributes(typeof(T), inherit);
    }

    public static bool HasAttribute(this ICustomAttributeProvider member, Type attributeType, bool inherit = true)
    {
        return member.GetCustomAttributes(attributeType, inherit).Any();
    }

    public static bool HasAttribute<T>(this ICustomAttributeProvider member, bool inherit = true) where T : Attribute
    {
        return GetAttributes<T>(member, inherit).Any();
    }
}

#endif // TOOLS
