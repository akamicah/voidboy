using DirectoryService.Api;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);

startup.ConfigureBuilder(builder);

var app = builder.Build();

startup.Configure(app);
