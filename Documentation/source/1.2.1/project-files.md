#Project Files
The project file is simply a plain text file that contains information about your project. You can specify a project file using the `--project_info` option

You can specify a tagline by writing it on the first line of the file. After this you can specify the content to be displayed on the project page by per-appending it with `>`. Markdown is allowed within this section. Next you can add informational elements which must start with `Type:`. The following project information types are supported:

 * [Name](project-files/name.md)
 * [Author](project-files/author.md)
 * [Markup](project-files/markup.md)
 * [ShowSource](project-files/showsource.md)
 * [Searchable](project-files/searchable.md)

An example project file is shown below

    Objects with both magnitude and direction.
    >### Description 
    >This project contains types and procedures.
    
    >### File List
    > This project contains the following Fortran 95 source files.
    
    > * Example1.f90
    > * Example2.f90
    > * Vectors.f90
    > * Vectors_1d.f90
    > * Vectors_2d.f90
    
    Name: Vectors
    
    Searchable:
    > Type: File
    > Type: Module
    > Type: Type
    > Type: Assignment
    > Type: Overload
    > Type: Operator
    > Type: Function
    > Type: Subroutine
    
    ShowSource:
    > Type: Program
    > Type: Type
    > Type: Assignment
    > Type: Overload
    > Type: Operator
    > Type: Function
    > Type: Subroutine