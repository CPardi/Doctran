<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="AddDescription" match="Description">
		<xsl:variable name="Detailed" select="Detailed" />
		<xsl:if test="$Detailed != ''">
			<xsl:call-template name="Section">
				<xsl:with-param name="name" select="'Description'"/>
				<xsl:with-param name="id" select="'Description'"/>
				<xsl:with-param name="content">
					<xsl:apply-templates mode="RemoveRoot" select="$Detailed"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>