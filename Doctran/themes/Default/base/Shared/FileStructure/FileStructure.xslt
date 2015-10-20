<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="GeneratePath" match="*">

		<xsl:text>html</xsl:text>
		<xsl:apply-templates mode="Generate-Url-Path" select="Name" >
			<xsl:with-param name="slash" select="'/'"/>
		</xsl:apply-templates>
        <xsl:text>/</xsl:text>

	</xsl:template>

	<xsl:template mode="GenerateURL" match="*">

		<xsl:text>html</xsl:text>
		<xsl:apply-templates mode="Generate-Url-Path" select="Name" >
			<xsl:with-param name="slash" select="'/'"/>
		</xsl:apply-templates>
		<xsl:text>.html</xsl:text>

	</xsl:template>

	<xsl:template mode="Generate-Url-Path" match="Name">
		<xsl:param name="slash"/>

		<xsl:apply-templates mode="Generate-Url-Path" select="../../../Name" >
			<xsl:with-param name="slash" select="$slash"/>
		</xsl:apply-templates>

		<xsl:if test="../../../Name">
			<xsl:value-of select="$slash"/>
			<xsl:apply-templates mode="FileName" select=".."/>
		</xsl:if>

	</xsl:template>

	<xsl:template mode="PathToRoot" match="*">

		<xsl:apply-templates mode="PathToRoot-Name" select="Name" />

	</xsl:template>
	<xsl:template mode="PathToRoot-Name" match="Name">

		<xsl:apply-templates mode="PathToRoot-Name" select="../../../Name" />
		<xsl:if test="../../../Name">
			<xsl:text>../</xsl:text>
		</xsl:if>	

	</xsl:template>

</xsl:stylesheet>














