<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="ModifyInfo">
		<xsl:param name="localName"/>
		<xsl:param name="newInfo"/>
		<xsl:param name="current"/>

		<xsl:apply-templates mode="ModifyFirst" select="$current">
			<xsl:with-param name="localName" select="$localName"/>
			<xsl:with-param name="newInfo" select="$newInfo"/>
		</xsl:apply-templates>

	</xsl:template>

	<xsl:template mode="ModifyFirst" match="@* | node()">
		<xsl:param name="localName"/>
		<xsl:param name="newInfo"/>

		<xsl:copy>
			<xsl:element name="{$localName}">
				<xsl:value-of select="$newInfo"/>
			</xsl:element>
			<xsl:apply-templates mode="ModifyRest" select="@* | node()[local-name() != $localName]"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template mode="ModifyRest" match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates mode="ModifyRest" select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>