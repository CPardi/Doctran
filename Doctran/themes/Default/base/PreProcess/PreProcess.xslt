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

    <xsl:template mode="Preprocess" match="Project">
        <xsl:call-template name="AddNavigation">
            <xsl:with-param name="project" select="."/>
        </xsl:call-template>
    </xsl:template>

    <xsl:template name="AddNavigation">
        <xsl:param name="project"/>
        <xsl:apply-templates mode="AddNavigation" select="$project"/>
    </xsl:template>

    <xsl:template mode="AddNavigation" match="UserPage">
        <xsl:copy>
            <Prefix>
                <xsl:value-of select="doctran:prefix-from-path(Path)"/>
            </Prefix>
            <href>
                <xsl:value-of select="Path"/>
            </href>
            <xsl:apply-templates mode="AddNavigation" select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template mode="AddNavigation" match="Project">
        <xsl:copy>
            <Prefix>
            </Prefix>
            <href>
                <xsl:value-of select="'index.html'"/>
            </href>
            <xsl:apply-templates mode="AddNavigation" select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template mode="AddNavigation" match="File">
        <xsl:copy>
            <xsl:call-template name="addNavigationElements"/>
            <xsl:apply-templates mode="AddNavigation" select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template mode="AddNavigation" match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates mode="AddNavigation" select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template name="addNavigationElements">
        <Prefix>
            <xsl:apply-templates mode="PathToRoot" select="."/>
        </Prefix>
        <href>
            <xsl:apply-templates mode="GenerateURL" select="."/>
        </href>
    </xsl:template>

    <xsl:function name="doctran:prefix-from-path" as="xs:string">
        <xsl:param name="path" as="xs:string"/>

        <xsl:value-of>
            <xsl:text></xsl:text>
            <xsl:for-each select="tokenize($path, '[\\ /]')[position() > 1]">
                <xsl:text>../</xsl:text>
            </xsl:for-each>
        </xsl:value-of>

    </xsl:function>

</xsl:stylesheet>