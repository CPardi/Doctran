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

    <!--Description contains headings, write the heading and then a content div.-->
    <xsl:template mode="SectionFormatter" match="node()">
        <xsl:param name="prefix"/>

        <!-- Form groups that start from a h2 tag and end at the tag prior to the next h2.-->
        <xsl:for-each-group select="node()" group-starting-with="h2">
            <xsl:apply-templates mode="SectionFormatter_headings" select=".">
                <xsl:with-param name="currentGroup" select="current-group()"/>
                <xsl:with-param name="prefix" select="$prefix"/>
            </xsl:apply-templates>
        </xsl:for-each-group>

    </xsl:template>

    <xsl:template mode="SectionFormatter_headings" match="node()">
        <xsl:param name="prefix"/>

        <xsl:copy-of select="current-grouping-key()"/>
        <xsl:apply-templates mode="SectionFormatter_ingroup" select="current-group()">
            <xsl:with-param name="prefix" select="$prefix"/>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template mode="SectionFormatter_headings" match="h2">
        <xsl:param name="currentGroup"/>
        <xsl:param name="prefix"/>

        <div class="section">
            <!--Copy the group heading-->
            <xsl:copy-of select="."/>
            <div class="content">
                <!-- Copy the content with the heading removed.-->
                <xsl:apply-templates mode="SectionFormatter_ingroup" select="$currentGroup[position() > 1]">
                    <xsl:with-param name="prefix" select="$prefix"/>
                </xsl:apply-templates>
            </div>
        </div>
    </xsl:template>

    <xsl:template mode="SectionFormatter_ingroup" match="@*| node()">
        <xsl:param name="prefix"/>

        <xsl:copy>
            <xsl:apply-templates mode="SectionFormatter_ingroup" select="@*| node()">
                <xsl:with-param name="prefix" select="$prefix"/>
            </xsl:apply-templates>
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>