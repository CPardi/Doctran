<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="doctran xs xsl"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:doctran="http://www.doctran.co.uk">

    <xsl:template name="PostProcessLinks">
        <xsl:param name="prefix"/>
        <xsl:param name="article"/>

        <xsl:apply-templates mode="PostProcessLinks_Recurse" select="$article">
            <xsl:with-param name="prefix" select="$prefix"/>
        </xsl:apply-templates>

    </xsl:template>

    <xsl:template mode="PostProcessLinks_Recurse" match="a[@href] | link[@href] | script[@src] | img[@src]">
        <xsl:param name="prefix"/>

        <xsl:variable name="linkName" select="doctran:linkName(local-name())"/>
        <xsl:variable name="linkAttr" select="@*[local-name()=$linkName]"/>

        <xsl:copy>
            <!-- If its an absolute URL then leave as is. If its a relative URL then add the prefix to make it relative
            to the index.html. Change any .md extensions to .html.-->
            <xsl:attribute name="{$linkName}"
                           select="replace(if (matches($linkAttr,'^(/|\w+://)')) then $linkAttr else concat($prefix, $linkAttr), '\.md', '.html')"/>
            <xsl:apply-templates mode="PostProcessLinks_Recurse" select="@*[local-name()!=$linkName] | node()">
                <xsl:with-param name="prefix" select="$prefix"/>
            </xsl:apply-templates>
        </xsl:copy>
    </xsl:template>

    <xsl:template mode="PostProcessLinks_Recurse" match="node() | @*">
        <xsl:param name="prefix"/>

        <xsl:copy>
            <xsl:apply-templates mode="PostProcessLinks_Recurse" select="@* | node()">
                <xsl:with-param name="prefix" select="$prefix"/>
            </xsl:apply-templates>
        </xsl:copy>

    </xsl:template>

    <xsl:function name="doctran:linkName" as="xs:string">
        <xsl:param name="localName" as="xs:string"/>

        <xsl:choose>
            <xsl:when test="$localName=tokenize('a,link',',')">
                <xsl:value-of select="'href'"/>
            </xsl:when>
            <xsl:when test="$localName=tokenize('img,script',',')">
                <xsl:value-of select="'src'"/>
            </xsl:when>
        </xsl:choose>

    </xsl:function>

</xsl:stylesheet>

















