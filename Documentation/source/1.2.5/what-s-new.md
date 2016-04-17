# What's New

###What's new in Doctran 1.2.4

 * Errors messages are displayed for structural issues in the code.
 * The option `--save_xml=PATH` has been changed to `--save-xmls`. The appearance of this option now instructs the program to save the XMLs generated to a directory called `xml` within the output directory.
 * The option `--no-output` has been added. The appearance of this option instructs the program not to generate any documentation. If `--save-xmls` appears, XML data will still be saved.
 * The options `--project_file` has been changed to `--project-file`.
 * The font of syntax highlighted code has been changed to monospace to an issue with misaligned characters.
 * Initializations are now shown for variables.
 * Fixed bug which caused procedure arguments not to be shown in call syntax argument lists.
 * Fixed bug causing inheritance hierarchy to be display in reverse.
 * Fixed some issues with array descriptions.

###What's new in Doctran 1.2.3

 * Improved error messages.
 * Fixed bug causing block description heading sections to not be collapsible.
 * Fixed broken links in procedure argument documentation.

###What's new in Doctran 1.2.2

 * The style of the generated documentation has been modified to take advantage of wide screen and high resolution displays.
 * `Menu` project information element added, to allow menu customizations. The menu is now generated from a markdown or HTML file containing macros.
 * `UserPage` project information element added, so that additional documentation pages can be created from the user's markdown or HTML files.
 * `Source` project information element added so that source files can be specified from the project file. 
 * `Tagline` project information element added, which should not be used to specify the project's tagline.
 * A project page is no longer automatically generated from comments within the project file, instead this can be created using the `UserPage` information element.
 * Headings in meta-data have moved one level up, so the top level heading used in descriptions is now `<h2>` or `##`.
  




