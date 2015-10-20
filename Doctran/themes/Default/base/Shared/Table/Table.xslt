<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="Table-head">

        <link rel="stylesheet" type="text/css" href="base/Shared/Table/Table.css"/>
        <script type="text/javascript" src="base/Shared/Table/Table.js"></script>
        <script type="text/javascript" src="base/Shared/Table/sorttable.js"></script>

    </xsl:template>

    <xsl:template name="Table">
        <xsl:param name="rows"/>
        <xsl:param name="columns" as="element()"/>
        <xsl:param name="sortBy"/>
        <xsl:param name="sortOrder" select="'ascending'"/>
        <xsl:param name="sortable" select="false()"/>

        <xsl:call-template name="TableHolder">
            <xsl:with-param name="columns" select="$columns"/>
            <xsl:with-param name="sortBy" select="$sortBy"/>
            <xsl:with-param name="sortable" select="$sortable"/>
            <xsl:with-param name="sortOrder" select="$sortOrder"/>
            <xsl:with-param name="content">
                <xsl:call-template name="TableContent">
                    <xsl:with-param name="rows" select="$rows"/>
                    <xsl:with-param name="columns" select="$columns"/>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>

    </xsl:template>

    <xsl:template name="TableHolder">
        <xsl:param name="sortBy"/>
        <xsl:param name="sortOrder" select="'ascending'"/>
        <xsl:param name="sortable" select="false()"/>
        <xsl:param name="columns" as="element()"/>
        <xsl:param name="content"/>

        <div class="tableHolder">
            <xsl:call-template name="SmartenTable">
                <xsl:with-param name="sortColumnNum" select="$columns/*[Name=$sortBy]/position()"/>
                <xsl:with-param name="sortOrder" select="$sortOrder"/>
                <xsl:with-param name="table" as="element()">
                    <table class="{if ($sortable) then 'sortable' else ''}">
                        <thead>
                            <tr>
                                <xsl:for-each select="$columns/*">
                                    <td>
                                        <xsl:value-of select="Title"/>
                                    </td>
                                </xsl:for-each>
                            </tr>
                        </thead>
                        <xsl:copy-of select="$content"/>
                    </table>
                </xsl:with-param>
            </xsl:call-template>
        </div>

    </xsl:template>

    <xsl:template name="TableContent">
        <xsl:param name="rows"/>
        <xsl:param name="columns" as="element()"/>

        <tbody>
            <xsl:for-each select="$rows">
                <xsl:variable name="row" select="." as="element()"/>
                <tr>
                    <xsl:for-each select="$columns/*">
                        <xsl:variable name="column" select="."/>
                        <td>
                            <xsl:choose>
                                <xsl:when test="@type='nodeless'">
                                    <xsl:apply-templates mode="TableNodeless" select="$row">
                                        <xsl:with-param name="columnName" select="Name"/>
                                    </xsl:apply-templates>
                                </xsl:when>
                                <xsl:when test="$row/*[local-name()=$column/Name][node()]">
                                    <xsl:apply-templates mode="TableCell" select="$row/*[local-name()=$column/Name]"/>
                                </xsl:when>
                            </xsl:choose>
                        </td>
                    </xsl:for-each>
                </tr>
            </xsl:for-each>
        </tbody>
    </xsl:template>

</xsl:stylesheet>
