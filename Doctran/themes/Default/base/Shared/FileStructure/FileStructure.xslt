<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="xsl xs doctran"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:doctran="http://www.doctran.co.uk">

    <xsl:function name="doctran:object-anchor-link" as="item()*">
        <xsl:param name="object" as="element()" />
        <xsl:apply-templates mode="ObjectAnchorLink" select="$object" />
    </xsl:function>

    <xsl:function name="doctran:anchor-link" as="element()">
        <xsl:param name="uri" as="xs:string"/>
        <xsl:param name="content" as="item()*"/>
        <a href="{$uri}">
            <xsl:sequence select="$content"/>
        </a>
    </xsl:function>

    <!--Convert a general uri to a form normalized for use with this stylesheet.  -->
    <xsl:function name="doctran:normalize-uri" as="xs:string">
        <!--The path to normalize-->
        <xsl:param name="path" as="xs:string"/>

        <xsl:variable name="toForwardSlash" select="replace($path, '\\', '/')" as="xs:string"/>

        <xsl:value-of select="if(starts-with($toForwardSlash, './'))then
                                    substring-after($toForwardSlash, './')
                                else if(starts-with($toForwardSlash, '.\'))then
                                    substring-after($toForwardSlash, '.\')
                                else $toForwardSlash"/>

    </xsl:function>

    <xsl:function name="doctran:object-by-guid" as="element()*">
        <xsl:param name="project" as="element()"/>
        <xsl:param name="guid" as="xs:string+"/>

        <xsl:if test="$project and $guid!=''">
            <xsl:sequence select="$project/key('object-by-guid', $guid)"/>
        </xsl:if>

    </xsl:function>

    <!--Returns a relative uri for the documentation page describing an object.-->
    <xsl:function name="doctran:object-uri" as="xs:string">
        <!--An object that has a corresponding documentation page. -->
        <xsl:param name="object" as="element()"/>

        <xsl:value-of>
            <xsl:apply-templates mode="ObjectUri" select="$object">
                <xsl:with-param name="slash" select="'/'"/>
                <xsl:with-param name="appendExtension" select="true()"/>
            </xsl:apply-templates>
        </xsl:value-of>

    </xsl:function>

    <!--If $item is a node, then the relative uri between an object's documentation page
    and the documentation's root directory is returned. If $item is a text node, then the
    root path is inferred from the number of slashes. -->
    <xsl:function name="doctran:root-uri" as="xs:string">
        <!--An object that has a corresponding documentation page, or a text node. -->
        <xsl:param name="item" as="item()"/>
        <xsl:value-of>
            <xsl:apply-templates mode="RootUri" select="$item"/>
        </xsl:value-of>
    </xsl:function>

    <xsl:key name="object-by-guid" use="@guid" match="/Project//.[@guid]"/>

    <xsl:template mode="ObjectAnchorLink" match="*" as="item()*">
        <xsl:sequence select="doctran:anchor-link(doctran:object-uri(.), Name)" />
    </xsl:template>

    <xsl:template mode="ObjectUri" match="Project" as="xs:string">
        <xsl:text>html</xsl:text>
    </xsl:template>

    <xsl:template mode="ObjectUri" match="UserPage" as="xs:string">
        <xsl:value-of select="doctran:normalize-uri(Path)"/>
    </xsl:template>

    <xsl:template mode="ObjectUri" match="*" as="xs:string">
        <xsl:param name="slash" as="xs:string"/>
        <xsl:param name="appendExtension" as="xs:boolean" select="false()"/>

        <xsl:value-of>
            <xsl:apply-templates mode="ObjectUri" select="../..">
                <xsl:with-param name="slash" select="$slash"/>
            </xsl:apply-templates>

            <xsl:value-of select="$slash"/>
            <xsl:apply-templates mode="FileName" select="."/>
            <xsl:if test="$appendExtension">
                <xsl:text>.html</xsl:text>
            </xsl:if>
        </xsl:value-of>
    </xsl:template>

    <xsl:template mode="RootUri" match="Project" as="xs:string?"/>

    <xsl:template mode="RootUri" match="UserPage" as="xs:string">
        <xsl:value-of select="doctran:root-uri(Path/text())"/>
    </xsl:template>

    <xsl:template mode="RootUri" match="*" as="xs:string">
        <xsl:value-of>
            <xsl:apply-templates mode="RootUri" select="../.."/>
            <xsl:text>../</xsl:text>
        </xsl:value-of>
    </xsl:template>

    <xsl:template mode="RootUri" match="text()" as="xs:string">

        <xsl:value-of>
            <xsl:text></xsl:text>
            <xsl:for-each select="tokenize(doctran:normalize-uri(.), '[\\ /]')[position() > 1]">
                <xsl:text>../</xsl:text>
            </xsl:for-each>
        </xsl:value-of>

    </xsl:template>

</xsl:stylesheet>














