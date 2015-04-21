<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="MenuEntry" match="*">
		<xsl:param name="prefix"/>
		<xsl:param name="zeroDepth" select="false()"/>
		<xsl:param name="name"/>
		<xsl:param name="locationNode"/>

		<xsl:variable name="groups" as="element()">
			<Groups>
				<xsl:apply-templates mode="SubObjects" select=".">
					<xsl:with-param name="prefix" select="$prefix"/>
				</xsl:apply-templates>
			</Groups>
		</xsl:variable>
		<xsl:variable name="current" select="."/>

		<xsl:if test="$groups/Group">
			<ul>
				<xsl:for-each select="$groups/Group">
					<xsl:variable name="group" select="."/>
					<xsl:variable name="SubObjects" select="$current/*[local-name() = $group/Containers/Name]
												  /*[local-name() = $group/Objects/Name][Access!='Private' or not(Access)]"/>

					<xsl:if test="$SubObjects">
						<li class="subtitle">
							<a>
								<i>
									<xsl:value-of select="Title"/>
								</i>
							</a>
							<ul>
								<xsl:apply-templates mode="MenuItems" select="$SubObjects">
									<xsl:sort select="Identifier"/>
									<xsl:with-param name="prefix" select="$prefix"/>
									<xsl:with-param name="zeroDepth" select="$zeroDepth"/>
									<xsl:with-param name="name" select="$name"/>
									<xsl:with-param name="locationNode" select="$locationNode"/>
								</xsl:apply-templates>
							</ul>
						</li>
					</xsl:if>
				</xsl:for-each>
			</ul>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>















