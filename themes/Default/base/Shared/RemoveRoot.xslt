<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<!-- template for the document element -->
	<xsl:template mode="RemoveRoot" match="*">
		<xsl:apply-templates mode="RemoveRoot-Recur" select="node()" />
	</xsl:template>	

	<!-- identity template -->
	<xsl:template mode="RemoveRoot-Recur" match="node() | @*">
		<xsl:copy>
			<xsl:apply-templates mode="RemoveRoot-Recur" select="node() | @*" />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>