## Doctran Documentation Start Page

![](images/logo.png "Optional title")
This is the default page that is shown for projects that have not yet specified a start page. It is included to give some information on how to override this page with something more relevant to your project.

### Specifying Your Own Start Page

In order to specify your own start page you first have to create a project file. A project file is simply a plain text file that contains information about your project. Within this file put the following:

    Userpage: index.html

Then, place your index.html file in the same directory as your project file and re-run Doctran using the following:

	doctran --project_info <Project File Name> <Any Source Files>

This start page will then be overridden by the content in your file. Note you can also use the .md or .markdown extension to specify files containing markdown content.

### More Information

Other information that you can include in your project file is

 * Your project's name and tag-line.
 * A custom menu.
 * Author information. 

You can get more information on what can go in a project file by checking out the [User Guide](http://www.doctran.co.uk/doc).