using MemoryCardGame.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MemoryCardGame; // ✅ thêm dòng này — namespace của App.razor

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");          // ✅ bỏ comment
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient());

var cardValues = new[]
{
    "🍎","🍌","🍇","🍊","🍓","🥝","🍑","🍒",
    "🍎","🍌","🍇","🍊","🍓","🥝","🍑","🍒"
};
builder.Services.AddScoped(_ => new GameLogic(cardValues));

await builder.Build().RunAsync();