#Command Line Options
Doctran can be supplied with the command-line options shown below.

##Help
These options force Doctran to display various information meant to be helpful to the user.

<table>
<thead>
<tr><td>Option name</td><td>Description</td></tr>
</thead>
<tbody>
<tr><td>--help</td><td>Displays usage information.</td></tr>

<tr><td>--license</td><td>Run Doctran with this options to enter a license key and view license information.</td></tr>

<tr><td>--plugins</td><td>This option forces Doctran to check the plugins path for any plugins present and write their names and version. Doctran will exit immediately after plugins have been checked.</td></tr>

<tr><td>--verbose VALUE</td><td>
Sets how much information should written to the screen. VALUE can be the following:
 <ul>
     <li> 1 - No output apart from fatal errors.</li>
     <li> 2 - (Default) Basic information output. Statement of the current operation, warning messages and fatal errors are written to the screen.</li>
     <li> 3 - Detailed information output. Everything from 2 with a lot of information on what is processing.</li>
 </ul>
</td></tr>

</tbody>
</table>

##I/O
These options allow the user to affect the input and output.

<table>
<thead>
<tr><td>Option name</td><td>Description</td></tr>
</thead>
<tbody>

<tr><td> --output, -o DIR</td><td>DIR is the output directory for the documentation. (Default: Docs).</td></tr>

<tr><td>--overwrite</td><td>If this option is specified, then the auxiliary files (css, scripts, etc.) and html files in the output directory will be overwritten. If not, then just the html files will be overwritten, which is the default behaviour.</td></tr>

<tr><td> --save_xml FILE</td><td>Specifies a path to save the intermediary XML document generated. The document is not saved by default.</td></tr>

</tbody>
</table>

##Project
These options allow the user to specify information relating to their project.

<table>
<thead>
<tr>
<td>Option name</td><td>Description</td>
</tr>
</thead>
<tbody>
<tr>
<td>--project_info FILE,</td><td>FILE is the name and/or path of the project's information file.</td>
</tr>
</tbody>
</table>   