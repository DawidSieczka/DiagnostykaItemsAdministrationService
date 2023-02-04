using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DiagnostykaItemsAdministrationService.UnitTests.Tests;

internal class HashGeneratorTests
{
    [Test]
    [TestCase(1, "a4ayc/80/OGd")]
    [TestCase(100, "rVc2aGUSblVk")]
    [TestCase(5000000, "JhhiieExlg03")]
    public void HashGenerator_WithIntAndStringInput_ShouldReturnTheSameHashSucessfuly(int id, string hashExpectedResult)
    {
        //Arrange
        var hashGenerator = new HashGenerator();

        //Act
        var hashIntOverloadResult = hashGenerator.GenerateHash(id);
        var hashStringOveloadResult = hashGenerator.GenerateHash(id.ToString());

        //Assert
        hashIntOverloadResult.Should().Be(hashStringOveloadResult).And.Be(hashExpectedResult);
    }
}