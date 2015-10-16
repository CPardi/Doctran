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

    <xsl:function name="doctran:object-name" as="xs:string">
        <xsl:param name="object"/>
        <xsl:value-of>
            <xsl:apply-templates mode="Name" select="$object"/>
        </xsl:value-of>
    </xsl:function>

    <xsl:template mode="Name" match="*">
        <xsl:value-of select="Name"/>
    </xsl:template>

    <xsl:template mode="FileName IdName" match="*[not(ValidName)]">
        <xsl:value-of select="concat(local-name(),'-',Name)"/>
    </xsl:template>

    <xsl:template mode="FileName IdName" match="*[ValidName]">
        <xsl:value-of select="concat(local-name(),'-',ValidName)"/>
    </xsl:template>

    <xsl:function name="doctran:block-name" as="xs:string">
        <xsl:param name="object" as="element()"/>
        <xsl:value-of>
            <xsl:apply-templates mode="BlockName" select="$object"/>
        </xsl:value-of>
    </xsl:function>

    <xsl:template mode="BlockName" match="*">
        <xsl:value-of select="local-name()"/>
    </xsl:template>

</xsl:stylesheet>