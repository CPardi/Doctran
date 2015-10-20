<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
-->
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <!-- Applies all the templates within the project. -->
    <xsl:import href="base/PreProcess/include.xslt"/>
    <xsl:import href="f95/PreProcess/include.xslt"/>
    <xsl:import href="f03/PreProcess/include.xslt"/>

    <xsl:output method="xml" indent="no" encoding="utf-8"/>

    <xsl:template match="/">
        <xsl:apply-templates mode="Preprocess" select="Project"/>
    </xsl:template>

</xsl:stylesheet>
