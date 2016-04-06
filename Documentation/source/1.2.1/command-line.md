# Command Line Options
Doctran can be supplied with the command-line options shown below.

## Help
These options force Doctran to display various information meant to be helpful to the user.

<table>
<thead>
<tr><td>Option name</td><td>Description</td></tr>
</thead>
<tbody>
<tr><td>plugins</td><td>This option forces Doctran to check the plugins folder with its installation path for any shared libraries present and write their names and version. Doctran will exit immediately after plugins have been checked.</td></tr>
<tr><td>verbose VALUE</td><td>
Sets how much information should written to the screen. VALUE can be the following:
 
 * 1 - No output apart from fatal errors.
 * 2 - (Default) Basic information output. Statement of the current operation, warning messages and fatal errors are written to the screen.
 * 3 - Detailed information output. Everything from 2 with a lot of information on what is processing.
</td></tr>
<tr><td>help, -h</td><td>Displays usage information.</td></tr>
</tbody>
</table>

##I/O
These options allow the user to affect the output Doctran produces.

<table>
<thead>
<tr><td>Option name</td><td>Description</td></tr>
</thead>
<tbody>
<tr>
<td> save_xml FILE</td><td>Tells Doctran to save the intermediary xml generated during the documentation's generation. FILE is the name and/or path for the xml file.</td></tr>
<tr>
<td> output_dir, -o DIR</td><td>DIR is the output directory for the documentation. If this is not specified then Doctran output the documentation in the current directory.</td></tr>
<tr>
<td> extensions EXT,</td><td>EXT should be a comma separated list of any additional extensions your source files may have. (Default: f90, f95, f03, f08)</td>
</tr>
</tbody>
</table>

## Project
These options allow the user to specify information relating to their project.

<table>
<thead>
<tr>
<td>Option name</td><td>Description</td>
</tr>
</thead>
<tbody>
<tr>
<td> project_info FILE,</td><td>FILE is the name and/or path of the project's information file.</td>
</tr>
</tbody>
</table>
    
## Themes
These options relate to the customization of the documentation's look and feel.

<table>
<thead>
<tr><td>Option name</td><td>Description</td></tr>
</thead>
<tbody>
<tr><td> theme, -t NAME,</td><td>NAME is the name of the theme to be applied. (Default: Default)</td></tr>
<tr><td> color_scheme, -c NAME</td><td>NAME is the name of the color scheme to be used. (Default: Default)</td></tr>
<tr><td> overwrite true/false</td><td>If true then the auxiliary files (css, scripts, etc.) and html files in the output directory will be overwritten. If false then just the html files will be overwritten. (Default: false)</td></tr>
</tbody>
</table>