# Project Files
The project file is simply a plain text file that contains information about your project. You can specify a project file using the `--project_info` option.

A project file contains informational elements which have the form `TYPE: VALUE`. The following values for `TYPE` are supported:

  * [Author](project-files/types/author.md)
  * [Copy](project-files/types/copy.md)
  * [CopyAndParse](project-files/types/copyandparse.md)
  * [Menu](project-files/types/menu.md)
  * [Name](project-files/types/name.md)
  * [Searchable](project-files/types/searchable.md)
  * [ShowSource](project-files/types/showsource.md)
  * [Source](project-files/types/source.md)
  * [Tagline](project-files/types/name.md)
  * [UserPage](project-files/types/userpage.md)

The following possible values of `VALUE` are supported:

  * [PATH](project-files/values/path.md)

## Example
An example project file is shown below

    Name: Timers
    Tagline: Making timing easy.
    
    Menu: menu.md
    Markup: markdown
    
    UserPage: index.md
    UserPage: filelist.md
    UserPage: modulelist.md
    
    Source: ../src/*.f90
    
    ShowSource:
    > Type: File
    > Type: Program
    
    Searchable:
    > Type: Program
    > Type: DerivedType
    > Type: Assignment
    > Type: Overload
    > Type: Operator
    > Type: Function
    > Type: Subroutine