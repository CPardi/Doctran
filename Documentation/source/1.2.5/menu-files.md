#Menu Files
A menu file is just a HTML (.html) or markdown (.md or .markdown) file containing a list of links you want to appear in the documentation menu. Items can either be simple links or special macros that will insert lists of links generated from your Fortran source code. Files should should only contain the list in your chosen markup and not any boilerplate tags such as `<html>` or `<body>`.

##Macros
Macros can be used within your menu files to automatically generate menu links to your source code pages. The following macros are supported:

 * `|Name|` - The name of the current block.
 * `|BlockName|` - The type name of the current block.
 * `|List, TYPE_VALUE|` - Lists all items of `TYPE`.
 * `|List, TYPE_VALUE, Recursive|` - Lists all items of `TYPE`, as well as their sub-blocks.

where `TYPE_VALUE` can be any of the following:

 * `Project` - The current project.
 * `File` - Fortran source files.
 * `Program` - Fortran programs.
 * `Module` - Fortran modules.
 * `DerivedType` - Fortran user-defined derived-types.
 * `Function` - External and module functions.
 * `Subroutine` - External and module subroutines.
 * `Assignment` - Generic assignment interfaces.
 * `Overload` - Generic name interfaces.
 * `Operator` - Generic operator interfaces.
 * `Variable` - Module variables.

The previous `TYPE_VALUE` values adds menu items that do not change from page to page. The next `TYPE_VALUE` values add menu item that are dependant on the user's current page.

 * `SubBlocks` -  Current block's sub-blocks.
 * `SubBlocksAndSelf` - Current block as well as it's sub-blocks.
 * `SameType` -Blocks of the same type as the current block.

##Example
An example markdown menu file is shown below.

        * [Welcome](index.md)
        * [File List](filelist.md) |List, File|
        * [Module API](modulelist.md) |List,Module,Recursive|

The above menu file will produce

 * A link to an index [user page](project-files/types/userpage.md).
 * A "File List" heading linked to a [user page](project-files/types/userpage.md), followed by a generated list of links to source file documentation. 
 * A "Module API" heading linked to a [user page](project-files/types/userpage.md), followed by a generated list of links to module documentation. 