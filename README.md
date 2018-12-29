<h1 align="center">

<img src="https://raw.githubusercontent.com/SixLabors/Branding/master/icons/org/sixlabors.512.png" alt="SixLabors.Standards" width="256"/>
<br/>
SixLabors.Standards
</h1>

This repository contains the configuration and guidelines for automated linting of C# projects. It is designed to be installed as a [git submodule](https://blog.github.com/2016-02-01-working-with-submodules/) into your solution.

## Installation.

This installation guide assumes that your solution conforms to the following structure: 

``` txt
solution.sln
readme.md
.gitattributes
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

To add SixLabors.Standards as a submodule of your project. In the project repository type:

``` bash
git submodule add https://github.com/SixLabors/Standards standards
```

At this point, you’ll have a **standards** folder inside your project, but if you were to peek inside that folder, depending on your version of Git, you might see… nothing.

Newer versions of Git will do this automatically, but older versions will require you to explicitly tell Git to download the contents of rock:

``` bash
git submodule update --init --recursive
```

If everything looks good, you can commit this change and you’ll have a **standards** folder in your project repository with all the content from the SixLabors.Standards repository.

### Updating the Submodule. 

Since the submodule is stored in a separate repository you may find at times updates have been made to the linting rules that require you to update your copy. The same command as above will allow you to do so:

``` bash
git submodule update --init --recursive
```

### Wiring up the Linting Tools

There are two tools contained within the submodule that will help to automatically promote and enforce coding standards and consistancy:

- [EditorConfig](https://docs.microsoft.com/en-us/visualstudio/ide/create-portable-custom-editor-options?view=vs-2017)
- [StyleCop Analyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)

#### Editor Config 

Settings in EditorConfig files enable you to maintain consistent coding styles and settings in a codebase, such as indent style, tab width, end of line characters, encoding, and more, regardless of the editor or IDE you use. For example, when coding in C#, if your codebase has a convention to prefer that indents always consist of five space characters, documents use UTF-8 encoding, and each line always ends with a CR/LF, you can configure an .editorconfig file to do that.

Adding an EditorConfig file to your project or codebase does not convert existing styles to the new ones. For example, if you have indents in your file that are formatted with tabs, and you add an EditorConfig file that indents with spaces, the indent characters are not automatically converted to spaces. However, any new lines of code are formatted according to the EditorConfig file. Additionally, if you format the document (**Edit > Advanced > Format Document** or <kbd>Ctrl+K, Ctrl+D</kbd>), the settings in the EditorConfig file are applied to existing lines of code.

To add an EditorConfig file to your solution open the solution in Visual Studio. Select the solution node and right-click.

From the menu bar, choose **Project > Add Existing Item**, or press <kbd>Shift+Alt+A</kbd>.

The Add Existing Item dialog box opens. Use this to navigate to your new **standards** folder and select the .editorconfig file.

An .editorconfig file appears in Solution Explorer, and it opens in the editor.

![EditorConfig file in solution explorer.](images/editorconfig-in-solution-explorer.png)

#### StyleCop Analyzers

StyleCop Analyzers are Roslyn Analyzer that contain an implementation of the StyleCop rules using the .NET Compiler Platform. Where possible, code fixes are also provided to simplify the process of correcting violations.

StyleCopAnalyzers can be installed using the NuGet command line or the NuGet Package Manager in Visual Studio 2017.

Install using the command line:

``` bash
Install-Package StyleCop.Analyzers
```

Installing via the package manager:

![stylecop analyzers-via-nuget](images/stylecop-analyzers-via-nuget.png)

Once the Nuget package is installed you will should add the following configuration files to the **Solution Items** folder you created when installing the .editorconfig file so you can easily view the contents. 

- `SixLabors.ruleset`
- `stylecop.json`

These files tell StyleCop what rules to enforce and will have to be manually added to each project. **right-click > Edit [YOUR_PROJECT_NAME].csproj**

``` xml
<PropertyGroup>
  <CodeAnalysisRuleSet>..\..\standards\SixLabors.ruleset</CodeAnalysisRuleSet>
</PropertyGroup>

<ItemGroup>
  <AdditionalFiles Include="..\..\standards\stylecop.json" />
</ItemGroup>
```

An up-to-date list of which StyleCop rules are implemented and which have code fixes can be found [here](https://dotnetanalyzers.github.io/StyleCopAnalyzers/).