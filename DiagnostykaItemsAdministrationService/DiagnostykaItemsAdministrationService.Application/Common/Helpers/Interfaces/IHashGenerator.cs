namespace DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;

public interface IHashGenerator
{
    string GenerateHash(string input);
    string GenerateHash(int input);
}