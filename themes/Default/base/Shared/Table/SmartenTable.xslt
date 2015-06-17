<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="SmartenTable">
        <xsl:param name="table" as="element()"/>
        <xsl:param name="sortColumnNum"/>

        <xsl:apply-templates mode="SmartenTable" select="$table">
            <xsl:with-param name="sortColumnNum" select="$sortColumnNum"/>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template mode="SmartenTable" match="@* | node()">
        <xsl:param name="sortColumnNum"/>

        <xsl:copy>
            <xsl:apply-templates mode="SmartenTable" select="@* | node()">
                <xsl:with-param name="sortColumnNum" select="$sortColumnNum"/>
            </xsl:apply-templates>
        </xsl:copy>
    </xsl:template>

    <xsl:template mode="SmartenTable" match="tbody">
        <xsl:param name="sortColumnNum"/>

        <xsl:copy>
            <xsl:perform-sort>
                <xsl:sort select="(td[position()=$sortColumnNum][a]/a | td[position()=$sortColumnNum][not(a)])/lower-case(text())"/>
                <xsl:apply-templates mode="SmartenTable" select="tr"/>
            </xsl:perform-sort>
        </xsl:copy>

    </xsl:template>

    <xsl:template mode="SmartenTable" match="td[../../self::thead]">

        <xsl:variable name="number">
            <xsl:number/>
        </xsl:variable>

        <xsl:if test="ancestor::table/tbody/tr/td[position()=$number][node()]">
            <xsl:copy>
                <xsl:value-of select="@* | node()"/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>

    <xsl:template mode="SmartenTable" match="td[not(node()) and ../../self::tbody]">
        <xsl:variable name="number">
            <xsl:number/>
        </xsl:variable>
        <xsl:if test="ancestor::tbody/tr/td[position()=$number][node()]">
            <xsl:copy>
                <xsl:text>-</xsl:text>
            </xsl:copy>
        </xsl:if>
    </xsl:template>

</xsl:stylesheet>