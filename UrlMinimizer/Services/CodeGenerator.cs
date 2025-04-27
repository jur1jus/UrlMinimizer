
using System.Text;
using Microsoft.Extensions.Options;

namespace UrlMinimizer.Services;

public interface ICodeGenerator
{
    Task<string> Generate();
}

internal class CodeGenerator : ICodeGenerator
{
    private static readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static readonly int CodeLength = 7;

    private readonly ILogger<CodeGenerator> _logger;
    private readonly AppInstanceSettings _options;
    
    public CodeGenerator(ILogger<CodeGenerator> logger, IOptions<AppInstanceSettings> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<string> Generate()
    {
        _logger.LogInformation("App Instance: {0}", _options.AppInstanceName);
        var random = new Random();
        var code = new char[CodeLength];

        for (var i = 0; i < CodeLength; i++)
        {
            var randomIndex = random.Next(Alphabet.Length);
            code[i] = Alphabet[randomIndex];
        }
        
        var codeString = new string(code);
        var codeValue = $"{Guid.NewGuid()}{codeString}{_options.AppInstanceName}";
        return $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(codeValue)).Substring(0, 8)}";
    }
}