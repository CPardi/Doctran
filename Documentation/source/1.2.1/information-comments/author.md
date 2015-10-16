#Author
An author information element is used to specify the author who wrote that block of code. It may be placed within any block of code and the information will be shown on that block's documentation page. The unique author names within all blocks are collect and also shown on the project's index page.

##Sub-Information
The author information comment supports the following sub-information:

 * Name - The author's name.
 * Email - The author's email, used to create a mailto link.
 * Affiliation - The institution or company the author belongs to.

Below shows an example of an author information element for a subroutine.

    subroutine equals(lhs, rhs)
    !> Author:
    !>> Name: Bob
    !>> Email: bob@example.com
    !>> Affiliation: A Different Fortran Software Company
        
        type(Vector_1d),intent(inout) :: lhs
        type(Vector_1d),intent(in) :: rhs
                
        lhs%x_1 = rhs%x_2
        
    end subroutine