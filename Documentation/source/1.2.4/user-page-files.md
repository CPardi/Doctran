# User Page Files
A user page file can be either be a html (.html) or markdown (.md or .markdown) file. This file should contain the content you want to be displayed in your chosen markup. To include a user page file within your project use the [UserPage](project-files/types/userpage.md) element in your [project file](project-files.md).

The first heading is used as the title for the user page and all relative links such as links, images, CSS and script are assumed to be relative to the project file. Files should not contain any boilerplate tags such as `<html>` or `<body>` only your content.

To allow more flexibility, menu entries are not automatically generated. Instead, menu entries are added manually to the [menu file](menu-files.md).

## Macros
Macros can be used within your user page files to include information about your source code. The following macros are supported:

 * `|Table, TYPE_VALUE|` - When placed within `<table>` tags, the table will be populated with item of `TYPE_VALUE`.

where `TYPE_VALUE` can be any of the following:

 * `Project` - The current project.
 * `File` - Fortran source files.
 * `Module` - Fortran modules.
 * `DerivedType` - Fortran user-defined derived-types.
 * `Function` - External and module functions.
 * `Subroutine` - External and module subroutines.
 * `Assignment` - Generic assignment interfaces.
 * `Overload` - Generic name interfaces.
 * `Operator` - Generic operator interfaces.
 * `Variable` - Module variables.

## Example
An example user page file is shown below. It uses markup syntax and a macro to show a list of files linking to their documentation pages.

    #Welcome

    ## Description 
    This project contains types and procedures.
    
    ## File List
    This project contains the following Fortran 95 source files.
    
    <table>
    |table,file|
    </table>