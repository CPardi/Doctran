# Generating a Static Wiki
Doctran can be used as a static wiki generator. Meaning that given a collection of Markdown or HTML files, Doctran will transform these into styled HTML documents with a title, menu and search. Of course, the following can be combined with document generator capabilities to produce quality documentation.

Assuming for simplicity you have a single markdown file, say `index.md`, then firstly you need to create a [menu file](menu-files.md). Save the following text as `menu.md` in the same directory as your index.md

     * [Welcome](index.html)

This will produce a menu with one item named "Welcome" linking to `index.md`.

Next you need to create a [project file](project-files.md) to include your content and menu. Create another file called `project.info`, again within the same folder, that contains the following:

    Name: My Static Wiki
    Tagline: My Wiki Tagline
    Menu: menu.md
    UserPage: index.md
    
This will set a project name and tagline and include you menu and content files.

Finally, run Doctran in the folder that these files are saved within, using the following:

    doctran --project_info project.info -o StaticWiki
    
This will generate your static wiki within a `StaticWiki` folder.