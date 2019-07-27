# OculusMobileVoiceChat using MagicOnion and UnityOpus

Social VR sample of Oculus Go and Oculus Quest using MagicOnion and UnityOpus.

![demo](https://github.com/nikaera/MagicOnionExample-OculusMobileVoiceChat/blob/master/OculusMobileVoiceChat.gif)

# Dependency

## Server
- Visual Studio Code 1.36.0
- ASP.NET Core 2.2
- [MagicOnion 2.1.0(2.1.2)](https://github.com/Cysharp/MagicOnion/releases/tag/2.1.0)
  Licensed under the MIT License. Copyright (c) 2016 Yoshifumi Kawai
- [MessagePack.Unity.1.7.3.5](https://github.com/neuecc/MessagePack-CSharp/releases/tag/v.1.7.3.7)
  Licensed under the MIT License. Copyright (c) 2017 Yoshifumi Kawai

## Unity
- Unity 2019.1.4f1
- Visual Studio for Mac 8.1.5
- [Oculus Integration(1.39.0)](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022)
- [MagicOnion 2.1.0(2.1.2)](https://github.com/Cysharp/MagicOnion/releases/tag/2.1.0)
  Licensed under the MIT License. Copyright (c) 2016 Yoshifumi Kawai
- [UnityOpus 1.1.0](https://github.com/TyounanMOTI/UnityOpus/releases/tag/v1.1.0)
  Licensed under the MIT License. Copyright (c) 2018 TyounanMOTI
- [grpc_unity_package.1.22.0-dev.zip](https://packages.grpc.io/archive/2019/06/7fce9da88febd7c0d6f6e0fb90ba31e600624623-9f70381d-a50b-44b5-87cf-62e43d72037e/index.xml)
- macOS Mojave 10.14.5

# Usage

1. Move to `MagicOnionExample-OculusMobileVoiceChat/OculusMobileVoiceChat.Server` with terminal, and ｓtart MagicOnion server as follows.
```
dotnet restore
dotnet run
```

2. Open the `MagicOnionExample-OculusMobileVoiceChat/OculusMobileVoiceChat.Unity` folder with Unity.
3. Import `Oculus Integration` from Unity Asset Store.
4. Open the `MagicOnionExample-OculusMobileVoiceChat/OculusMobileVoiceChat.Unity/Assets/Scenes/Room.unity` scene.
5. Rewrite `Magic Onion Host` field of VRRoomManager GameObject.
6. Build and run Android with Oculus Go or Oculus Quest connected.
7. Rewriting the source code as you like, Have fun!