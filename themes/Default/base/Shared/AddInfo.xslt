<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="InsertInfo">
		<!-- Inserts extra information within the root of a node. -->
		
		<xsl:param name="current"/>
		<!-- The node to insert information within. -->
		<xsl:param name="info" />
		<!--The information to be inserted. -->

		<xsl:apply-templates mode="AddFirst" select="$current">
			<xsl:with-param name="info" select="$info"/>
		</xsl:apply-templates>

	</xsl:template>
	<xsl:template mode="AddFirst" match="@* | node()">
		<xsl:param name="info" />

		<xsl:copy>
			<xsl:copy-of copy-namespaces="no" select="$info"/>
			<xsl:apply-templates mode="AddRest" select="@* | node()"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template mode="AddRest" match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates mode="AddRest" select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>