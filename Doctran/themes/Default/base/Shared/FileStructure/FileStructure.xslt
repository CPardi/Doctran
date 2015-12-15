<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
								xmlns:xs="http://www.w3.org/2001/XMLSchema"
								xmlns:doctran="http://www.doctran.co.uk">

	<xsl:template mode="GeneratePath" match="*">

		<xsl:text>html</xsl:text>
		<xsl:apply-templates mode="Generate-Url-Path" select="." >
			<xsl:with-param name="slash" select="'/'"/>
		</xsl:apply-templates>
        <xsl:text>/</xsl:text>

	</xsl:template>

	<xsl:template mode="GenerateURL" match="*">
		
		<xsl:apply-templates mode="Generate-Url-Path" select="." >
			<xsl:with-param name="slash" select="'/'"/>
		</xsl:apply-templates>
		<xsl:text>.html</xsl:text>

	</xsl:template>

	<xsl:template mode="Generate-Url-Path" match="Project">
		<xsl:text>html</xsl:text>
	</xsl:template>
	
	<xsl:template mode="Generate-Url-Path" match="*">
		<xsl:param name="slash" as="xs:string"/>
		
		<xsl:apply-templates mode="Generate-Url-Path" select="../.." >
			<xsl:with-param name="slash" select="$slash"/>
		</xsl:apply-templates>
		
		<xsl:value-of select="$slash"/>
		<xsl:apply-templates mode="FileName" select="."/>
	</xsl:template>
	
	<xsl:template mode="PathToRoot" match="Project"/>
	
	<xsl:template mode="PathToRoot" match="*">
		<xsl:apply-templates mode="PathToRoot" select="../.." />
		<xsl:text>../</xsl:text>
	</xsl:template>

</xsl:stylesheet>














