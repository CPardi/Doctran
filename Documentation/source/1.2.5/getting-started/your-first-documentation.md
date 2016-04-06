# Your First Documentation
Doctran can only be run from the command line. The most basic syntax for running Doctran is 

    doctran [source_files]

where source_files is a space separated list of paths to your source files. This will output your documentation in a newly created Docs folder within your current directory.

Commonly, you would want your documentation to be placed within a new folder. To do this use the `--output_dir` option or its shorter `-o` option as shown below.

    doctran -o <DIR> [source_files]

The above will output documentation in the `<DIR>` directory, creating the path if required. The most general syntax for running Doctran is shown below.

    doctran [options] [source_files]

A full list of options can be found [here](command-line-options.md)