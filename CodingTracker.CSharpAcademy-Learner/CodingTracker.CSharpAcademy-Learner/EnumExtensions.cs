using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner
{
    internal static class EnumExtensions
    {
        internal static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? enumValue.ToString();
        }
    }
}
