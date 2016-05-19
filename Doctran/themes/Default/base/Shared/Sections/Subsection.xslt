<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Subsection">
		<xsl:param name="name"/>
		<xsl:param name="content"/>
		<xsl:param name="id"/>
		<xsl:param name="class"/>

		<div class="subsection">
			<xsl:if test="$id">
				<xsl:attribute name="id" select="$id"/>
			</xsl:if>
			<xsl:if test="$class">
				<xsl:attribute name="class" select="concat('subsection ', $class)"/>
			</xsl:if>
			<h3>
				<xsl:copy-of select="$name/node()"/>
			</h3>
			<div class="content">
				<xsl:copy-of select="$content"/>
			</div>
		</div>

	</xsl:template>

</xsl:stylesheet>