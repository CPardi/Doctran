#Author
An author information element is used to specify the person who wrote a block of code. It may be placed within any block of code and the information will be shown on that block's documentation page.

##Sub-Information
The author information comment supports the following sub-information:

<table>
<thead>
<tr>
<td>Name</td><td>Maximum allowed</td><td>Description</td>
</tr>
</thead>
<tbody>
<tr>
<td>Name</td><td>1</td><td>The author's name.</td>
</tr>
<tr>
<td>Email</td><td>1</td><td>The author's email will be displayed as a mailto link.</td>
</tr>
<tr>
<td>Affiliation</td><td>1</td><td> The author's institution or work place.</td>
</tr>
</tbody>
</table>

Below shows an example of an author information element for a subroutine.

    subroutine equals(lhs, rhs)
    !> Author:
    !>> Name: Bob
    !>> Email: bob@example.com
    !>> Affiliation: A Fortran Software Company
        
        type(Vector_1d),intent(inout) :: lhs
        type(Vector_1d),intent(in) :: rhs
                
        lhs%x_1 = rhs%x_2
        
    end subroutine