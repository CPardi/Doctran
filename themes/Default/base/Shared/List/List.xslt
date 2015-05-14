<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!--Provides templates of how the lists of objects should be displayed. -->

	<xsl:template name="List-head">
		<xsl:param name="prefix" select="Prefix"/>

		<link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/List/List.css')}" />

    <script type="text/javascript" src="{concat($prefix, 'base/Shared/List/List.js')}"></script>
    <script type="text/javascript" src="{concat($prefix, 'base/Shared/List/sorttable.js')}"></script>
    
  </xsl:template>

	<xsl:template mode="List-AddHeading" match="*">

		<xsl:variable name="Headings" as="element()">
			<Headings>
				<xsl:apply-templates mode="CellHeadings" select="."/>
			</Headings>
		</xsl:variable>
    <thead>
      <tr class="Heading">
			<xsl:apply-templates mode="List-AddCell" select="$Headings/Heading"/>
		  </tr>
    </thead>

	</xsl:template>

	<xsl:template mode="List-AddRows" match="*">
		<xsl:param name="prefix"/>

		<xsl:variable name="Cells" as="element()">
			<Cells>
				<xsl:apply-templates mode="CellFormat" select=".">
					<xsl:with-param name="prefix" select="$prefix"/>
				</xsl:apply-templates>
			</Cells>
		</xsl:variable>
    
		<tr class="Row">
			<xsl:apply-templates mode="List-AddCell" select="$Cells/Cell"/>
		</tr>
    
	</xsl:template>

	<xsl:template mode="List-AddCell" match="Heading">
		<td>
			<a>
				<xsl:for-each select="*[local-name()!='Text']">
					<xsl:attribute name="{local-name()}" select="."/>
				</xsl:for-each>
				<i>
					<xsl:copy-of select="Text"/>
				</i>
			</a>
		</td>
	</xsl:template>

	<xsl:template mode="List-AddCell" match="Cell">
		<td>
			<a>
				<xsl:for-each select="*[local-name()!='Text']">
					<xsl:attribute name="{local-name()}" select="."/>
				</xsl:for-each>

        <xsl:apply-templates mode="RemoveRoot" select="Text"/>
			</a>
		</td>
	</xsl:template>

</xsl:stylesheet>