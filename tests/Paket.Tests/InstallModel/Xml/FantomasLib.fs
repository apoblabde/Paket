﻿module Paket.InstallModel.Xml.FantomasLibSpecs

open Paket
open NUnit.Framework
open FsUnit
open Paket.TestHelpers
open Paket.Domain
open Paket.Requirements

let expected = """
<ItemGroup xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Reference Include="FantomasLib">
    <HintPath>..\..\..\Fantomas\Lib\FantomasLib.dll</HintPath>
    <Private>False</Private>
    <SpecificVersion>True</SpecificVersion>
    <Paket>True</Paket>
  </Reference>
</ItemGroup>"""

[<Test>]
let ``should generate Xml for Fantomas 1.5``() =
    ensureDir()
    let model =
        InstallModel.CreateFromLibs(PackageName "Fantomas", SemVer.Parse "1.5.0", FrameworkRestriction.NoRestriction,
            [ @"..\Fantomas\Lib\FantomasLib.dll"
              @"..\Fantomas\Lib\FSharp.Core.dll"
              @"..\Fantomas\Lib\Fantomas.exe" ] |> Paket.InstallModel.ProcessingSpecs.fromLegacyList @"..\Fantomas\",
              [],
              [],
              Nuspec.Explicit ["FantomasLib.dll"])

    let ctx = ProjectFile.TryLoad("./ProjectFile/TestData/Empty.fsprojtest").Value.GenerateXml(model, System.Collections.Generic.HashSet<_>(),Map.empty,Some false,Some true,true,KnownTargetProfiles.AllProfiles,None)
    ctx.ChooseNodes.Head.OuterXml
    |> normalizeXml
    |> shouldEqual (normalizeXml expected)
