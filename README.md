# CSharp-CommonSecrets

CSharp library implementation of [CommonSecrets](https://github.com/mcraiha/CommonSecrets) format.

## What is CommonSecrets

CommonSecrets is specification for storing encrypted and plaintext login information (username, password, URL etc.), notes and files.

## Build status

[![CircleCI](https://circleci.com/gh/mcraiha/CSharp-CommonSecrets.svg?style=svg)](https://circleci.com/gh/mcraiha/CSharp-CommonSecrets)

## What could I do with it

You can e.g. create you password manager that is compatible with CommonSecrets specifications.

## How to use

1. Either use nuget package (that is upcoming) OR download **src** folder content and add it to your project
2. Create CommonSecretsContainer and add stuff to it
3. Serialize to your chosen data-interchange format (JSON, XML, YAML etc.)
4. Deserialize the content and continue to add stuff to it
5. Jump back to step 3.

Same in actual code, step 2.
```csharp
using CSCommonSecrets;

// Create CommonSecretsContainer
CommonSecretsContainer csc = new CommonSecretsContainer();

string password = "dragon667";
byte[] initialCounter1 = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
SettingsAES_CTR settingsAES_CTR1 = new SettingsAES_CTR(initialCounter1);
SymmetricKeyAlgorithm skaAES = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR1);

KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry("master");

byte[] derivedPassword = kdfe.GeneratePasswordBytes(password);

// Add Key Derivation Function to CommonSecretsContainer
csc.keyDerivationFunctionEntries.Add(kdfe);

// Add Login Information secret
csc.loginInformationSecrets.Add(new LoginInformationSecret(new LoginInformation("My site", "https://example.com", "CoolGuy1990", "NobodyKnows1!"), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));
```

step 3.
```csharp
using CSCommonSecrets;
using System.Text.Json;
using System.Text.Json.Serialization;

string json = JsonSerializer.Serialize(csc, serializerOptions);
```

step 4.
```csharp
using CSCommonSecrets;
using System.Text.Json;
using System.Text.Json.Serialization;

CommonSecretsContainer cscDeserialized = JsonSerializer.Deserialize<CommonSecretsContainer>(json);
```


## License

Library and its source code are licensed under [Unlicense](LICENSE), so you might use these as you wish.

## Version history

0.1 First public release (this has NOT happened yet)