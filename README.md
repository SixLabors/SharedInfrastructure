<h1 align="center">

<img src="https://raw.githubusercontent.com/SixLabors/Branding/main/icons/org/sixlabors.512.png" alt="SixLabors.SharedInfrastructure" width="256"/>
<br/>
SixLabors.SharedInfrastructure
</h1>

<div align="center">

[![Build Status](https://img.shields.io/github/workflow/status/SixLabors/SharedInfrastructure/Build/main)](https://github.com/SixLabors/SharedInfrastructure/actions)
[![Code coverage](https://codecov.io/gh/SixLabors/SharedInfrastructure/branch/main/graph/badge.svg)](https://codecov.io/gh/SixLabors/SharedInfrastructure)
[![License: Six Labors Split](https://img.shields.io/badge/license-Six%20Labors%20Split-%23e30183)](https://github.com/SixLabors/ImageSharp/blob/main/LICENSE)

</div>

This repository contains:
- Configuration and guidelines for automated linting of C# projects.
- Standardized internal C# utility classes to be reused across SixLabors projects (like `Guard`, `MathF`, and `HashCode`)
- `SixLabors.snk` to support strong-name signing of SixLabors assemblies
- Centralized msbuild configuration and utilities for SixLabors projects

It is designed to be installed as a [git submodule](https://blog.github.com/2016-02-01-working-with-submodules/) into Six Labors solutions.

## Installation.

This installation guide assumes that your solution conforms to the following structure: 

``` bash
solution.sln
readme.md
.gitignore
+---> src
+      +
+      +---> project
+      +   +
+      +   +---> project.csproj
+      +
+      +---> project
+          +
+          +---> project.csproj
+
+---> tests
       +
       +---> project.tests
       +   +
       +   +---> project.tests.csproj
       +
       +---> project.tests
           +
           +---> project.tests.csproj
```

If the solution does not conform to this structure you will have to update it to do so.

### Adding the Submodule

To add SixLabors.SharedInfrastructure as a submodule of your project. In the project repository type:

``` bash
git submodule add https://github.com/SixLabors/SharedInfrastructure shared-infrastructure
```

At this point, you’ll have a **shared-infrastructure** folder inside your project, but if you were to peek inside that folder, depending on your version of Git, you might see… nothing.

Newer versions of Git will do this automatically, but older versions will require you to explicitly tell Git to download the contents of **shared-infrastructure**:

``` bash
git submodule update --init --recursive
```

If everything looks good, you can commit this change and you’ll have a **shared-infrastructure** folder in your project repository with all the content from the SixLabors.SharedInfrastructure repository.

### Updating the Submodule. 

Since the submodule is stored in a separate repository you may find at times updates have been made to the linting rules that require you to update your copy. The command below will allow you to do so:

``` bash
git submodule update --init --recursive
git submodule foreach git pull origin main
```

### Linting Tools

There are three tools contained within the submodule that will help to automatically promote and enforce coding standards and consistency:
- [.gitattributes](https://git-scm.com/docs/gitattributes)
- [.editorconfig](https://docs.microsoft.com/en-us/visualstudio/ide/create-portable-custom-editor-options?view=vs-2017)
- [StyleCop Analyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)  
  
These tools are automatically installed into your solution by referencing the `.props` and `.targets` files found in the [/msbuild](/msbuild) folder.
  
### MsBuild

Within the aforementioned folder there are separate `.props` and `.targets` files designed for shared, src, and test scenarios. These files control the build process and are responsible for automatically referencing all the required projects for versioning, linting and testing.  An example use case and installation can be found at the [ImageSharp](https://github.com/SixLabors/ImageSharp) repository.

