# Protobuf.CodeGen.BuildTask

[![Build status](https://img.shields.io/appveyor/ci/hanabi1224/protobuf-codegen-buildtask/master.svg)](https://ci.appveyor.com/project/hanabi1224/protobuf-codegen-buildtask)
[![Build Status](https://img.shields.io/travis/hanabi1224/Protobuf.CodeGen.BuildTask/master.svg)](https://travis-ci.org/hanabi1224/Protobuf.CodeGen.BuildTask)
[![MIT License](https://img.shields.io/github/license/hanabi1224/Protobuf.CodeGen.BuildTask.svg)](https://github.com/hanabi1224/Protobuf.CodeGen.BuildTask/blob/master/LICENSE)
[![NuGet version](https://buildstats.info/nuget/Protobuf.CodeGen.BuildTask)](https://www.nuget.org/packages/Protobuf.CodeGen.BuildTask)
====

MSBuild task that automatically uses official protoc binary in [Google.Protobuf.Tools](https://www.nuget.org/packages/Google.Protobuf.Tools/) to generate c# files from .proto files. In order to build the generated c# files, it adds official [Google.Protobuf](https://www.nuget.org/packages/Google.Protobuf/) as indirect reference to your project as well

Install via [nuget](https://www.nuget.org/packages/Protobuf.CodeGen.BuildTask)

or add below code snippet to your .csproj file

Windows, Linux, OSX are all supported and tested

```
<ItemGroup>
    <PackageReference Include="Protobuf.CodeGen.BuildTask" Version="0.1.1" >
        <PrivateAssets>All</PrivateAssets>
    </PackageReference>
</ItemGroup>
```

The generated files will be under _ProtobufCodeGen folder, which you can consider to [gitignore](https://github.com/hanabi1224/Protobuf.CodeGen.BuildTask/blob/master/test/Protobuf.CodeGen.BuildTask.Tests/.gitignore) it

The unit test project [here](https://github.com/hanabi1224/Protobuf.CodeGen.BuildTask/tree/master/test/Protobuf.CodeGen.BuildTask.Tests) is a good example to demonstrate the usage.
