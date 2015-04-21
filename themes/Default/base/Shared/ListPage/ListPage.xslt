<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="ListPage">
		<xsl:param name="filename"/>
		<xsl:param name="title"/>
		<xsl:param name="objects"/>

		<xsl:variable name="prefix" select="'../../'"/>
		<xsl:result-document href="{concat('html/Navigation/',$filename,'.html')}">

			<xsl:call-template name="Page">
				<xsl:with-param name="title" select="$title"/>
				<xsl:with-param name="prefix" select="$prefix"/>
				<xsl:with-param name="Content-head">
					<xsl:call-template name="Browse-head">
						<xsl:with-param name="prefix" select="$prefix"/>
					</xsl:call-template>				
					<xsl:call-template name="List-head">
						<xsl:with-param name="prefix" select="$prefix"/>
					</xsl:call-template>
					<link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/ListPage/ListPage.css')}" />					
				</xsl:with-param>
				<xsl:with-param name="Content-body">
					<xsl:call-template name="Browse-body">
						<xsl:with-param name="prefix" select="$prefix"/>
						<xsl:with-param name="activeBrowse" select="$objects/local-name()"/>
					</xsl:call-template>
					<div id="ListContent">
						<xsl:call-template name="Section">
							<xsl:with-param name="name" select="$title"/>
							<xsl:with-param name="content">
								<table class="List">
									<xsl:apply-templates mode="List-AddHeading" select="$objects"/>
									<xsl:apply-templates mode="List-AddRows" select="$objects/*[Access != 'Private' or not(Access)]">
										<xsl:sort select="Identifier"/>
										<xsl:with-param name="prefix" select="$prefix"/>
									</xsl:apply-templates>
								</table>
							</xsl:with-param>
						</xsl:call-template>
					</div>					
				</xsl:with-param>
			</xsl:call-template>

		</xsl:result-document>

	</xsl:template>

</xsl:stylesheet>