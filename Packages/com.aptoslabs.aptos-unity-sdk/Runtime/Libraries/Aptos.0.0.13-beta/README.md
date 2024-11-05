# C#/.NET SDK for Aptos (Beta)

![License][github-license]

## Overview

The Aptos .NET SDK is a library that provides a convenient way to interact with the Aptos blockchain using C# under the .NET framework. The SDK is designed to offer all the necessary tools to build applications that interact with the Aptos blockchain.

### Features

- Binary Canonical Serialization (BCS) encoding and decoding
- Ed25519, SingleKey, MultiKey, and Keyless signer support
- Utilities for transaction building, signing, and submission
- Abstractions over the Aptos Fullnode and Indexer APIs

## Usage

Initialize an instance of the `AptosClient` class to interact with the Aptos blockchain. You can use a pre-defined network configuration from the `Networks` class.

```csharp
// 1. Import the Aptos namespace
using Aptos;

// 2. Initialize the Aptos client
var config = new AptosConfig(Networks.Mainnet);
var client = new AptosClient(config);

// 3. Use the client to interact with the blockchain!
var ledgerInfo = await client.Block.GetLedgerInfo();
```

### Sign and Submit Transactions

To sign and submit a transaction, you can build a payload using the `AptosClient` and sign it with an `Account` signer.

```csharp
using Aptos;

// 1. Initialize the Aptos client
var config = new AptosConfig(Networks.Mainnet);
var client = new AptosClient(config);

// 2. Create a new account
var account = Account.Generate();

// 2. Create a transaction payload
var transaction = await client.Transaction.Build(
    sender: account,
    data: new GenerateEntryFunctionPayloadData(
        function: "0x1::aptos_account::transfer_coins",
        typeArguments: ["0x1::aptos_coin::AptosCoin"],
        functionArguments: [account.Address, "100000"]
    )
);

// 3. Sign and submit the transaction
var pendingTransaction = client.Transaction.SignAndSubmitTransaction(account, transaction);

// 4. (Optional) Wait for the transaction to be committed
var committedTransaction = await client.Transaction.WaitForTransaction(pendingTransaction);
```

### Smart Contract View Functions

Call view functions to query smart contracts.

```csharp
using Aptos;

// 1. Initialize the Aptos client
var config = new AptosConfig(Networks.Mainnet);
var client = new AptosClient(config);

// 2. Call a view function
var result = await client.Contract.View(
    new GenerateViewFunctionPayloadData(
        function: "0x1::coin::name",
        functionArguments: [],
        typeArguments: ["0x1::aptos_coin::AptosCoin"]
    )
);

// 3. Parse the return values
Console.WriteLine(result[0]);
```

## Installation

The SDK is published onto [NuGet](https://www.nuget.org/packages/Aptos/) where you can install it using the following command:

```bash
dotnet add package Aptos
```

### Unity

Find the Aptos Unity SDK repository at [aptos-labs/unity-sdk](https://github.com/aptos-labs/unity-sdk).

### Unity Package Manager (UPM)

1. Open the Unity Package Manager (`Window` > `Package Manager`).
2. Click on the `+` button and select `Add package from git URL...`.
3. Enter the URL of the Aptos Unity SDK path in this repository: 
```bash
https://github.com/aptos-labs/unity-sdk.git?path=/Packages/com.aptoslabs.aptos-unity-sdk
```

### Import Asset Package
1. Go to the [`aptos-labs/unity-sdk Releases`](https://github.com/aptos-labs/unity-sdk/releases) and download the latest release. 
2. Drag and drop the `.unitypackage` file into your Unity project.

### Godot

To install the Aptos SDK into your Godot project, you will need to add the Aptos SDK into your Godot project's `.csproj` file.

1. Find the `.csproj` file in the root of your Godot project.
2. Add the following line to the `<ItemGroup>` section of the `.csproj` file. If it doesn't exist, create it the `<ItemGroup>` section.

```xml
<Project Sdk="Godot.NET.Sdk/4.3.0">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'android' ">net7.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'ios' ">net8.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <RootNamespace>AptosSDKExample</RootNamespace>
  </PropertyGroup>

  <!-- START: Add these lines -->
  <ItemGroup>
    <PackageReference Include="Aptos" Version="0.0.1-beta" />
  </ItemGroup>
  <!-- END -->

</Project>
```

3. You can now use the Aptos SDK in your Godot project.

```csharp
using Aptos;

public partial class MyClass : Node
{
    public override void _Ready()
    {
        var client = new AptosClient(Networks.Mainnet);
        var ledgerInfo = await client.Block.GetLedgerInfo();
        Console.WriteLine($"Ledger Block Height: {ledgerInfo.BlockHeight}");
    }
}
```

## API Reference

The entire API reference can be found here: [API Reference](https://aptos-labs.github.io/aptos-dotnet-sdk/)

## Examples 

Examples can be found in the [`Aptos.Examples`](https://github.com/aptos-labs/aptos-dotnet-sdk/tree/main/Aptos.Examples) project. Run the examples by using the following command:

```bash
dotnet run --project ./Aptos.Examples --framework net8.0
```

This will prompt the follow console. You can select an example to run by entering the corresponding number or using the arrow keys to navigate the menu.

![examples-demonstration](https://i.imgur.com/YS140Zb.png)

[github-license]: https://img.shields.io/github/license/aptos-labs/aptos-ts-sdk

## Release Process

To release a new version of the SDK, you can bump the version in the `Directory.Build.props` file and push the changes to the `main` branch. The GitHub Actions workflow will automatically publish the new version to NuGet.