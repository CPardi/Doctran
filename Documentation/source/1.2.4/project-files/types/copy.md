# Copy
Instructs that the content at a path should be copied to the output folder. This can be specified multiple times and takes the following form:

    Copy: PATH
    
The value of `PATH` is the path of the data to be copied, relative to the project file. The data found will then be copied to the output directory, with the relative path maintained.

For example, given the following directory structure:

    - Root
        - project.info
        - File1.md
        - Directory1
            - SubFile1.md

and within the project.info file there is the command

    Copy: Directory1/SubFile.md
    
Then the output path will have the directory structure

    - OutputDir
        - Directory1
            - SubFile1.md
        + GeneratedContent 

`PATH` can be more complex than a single filename. See [PATH](project-files/values/path.md) for more details.













