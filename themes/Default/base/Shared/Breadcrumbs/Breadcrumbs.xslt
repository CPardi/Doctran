<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Breadcrumbs-head" >
		<link rel="stylesheet" type="text/css" href="{concat(Prefix,'base/Shared/Breadcrumbs/Breadcrumbs.css')}" />
	</xsl:template>

	<xsl:template name="Breadcrumbs-body">

		<div id="Breadcrumbs">
			<h2>Breadcrumbs</h2>
			<ul>
				<li>
					<a href="{concat(Prefix,'index.html')}">
						<xsl:value-of select="/Project/Name"/>
					</a>
				</li>

				<xsl:apply-templates mode="Breadcrumbs" select=".">
					<xsl:with-param name="prefix" select="Prefix"/>
					<xsl:with-param name="active" select="true()"/>
				</xsl:apply-templates>
			</ul>
		</div>

	</xsl:template>

	<xsl:template mode="Breadcrumbs" match="*">
		<xsl:param name="prefix"/>
		<xsl:param name="active" select="false()"/>

		<xsl:variable name="local-name" select="local-name()" />

		<xsl:if test="$local-name != 'Project'">
			<xsl:apply-templates mode="Breadcrumbs" select="../../.">
				<xsl:with-param name="prefix" select="$prefix"/>
			</xsl:apply-templates>
			<li>
				<xsl:if test="$active">
					<xsl:attribute name="class" select="'active'"/>
				</xsl:if>
                <xsl:if test="Access='Private'">
                    <xsl:attribute name="class" select="'noaccess'"/>
                </xsl:if>
				<a>
					<xsl:if test="not($active) and (Access!='Private' or not(Access))">
						<xsl:attribute name="href" select="concat($prefix,href)"/>
					</xsl:if>
					<xsl:apply-templates mode="Name" select="."/>
				</a>
			</li>
		</xsl:if>

	</xsl:template>

</xsl:stylesheet>