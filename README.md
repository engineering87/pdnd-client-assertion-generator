# PDND Client Assertion Generator

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/PDNDClientAssertionGenerator?style=plastic)](https://www.nuget.org/packages/PDNDClientAssertionGenerator)
![NuGet Downloads](https://img.shields.io/nuget/dt/PDNDClientAssertionGenerator)
[![Build](https://github.com/engineering87/pdnd-client-assertion-generator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/engineering87/pdnd-client-assertion-generator/actions/workflows/dotnet.yml)
[![issues - pdnd-client-assertion-generator](https://img.shields.io/github/issues/engineering87/pdnd-client-assertion-generator)](https://github.com/engineering87/pdnd-client-assertion-generator/issues)
[![Language - C#](https://img.shields.io/static/v1?label=Language&message=C%23&color=blueviolet)](https://dotnet.microsoft.com/it-it/languages/csharp)
[![stars - pdnd-client-assertion-generator](https://img.shields.io/github/stars/engineering87/pdnd-client-assertion-generator?style=social)](https://github.com/engineering87/pdnd-client-assertion-generator)

.NET implementation of **OAuth2** authentication for **PDND** service with client assertion generation.

## Contents
- [PDND](#pdnd)
- [Voucher](#voucher)
- [Requesting a Voucher](#requesting-a-voucher)
- [How to Use the Client Assertion Generator](#how-to-use-the-client-assertion-generator)
- [Licensee](#licensee)
- [Contact](#contact)

## PDND
The **[Piattaforma Digitale Nazionale Dati (PDND)](https://developers.italia.it/it/pdnd/)** is an Italian digital infrastructure designed to facilitate **data interoperability** and exchange between public administrations and private entities. The platform aims to simplify the sharing of public data by providing a secure, standardized, and centralized system for data integration, access, and management. PDND promotes digital transformation within the public sector by ensuring data is accessible, reliable, and reusable, enabling more efficient public services, enhancing transparency, and supporting **data-driven decision-making** for both government and citizens.

## Voucher
Vouchers are simple JWT tokens. The implemented authentication flow is OAuth 2.0, which refers to [**RFC6750**](https://datatracker.ietf.org/doc/html/rfc6750) for the use of Bearer tokens and to [**RFC7521**](https://datatracker.ietf.org/doc/html/rfc7521) for client authorization via client assertion.

## Requesting a Voucher
To obtain a valid voucher, you must first upload at least one public key to an interop API client. The first step is to create a valid client assertion and sign it with your private key (which must match the public key registered with the client on PDND Interoperabilità). The client assertion consists of a header and a payload.

## Voucher Flow for Interoperability APIs
The user requests a voucher. Once obtained, they include it as an authorization header in subsequent calls to the PDND Interoperability APIs.

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

2. Register Services:
  ```csharp
  builder.Services.AddPDNDClientAssertionServices();
  ```

Then you can use `ClientAssertionGeneratorService`, which provides the following methods:
- `GetClientAssertionAsync`
- `GetTokenAsync(clientAssertion)`

## Testing the PDNDClientAssertionGenerator
This project includes a test application, **PDNDClientAssertionGenerator.Api**, designed to help you test the software with your own configuration. This application acts as a sandbox where you can validate the behavior of the PDNDClientAssertionGenerator components.

### How to Use the Test Application:

1. Configuration: Update the configuration settings in the `appsettings.json` file or through environment variables to match your specific use case and environment.

2. Running the Test Application:
    - Navigate to the PDNDClientAssertionGenerator.Api folder.
    - Use the following command to run the application:  
      `dotnet run --project src/PDNDClientAssertionGenerator.Api/PDNDClientAssertionGenerator.Api.csproj`
  
3. Testing Scenarios: Once the application is running, you can use various `GetClientAssertion` and `GetToken` to test the functionality of the software in different configurations.

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
