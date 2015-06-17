<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                              xmlns:xs="http://www.w3.org/2001/XMLSchema">

	<xsl:param name="verbose" as="xs:integer" />
    <xsl:param name="workingDirectory" as="xs:string"/>
	<xsl:param name="source">
		<Source/>
	</xsl:param>

	<xsl:include href="f03/main.xslt" />

</xsl:stylesheet>