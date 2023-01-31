namespace DiagnostykaItemsAdministrationService.Application.Common.Helpers;

internal static class ValidationMessages
{
    public static string GetStringLengthMessage(int min, int max) => $"Property length must be between {min} and {max}";
    public static string GetIntValueGreaterThanMessage(int greaterThanValue) => $"Property need to be greater than {greaterThanValue}";
}