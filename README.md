<p align="center">
  <img src="https://raw.githubusercontent.com/AuthwareCloud/dotnet-sdk/master/authware-s.png" width="75" height="75">
  <h1 align="center">Authware ASP.NET Core SDK</h1>
  <p align="center">Wrapper for Authware for ASP.NET Core & Blazor (.NET standard 2.1)</p>
   <p align="center">
  <a href="https://docs.authware.org">View our documentation</a>
  </p>
</p>

## 📲 Installation

Run this in your package manager console:

```
PM> Install-Package Authware.AspNetCore
```

## 👩‍💻 Usage

Usage of this library is easy as pie, it follows the exact same principal of the original .NET SDK, the only difference is how you initialize it

Firstly, you'll need to add it to your service collection, along with the HttpClientFactory dependency

```cs
services.AddHttpClient();
services.AddAuthware(options => {
    options.AppId = "Your app ID goes here";
});
```

Then you're done, you can access the library via the `AuthwareApplication` class, which has been injected into your services.

Here's an example of how you can access it in a Web API context

```cs
private readonly AuthwareApplication _authware;

public MyController(AuthwareApplication authware)
{
    _authware = authware;
}

[Route("/auth")]
[HttpGet]
public async Task<IActionResult> AuthAsync([FromBody] LoginForm ctx)
{
    try
    {
        var (token, profile) = await _authware.LoginAsync(ctx.Username, ctx.Password);

        return Ok(new 
        {
            error = false,
            token = token.AuthToken,
            username = profile.Username,
            email = profile.Email
        });
    }
    catch (AuthwareException ex)
    {
        return BadRequest(new 
        {
            error = true,
            message = ex.Message
        });
    }
}
```

## 🖥️ Compilation

In-order for compilation of the library, you must have the following:

- .NET SDK 5.0+

Note: Compilation with .NET 5.0+ is required for the instructions noted here.

1. Clone the repository

```
git clone https://github.com/AuthwareCloud/aspnetcore-sdk.git && cd dotnet-sdk
```

2. Tell `dotnet` to compile the library

```
dotnet build
```

3. All done! Navigate to the bin/Release or bin/Debug folder to find the `.nupkg` or `.dll` file.

## 📜 License

Licensed under the MIT license, see LICENSE.MD
