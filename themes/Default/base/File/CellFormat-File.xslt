<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!--Provides a template of how the lists of files should be displayed. -->

	<xsl:template mode="CellFormat" match="File">
		<xsl:param name="prefix"/>

		<xsl:variable name="href" select="concat($prefix,href)"/>

		<Cell>
			<Text>
				<xsl:apply-templates mode="Name" select="."/>
			</Text>
			<href>
				<xsl:value-of select="$href"/>
			</href>	
		</Cell>

		<Cell>
			<Text>
				<xsl:value-of select="LineCount"/>
			</Text>
			<href>
				<xsl:value-of select="$href"/>
			</href>	
		</Cell>	

		<Cell>
			<Text>
				<xsl:value-of select="format-dateTime(Created/DateTime,'[D01]/[M01]/[Y0001]')"/>
			</Text>
			<href>
				<xsl:value-of select="$href"/>
			</href>	
		</Cell>	

		<xsl:apply-templates mode="DescriptionCell" select=".">
			<xsl:with-param name="href" select="$href"/>
		</xsl:apply-templates>

	</xsl:template>

</xsl:stylesheet>