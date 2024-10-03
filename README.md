# Aptos Unity SDK (Beta)

![License][github-license]

## Overview

The Aptos Unity SDK is a library that provides a convenient way to interact with the Aptos blockchain using C# under the .NET framework. The SDK is designed to offer all the necessary tools to build applications that interact with the Aptos blockchain.

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

## Installation

### Unity Package Manager (UPM)

1. Open the Unity Package Manager (`Window` > `Package Manager`).
2. Click on the `+` button and select `Add package from git URL...`.
3. Enter the URL of the Aptos Unity SDK path in this repository: 
```bash
https://github.com/aptos-labs/unity-sdk?path=/Packages/com.aptoslabs.aptos-unity-sdk
```

### Import Asset Package
1. Go to [`Releases`](https://github.com/aptos-labs/unity-sdk/releases) and download the latest release.
2. Drag and drop the `.unitypackage` file into your Unity project.

### Unity Asset Store

The Aptos Unity SDK will be available on the Unity Asset Store soon.

## Contributing

The Aptos Unity SDK is under [`Packages/com.aptoslabs.aptos-unity-sdk`](/Packages/com.aptoslabs.aptos-unity-sdk/) in this repository. Make sure to make any necessary changes in the appropriate directory.

### Updating the .NET SDK

To update the .NET SDK, follow these steps:

1. Open the NuGet package manager located towards the top of the Editor.
2. Search for the `Aptos` package. **Make sure that "Show Pre-release" is checked.**
3. Install the package, it should automatically update it in the [`com.aptoslabs.aptos-unity-sdk/Runtime/Libraries`](/Packages/com.aptoslabs.aptos-unity-sdk/Runtime/Libraries/) directory.

### Exporting a Unity Package

To export the SDK, you can use the HybridPackage workflow. This will create a `.unitypackage` file from the Package Manager. Follow these steps:

1. Open the `Asset Store Tools` window located towards the top of the Editor and open `Asset Store Uploader`.
2. Login to your Publisher account. If you don't have one, you can create one [here](https://publisher.unity.com/).
   * Once you have created an account, make sure to create a package for testing. In the image below, we can see that we've created an "Export" package for the sole purpose of exporting our package.
   ![Create a testing package](https://i.imgur.com/TuYW9tr.png)
3. In the `Asset Store Uploader` window, click on the package you created for testing.
4. Change the `Upload Type` to `Local UPM Package` and change the `Package path`  to `Packages/com.aptoslabs.aptos-unity-sdk`.
![Exporting the package](https://i.imgur.com/0lblj4h.png)
5. Click on `Export` and this will create a `.unitypackage` file for the `Packages/com.aptoslabs.aptos-unity-sdk` directory.
6. (Optional) Click on `Validate` to make sure that the package doesn't have any errors.
7. You are done! You can distribute this Unity Package for users to import into their projects.
   
[github-license]: https://img.shields.io/github/license/aptos-labs/aptos-ts-sdk