#Information Comments
An information element specifies a specific property of a block. Any line of the form `!> TYPE : DESCRIPTION`, where `TYPE` is a string of alpha numeric characters, will be interpreted by Doctran as a information element. The `TYPE` specification is not case sensitive.

Any line of the form `!>> COMPONENT : DESCRIPTION` will be interpreted as a sub-component of the `TYPE` above. Again the `COMPONENT` is not case sensitive. See the author below information type below as an example.

Doctran supports the following informational elements:

  * [Author](source-comments/information-meta-data/author.md)