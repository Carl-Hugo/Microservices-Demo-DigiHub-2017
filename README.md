# Microservices-Demo-DigiHub-2017
This is the demo project that I built to demonstrate some microservices design patterns for my talk 
during the "Colloque TI 2017" at DigiHub Shawinigan.

**This is not production ready code, this is demo code.**

## Prerequisites
### Nuget Feeds
The `Users.Read` project leverage the ASP.NET Core 2.0 (not yet released) router.
You will need the AspNet Core CI MyGet feed to restore those dependencies.

I also used some of my packages that are still in the design/experimental phases. 
I will ultimately open-source them, but for now, I deployed some preliminary packages versions to my public MyGet feed.

**Feeds URI:**
- https://www.myget.org/F/forevolve/api/v3/index.json
- https://dotnet.myget.org/F/aspnetcore-ci-dev/api/v3/index.json

## Softwares, accounts, services
The code is built using [Visual Studio 2017](https://www.visualstudio.com/) and use Microsoft Azure Storage. 
You need a [Microsoft Azure](https://azure.microsoft.com/) account to make this work. 
You will also need to create a “Storage account” inside Azure.
That said, the Azure emulator might do the trick as well.

Auth0 is another service that you will need access to before running the code.
I used [Auth0](https://auth0.com/) to add jwt bearer authentication to the Gateway. 
*All other projects are not using any kind of security mechanism. Beware!* 

**All of this can be achieved for free.**

## Projects credentials
All credentials are stored as `user secrets`. There are placeholders in the `appsettings.json` files 
indicating the configuration `keys` to be set.

**Example `Microservices.Gateway`:**
```Json
  "Auth0": {
    "Domain": "[domain here]",
    "ClientId": "[client id here]",
    "ClientSecret": "[client secret here]",
    "CallbackUrl": "https://microservicesgateway2017.azurewebsites.net/signin-auth0"
  },
```

## The future of this repo
If there is interest in this project, I could make this evolve, and if you want to help, you are more than welcome.

If there are interests in the libraries I am building (the `ForEvolve.*` packages), feel free to contact me. 
And again, help are always welcome.

### Open source projects
For an up to date list of `ForEvolve.*` open source projects, see [ForEvolve Toc](https://github.com/ForEvolve/Toc).
