using ModelContextProtocol.Server;
using RebrickableMcp;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<RebrickableOptions>()
    .Bind(builder.Configuration.GetSection(RebrickableOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey),
        "Rebrickable:ApiKey is not configured. Set it via user-secrets or environment variable.")
    .ValidateOnStart();

builder.Services.AddHttpClient<RebrickableClient>();

builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        // Stateless mode is recommended for servers that don't need
        // server-to-client requests like sampling or elicitation.
        // See the Sessions documentation for details.
        options.Stateless = true;
    })
    .WithToolsFromAssembly();
var app = builder.Build();

app.MapMcp();

app.Run();
