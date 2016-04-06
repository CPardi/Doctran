# Searchable
The type of code block that should be indexed and viewable within the documentation search results. This should be specified only once and take the following form:

    Searchable:
    > Type: Type_Value

`Type` may be specified multiple times and `Type_Value` can take any of the following values:

  * File
  * Program
  * Module
  * DerivedType
  * Assignment
  * Overload
  * Operator
  * Function 
  * Subroutine

By default all of the above is made searchable.