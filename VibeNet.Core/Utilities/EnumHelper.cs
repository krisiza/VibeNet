using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VibeNet.Core.Utilities;

public static class EnumHelper
{
    public static string GetDisplayName(this Enum value)
    {
        var type = value.GetType();
        var member = type.GetMember(value.ToString()).FirstOrDefault();
        var attribute = member?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();
    }
}