# Descriptive Comments
A description can be placed anywhere within a block to describe that block's purpose. In the case of declarations of variables-like objects, a description must be placed on the line directly beneath the declaration line. A variable-like object refers to variables, arguments and components. A description made up of two parts, a basic and a detail part.

### Basic Part
The basic part of a description being with `!>` and is used to give a brief outline of what the block is used for. This description is used as the subtitle on the block's documentation page, as well as the description displayed within lists. Note, a variable-like object can only contain a basic description part.

### Detail Part
Doctran will treat any line beginning with `!>>` as a detailed description component. The detailed part of a description is only seen within the blocks documentation page. Detailed descriptions can contain HTML or markdown. Note that section headings are given by `<h2>` tags in html or the `##` in markdown as the level one heading is used for the object's name and type. Doctran will automatically make the content between each level two heading foldable.

If required, a description maybe split onto multiple lines by starting the next line with `!>` or `!>>` for basic and detailed components respectively.

### Example
An example of some valid descriptions are shown below.

    type Vector_1d
        !> A basic description.
        !> Another line may be included and must be directly below the previous line.
        !>> ## Markdown parsed heading
        !>> A detailed description.
        !>> The same applies for a multi-line detailed descriptions.
        
        real x_1
        !> A basic description of a variable. This must be directly below the declaration.
    end type

## Named Descriptions
Named descriptions allow the description of variable-like objects, where several of them are declared on a single line. Any line with the form `!> NAME - DESCRIPTION`, where `NAME` is a string of alpha numeric characters, will be interpreted by Doctran as a named description. As with all Fortran names the `NAME` specified is not case sensitive.

### Example
An example of some valid named descriptions are shown below.

    type Vector_2d
        real x_1, x_2
        !> x_1 - A real variable holding the value of the vector in the x direction.
        !> x_2 - A real variable holding the value of the vector in the y direction.
    end type

## Alternative Commenting
Named descriptions allow for more flexibility when specifying your descriptions. Doctran will search within a block and also its parent block for names matching the description's name. This allows for a different style of commenting to be used as shown in the example below.

    !> Vector_2d - A type containing two real components, representing the two degrees of freedom.
    !>> Mathematically this type represents the following:
    !>> \[ \vec{x} = \left(\begin{array}{c} x_1 \\ x_2 \end{array} \right) \]
    type Vector_2d
        !> x_1 - A real variable holding the value of the vector in the x direction.
        !> x_2 - A real variable holding the value of the vector in the y direction.
        real x_1, x_2
    end type