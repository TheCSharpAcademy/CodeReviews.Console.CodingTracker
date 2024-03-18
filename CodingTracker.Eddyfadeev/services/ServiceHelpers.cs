using System.Reflection;
using CodingTracker.enums;
using Spectre.Console;

namespace CodingTracker.services;

/// <summary>
/// Provides helper methods for invoking actions based on menu entries.
/// </summary>
internal abstract class ServiceHelpers
{
    /// <summary>
    /// Invokes the action associated with the given menu entry.
    /// </summary>
    /// <param name="entry">The menu entry to invoke the action for.</param>
    /// <param name="actionInstance">The instance of the object that contains the action.</param>
    internal static void InvokeActionForMenuEntry(Enum entry, object actionInstance)
    {
        var entryFieldInfo = entry.GetType().GetField(entry.ToString());
        var methodAttribute = entryFieldInfo!.GetCustomAttribute<EnumHelpers.MethodAttribute>();

        if (methodAttribute != null)
        {
            var method = actionInstance.GetType().GetMethod(
                methodAttribute.MethodName, 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (method != null)
            {
                method.Invoke(actionInstance, null);
            }
            else
            {
                AnsiConsole.WriteLine($"Method '{methodAttribute.MethodName}' not found.");
            }
        }
        else
        {
            AnsiConsole.WriteLine($"No methods assigned for {entry}.");
        }
    }
}