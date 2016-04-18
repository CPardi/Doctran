#Changelog

##Version 1.2.5 (Latest)
 * Updated syntax highlighter to be more accurate, aware of more language features and copy-and-paste friendly.
 * The option `--time-run` has been added to specify that performance timings should be saved within an XML file in the xml directory of the output directory.
 * Stopped the output of unneeded files when creating static wiki documentation (documentation containing no Fortran source files).
 * In tables and menu lists one can use `Program` as a `TYPE_VALUE` to output a table or list of fortran programs.
 * A button to toggle the visibility of the menu has been added to documentation generated.

##Version 1.2.4
 * Errors messages are displayed for structural issues in the code.
 * The option `--save_xml=PATH` has been changed to `--save-xmls`. The appearance of this option now instructs the program to save the XMLs generated to a directory called `xml` within the output directory.
 * The option `--no-output` has been added. The appearance of this option instructs the program not to generate any documentation. If `--save-xmls` appears, XML data will still be saved.
 * The options `--project_file` has been changed to `--project-file`.
 * The font of syntax highlighted code has been changed to monospace to an issue with misaligned characters.
 * Initializations are now shown for variables.
 * Fixed bug which caused procedure arguments not to be shown in call syntax argument lists.
 * Fixed bug causing inheritance hierarchy to be display in reverse.
 * Fixed some issues with array descriptions.

##Version 1.2.3
 * Improved error messages.
 * Fixed bug causing block description heading sections to not be collapsible.
 * Fixed broken links in procedure argument documentation.

##Version 1.2.2
 * The style of the generated documentation has been modified to take advantage of wide screen and high resolution displays.
 * `Menu` project information added, to allow menu customizations. The menu is now generated from a markdown or HTML file containing macros.
 * `UserPage` project information added, so that additional documentation pages can be created from the user's markdown or HTML files.
 * `Source` project information added so that source files can be specified from the project file.
 * A project page is no longer automatically generated from comments within the project file, instead this can be created using the `UserPage` information.
 * `Tagline` project information added, which should not be used to specify the project's tagline.
 * Color schemes removed.
 * `--color_scheme` command-line argument removed.
 * `--theme` command line argument removed.
 * Doctran will now run without any Fortran source files specified, to allow for use as a static wiki generator.

## Version 1.2.1
 * The block searching algorithm has been improved.
 * The "end" statement to terminate functions and subroutines within interfaces is now reconsigned.
 * Broken links in the 'Type' column of argument lists within the documentation pages of assignments and operators fixed.

## Version 1.2
 * Support for the object oriented constructs from the Fortran 2003 standard added.

## Version 1.1
 * Markdown supported within descriptions added.
 * Syntax highlighting of source code added.
 * A Javascript search added.
 * Support for different color schemes.

## Version 1.0
  * Initial Release 