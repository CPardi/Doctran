<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="Table-head">
        <xsl:param name="prefix" select="Prefix"/>

        <link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/Table/Table.css')}"/>

        <script type="text/javascript" src="{concat($prefix, 'base/Shared/Table/Table.js')}"/>
        <script type="text/javascript" src="{concat($prefix, 'base/Shared/Table/sorttable.js')}"/>

    </xsl:template>

    <xsl:template name="Table">
        <xsl:param name="rows"/>
        <xsl:param name="columns" as="element()"/>
        <xsl:param name="prefix"/>
        <xsl:param name="sortBy"/>
        <xsl:param name="sortable" select="false()"/>

		<div class="table-holder">
		    <xsl:call-template name="SmartenTable">
		        <xsl:with-param name="sortColumnNum" select="$columns/*[Name=$sortBy]/position()"/>
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
		                                                <xsl:with-param name="prefix" select="$prefix"/>
		                                                <xsl:with-param name="columnName" select="Name"/>
		                                            </xsl:apply-templates>
		                                        </xsl:when>
		                                        <xsl:when test="$row/*[local-name()=$column/Name][node()]">
		                                            <xsl:apply-templates mode="TableCell" select="$row/*[local-name()=$column/Name]">
		                                                <xsl:with-param name="prefix" select="$prefix"/>
		                                            </xsl:apply-templates>
		                                        </xsl:when>
		                                    </xsl:choose>
		                                </td>
		                            </xsl:for-each>
		                        </tr>
		                    </xsl:for-each>
		                </tbody>
		            </table>
		        </xsl:with-param>
		    </xsl:call-template>
        </div>
        
    </xsl:template>

</xsl:stylesheet>
