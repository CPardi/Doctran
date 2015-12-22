<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema">

    <!-- Applies all the templates within the project. -->
    <xsl:import href="base/include.xslt"/>
    <xsl:import href="f95/include.xslt"/>
    <!--<xsl:import href="f03/include.xslt"/>-->
    <!--<xsl:import href="f03/RunAll.xslt"/>-->

    <xsl:param name="verbose" as="xs:integer"/>
    <xsl:param name="workingDirectory" as="xs:string"/>
    <xsl:param name="source">
        <Source/>
    </xsl:param>

    <xsl:output method="html" indent="no" doctype-system="about:legacy-compat" encoding="utf-8"/>

    <xsl:template match="/">
        <xsl:apply-templates mode="RunAll" select="/Project"/>
    </xsl:template>

</xsl:stylesheet>