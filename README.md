# PDND Client Assertion Generator

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![issues - pdnd-client-assertion-generator](https://img.shields.io/github/issues/engineering87/pdnd-client-assertion-generator)](https://github.com/engineering87/pdnd-client-assertion-generator/issues)
[![Language - C#](https://img.shields.io/static/v1?label=Language&message=C%23&color=blueviolet)](https://dotnet.microsoft.com/it-it/languages/csharp)
[![stars - pdnd-client-assertion-generator](https://img.shields.io/github/stars/engineering87/pdnd-client-assertion-generator?style=social)](https://github.com/engineering87/pdnd-client-assertion-generator)

.NET implementation of **OAuth2** authentication for **PDND** service with client assertion generation.

## Voucher
Vouchers are simple JWT tokens. The implemented authentication flow is OAuth 2.0, which refers to **RFC6750** for the use of Bearer tokens and to **RFC7521** for client authorization via client assertion.

## Requesting a Voucher
To obtain a valid voucher, you must first upload at least one public key to an interop API client. The first step is to create a valid client assertion and sign it with your private key (which must match the public key registered with the client on PDND Interoperabilità). The client assertion consists of a header and a payload.

## How to Use the Client Assertion Generator
To properly set up and use the Client Assertion Generator in your ASP.NET Core application, follow these steps:

1. Configure Client Assertion Settings, an example below:

  ```xml
  "ClientAssertionConfig": {
    "ServerUrl": "",
    "KeyId": "ZmYxZGE2YjQtMzY2Yy00NWI5LThjNGItMDJmYmQyZGIyMmZh",
    "Algorithm": "RS256",
    "Type": "at+jwt",
    "ClientId": "9b361d49-33f4-4f1e-a88b-4e12661f2309",
    "Issuer": "interop.pagopa.it",
    "Subject": "9b361d49-33f4-4f1e-a88b-4e12661f2309",
    "Audience": "https://erogatore.example/ente-example/v1",
    "PurposeId": "1b361d49-33f4-4f1e-a88b-4e12661f2300",
    "KeyPath": "/path/",
    "Duration": "600"
  },
  ```

read the configuration:

  ```csharp
  builder.Services.Configure<ClientAssertionConfig>(configuration.GetSection("ClientAssertionConfig"));
  ```

2. Register Services:
  ```csharp
builder.Services.AddScoped<ClientAssertionConfig>();
builder.Services.AddScoped<IOAuth2Service, OAuth2Service>();
builder.Services.AddScoped<IClientAssertionGenerator, ClientAssertionGeneratorService>();
  ```

Then you can use `ClientAssertionGeneratorService`, which provides the following methods:
- `GetClientAssertionAsync`
- `GetToken(clientAssertion)`

## How to Contribute
Thank you for considering to help out with the source code!
If you'd like to contribute, please fork, fix, commit and send a pull request for the maintainers to review and merge into the main code base.

 * [Setting up Git](https://docs.github.com/en/get-started/getting-started-with-git/set-up-git)
 * [Fork the repository](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/working-with-forks/fork-a-repo)
 * [Open an issue](https://github.com/engineering87/pdnd-client-assertion-generator/issues) if you encounter a bug or have a suggestion for improvements/features

## Licensee
Repository source code is available under MIT License, see license in the source.

## Contact
Please contact at francesco.delre.87[at]gmail.com for any details.
