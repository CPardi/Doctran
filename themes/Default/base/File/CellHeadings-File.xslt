<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!--Provides a template of how the lists of variables should be displayed. -->

	<xsl:template mode="CellHeadings" match="Files">

		<Heading>
			<Text>Name</Text>
		</Heading>

		<Heading>
			<Text>Number of lines</Text>
		</Heading>
		
		<Heading>
			<Text>Date Created</Text>
		</Heading>

		<xsl:if test="File/Description">
			<Heading>
				<Text>Description</Text>
			</Heading>
		</xsl:if>

	</xsl:template>

</xsl:stylesheet>