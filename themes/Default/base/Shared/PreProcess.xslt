<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="Preprocess" match="Project">
		<xsl:call-template name="AddNavigation">
			<xsl:with-param name="project" select="."/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="AddNavigation">
		<xsl:param name="project"/>
		<xsl:apply-templates mode="AddNavigation" select="$project"/>
	</xsl:template>

	<xsl:template mode="AddNavigation" match="File">
		<xsl:copy>
			<xsl:call-template name="addNavigationElements"/>
			<xsl:apply-templates mode="AddNavigation" select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template mode="AddNavigation" match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates mode="AddNavigation" select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="addNavigationElements">
		<Prefix>
			<xsl:apply-templates mode="PathToRoot" select="."/>
		</Prefix>
		<href>
			<xsl:apply-templates mode="GenerateURL" select="."/>
		</href>
	</xsl:template>

</xsl:stylesheet>	