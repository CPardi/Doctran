<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="AddSubObjectLists" match="*">
		<xsl:param name="prefix"/>
		<xsl:param name="additionalSubObjects"/>

		<xsl:variable name="groups" as="element()">
			<Groups>
        <xsl:copy-of select="$additionalSubObjects"/>
        <xsl:apply-templates mode="SubObjects" select="."/>
			</Groups>
		</xsl:variable>
		<xsl:variable name="current" select="."/>
		
		<xsl:call-template name="Section">
			<xsl:with-param name="name" select="'Contained Objects'"/>
			<xsl:with-param name="content">
				<xsl:for-each select="$groups/Group">
					<xsl:sort select="position()" data-type="number" order="descending"/>

					<xsl:variable name="group" select="."/>
					<xsl:variable name="subObjects" select="$current/*[local-name() = $group/Containers/Name]
												  /*[local-name() = $group/Objects/Name][Access!='Private' or not(Access)]"/>

					<xsl:if test="$subObjects/.">
						<xsl:call-template name="Subsection">
							<xsl:with-param name="name" select="$group/Title"/>
							<xsl:with-param name="content">
								<table class="List">
									<xsl:apply-templates mode="List-AddHeading" select="$current/*[local-name() = $group/Containers/Name][1]"/>
									<xsl:apply-templates mode="List-AddRows" select="$subObjects">
										<xsl:with-param name="prefix" select="$prefix"/>							
										<xsl:sort select="Name"/>
									</xsl:apply-templates>
								</table>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
			</xsl:with-param>
		</xsl:call-template>

	</xsl:template>

</xsl:stylesheet>