using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using Xunit;

namespace backend.Tests;

public class CodeGeneratorTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Theory]
    [InlineData("HD")]
    [InlineData("PN")]
    [InlineData("TR")]
    [InlineData("BH")]
    [InlineData("NV")]
    [InlineData("KH")]
    public async Task NextAsync_MatchesExpectedFormatAndLength(string prefix)
    {
        // Arrange
        using var context = CreateDbContext();
        var generator = new CodeGenerator(context);

        // Act
        var code = await generator.NextAsync(prefix);

        // Assert
        Assert.NotNull(code);
        Assert.True(code.Length <= 15, $"Mã '{code}' vượt quá 15 ký tự!");
        Assert.StartsWith(prefix, code);

        // Pattern: Prefix (2 uppercase chars) + YearMonth (4 digits) + Sequence (5 digits) = 11 chars
        int digitCount = (prefix == "NV" || prefix == "KH") ? 8 : 9;
        var pattern = $"^{prefix}\\d{{{digitCount}}}$";
        Assert.Matches(new Regex(pattern), code);
    }

    [Fact]
    public async Task NextAsync_ConcurrentCalls_GeneratesUniqueCodes()
    {
        // Arrange
        using var context = CreateDbContext();
        var generator = new CodeGenerator(context);
        const int taskCount = 100;
        var generatedCodes = new ConcurrentBag<string>();

        // Act
        var tasks = Enumerable.Range(0, taskCount).Select(async _ =>
        {
            var code = await generator.NextAsync("HD");
            generatedCodes.Add(code);
        });

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(taskCount, generatedCodes.Count);
        var distinctCodes = generatedCodes.Distinct().ToList();
        Assert.Equal(taskCount, distinctCodes.Count);
    }
}
