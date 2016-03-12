#PATH
In general a `PATH` value can take the form:

    pathToDirectory[/**][/searchPattern]

The `searchPattern` within the `PATH` can include the wildcard characters `*`, `?`. Their uses are described below:

<table>
    <thead>
        <tr><td>Wildcard specifier</td><td>Description</td></tr>    
    </thead>
    <tbody>
        <tr>
            <td>* (asterisk)</td>
            <td>Matches zero or more characters in that position.</td>            
        </tr>    
        <tr>
            <td>? (question mark)</td>
            <td>Matches zero or one character in that position.</td>            
        </tr>    
    </tbody>
</table>

The path can optionally include `**` to specify that the preceding directory and all its sub-directories are to be searched for files matching the `searchPattern`.

##Examples
For the follow examples assume you have the directory structure:

    - Root
        - File1.txt
        - File2.md
        - Directory1
            - SubFile1.txt
            - SubFile2.md

Then, for the following `PATH` values:

 - `Root/File1.txt`, matches 
     - Root/File1.txt
 - `Root/*.txt`, matches 
     - Root/File1.txt
 - `Root/**/*`, matches
     - Root/File1.txt
     - Root/File2.md
     - Root/Directory1/SubFile1.txt
     - Root/Directory1/SubFile2.md
 - `Root/**/*File?.txt`, matches
     - Root/File1.txt
     - Root/Directory1/SubFile1.txt
