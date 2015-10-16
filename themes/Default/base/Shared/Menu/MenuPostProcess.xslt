<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="MenuPostProcess">
        <xsl:param name="menu"/>
        <xsl:param name="prefix"/>

        <xsl:apply-templates mode="MenuPostProcess" select="$menu">
            <xsl:with-param name="prefix" select="$prefix"/>
            <xsl:with-param name="href" select="href"/>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template mode="MenuPostProcess" match="a">
        <xsl:param name="prefix"/>
        <xsl:param name="href"/>

        <xsl:copy>
            <!-- If the current page path is the same as this link, then add the "active" class. -->
            <xsl:attribute name="class" select="if (@href=$href) then 'active' else ''"/>

            <!-- Add the prefix to make link relative to the index page. -->
            <xsl:attribute name="href" select="concat($prefix, @href)"/>

            <!-- And recurse... -->
            <xsl:apply-templates mode="MenuPostProcess" select="@*[local-name()!='href'] | node()">
                <xsl:with-param name="prefix" select="$prefix"/>
                <xsl:with-param name="href" select="$href"/>
            </xsl:apply-templates>
        </xsl:copy>

    </xsl:template>

    <xsl:template mode="MenuPostProcess" match="ul[not(li)]">
    </xsl:template>

    <xsl:template mode="MenuPostProcess" match="ul[parent::ul]">
        <xsl:param name="prefix"/>
        <xsl:param name="href"/>

        <xsl:apply-templates mode="MenuPostProcess" select="@* | node()">
            <xsl:with-param name="prefix" select="$prefix"/>
            <xsl:with-param name="href" select="$href"/>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template mode="MenuPostProcess" match="li[not(span[@class='subtitle']) and not(a) and not(ul[li])]">
    </xsl:template>

    <xsl:template mode="MenuPostProcess" match="@* | node()">
        <xsl:param name="prefix"/>
        <xsl:param name="href"/>

        <xsl:copy>
            <xsl:apply-templates mode="MenuPostProcess" select="@* | node()">
                <xsl:with-param name="prefix" select="$prefix"/>
                <xsl:with-param name="href" select="$href"/>
            </xsl:apply-templates>
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>