<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="DescriptionCell" match="*">
		<xsl:param name="href"/>

		<xsl:if test="../*/Description">
			<Cell>
				<Text>
          <xsl:apply-templates mode="RemoveRoot" select="Description/Basic"/>
				</Text>
				<href>
					<xsl:value-of select="$href"/>
				</href>	
			</Cell>
		</xsl:if>

	</xsl:template>

</xsl:stylesheet>