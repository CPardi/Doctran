# CopyAndParse
Instructs that the content at a path should be parse and then copied to the output folder. This can be specified multiple times and takes the following form:

    CopyAndParse: PATH
    
See [Copy](project-files/types/copy.md) for more information on the copied directory structure. `PATH` can be more complex than a single filename. See [PATH](project-files/values/path.md) for more details.

Below show a table of how files of a particular extension will be parsed. Any files not matching an extension below are simply copied without any transformation.

<table>
    <thead>
        <tr>
            <td>Extension</td>
            <td>Transformation description</td>                
            <td>Output Extension</td>       
        </tr>
    </thead>

    <tbody>
        <tr>
            <td>.md, .markdown</td>
            <td>Markdown content within the file is transformed to HTML.</td>                
            <td>.html</td>       
        </tr>
        
        <tr>
            <td>.less</td>
            <td>LESS content within the file is transformed to CSS</td>                
            <td>.css</td>       
        </tr>           
    </tbody>

</table>













