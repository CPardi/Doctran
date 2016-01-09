#Changelog

##Version 1.2.4 (Latest)
 * The option `--save_xml=PATH` has been changed to `--save_xmls`. The appearance of this option now instructs Doctran to save the XMLs generated to a directory called `xml` within the output directory.

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