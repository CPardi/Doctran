<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Browse-head">
		<xsl:param name="prefix" select="Prefix"/>
		<link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/Browse/Browse.css')}" />		
	</xsl:template>

	<xsl:template name="Browse-body">
		<xsl:param name="prefix" select="Prefix"/>
		<xsl:param name="activeBrowse"/>

		<xsl:variable name="browseItems" as="element()">
			<xsl:call-template name="BrowseItems"/>		
		</xsl:variable>

		<div id="Browse">
			<h3>Browse</h3>
			<ul>
				<xsl:for-each select="$browseItems/Item">
					<li>
						<a>
							<xsl:choose>
								<xsl:when test="Name = $activeBrowse">
									<xsl:attribute name="class" select="'active'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="href" select="concat($prefix,href)"/>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:value-of select="Title"/>
						</a>
					</li>
				</xsl:for-each>
			</ul>
		</div>
	</xsl:template>

</xsl:stylesheet>