using SECS_Code;
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
ConfigurationManager configuration = builder.Configuration;
var app = builder.Build();

//var test = new Test();
var test = new Worker(configuration);
// Configure the HTTP request pipeline.
while(true){
    Thread.Sleep(86400000);
}
/*
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
*/

