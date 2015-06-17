<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="OutputProject">

		<xsl:result-document href="index.html">
			<xsl:call-template name="Page">
				<xsl:with-param name="Content-head">
					<xsl:call-template name="Browse-head">
						<xsl:with-param name="prefix" select="Prefix"/>
					</xsl:call-template>
					<xsl:call-template name="AddAuthorsSection-head">
						<xsl:with-param name="prefix" select="Prefix"/>
					</xsl:call-template>
				</xsl:with-param>

				<xsl:with-param name="Content-body">
					<xsl:call-template name="Browse-body">
						<xsl:with-param name="prefix" select="Prefix"/>
					</xsl:call-template>

					<div id="Article">
						<xsl:apply-templates mode="AddDescription" select="Description"/>

						<xsl:variable name="Authors">
							<Authors>
								<xsl:copy-of select="//Author"/>
							</Authors>
						</xsl:variable>
						<xsl:apply-templates mode="AddStatisticsSection" select="/Project"/>
						<xsl:apply-templates mode="AddAuthorsSection" select="$Authors">
							<xsl:with-param name="blockName" select="'project'"/>
						</xsl:apply-templates>
					</div>
				</xsl:with-param>

				<xsl:with-param name="title" select="/Project/Name"/>
				<xsl:with-param name="id-name">
					<xsl:text>Project</xsl:text>
				</xsl:with-param>
				<xsl:with-param name="class-name">
					<xsl:text>NavigationPage</xsl:text>
				</xsl:with-param>				
			</xsl:call-template>
		</xsl:result-document>
	</xsl:template>

</xsl:stylesheet>