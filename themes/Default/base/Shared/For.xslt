<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="For">
		<xsl:param name="from" select="1" />
		<xsl:param name="to" />
		<xsl:param name="template"/>
		<xsl:param name="params"/>

		<xsl:if test="$from &lt;= $to">
			<xsl:apply-templates select="$template"/>
			<xsl:call-template name="For">
				<xsl:with-param name="from" select="$from + 1" />
				<xsl:with-param name="to" select="$to" />
				<xsl:with-param name="template" select="$template" />
				<xsl:with-param name="params" select="$params" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>