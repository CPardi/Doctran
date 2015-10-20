<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<!-- Applies all the templates within the project. -->
	<xsl:import href="include.xslt" />

	<xsl:output method="html" indent="no" doctype-system="about:legacy-compat" encoding="utf-8"/>

	<xsl:variable name="modifiedProject">
		<xsl:apply-templates mode="Preprocess" select="Project"/>
	</xsl:variable>

	<xsl:variable name="MenuStore" as="element()">
		<xsl:apply-templates mode="MenuEntry" select="$modifiedProject/Project">
		</xsl:apply-templates>
	</xsl:variable>

	<xsl:template match="/">

		<xsl:apply-templates mode="RunAll" select="$modifiedProject/Project"/>

	</xsl:template>

</xsl:stylesheet>