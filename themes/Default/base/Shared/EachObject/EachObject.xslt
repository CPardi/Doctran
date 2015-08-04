<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="EachObject">
		<xsl:param name="Object"/>
		<xsl:param name="AdditionalContent-head"/>
		<xsl:param name="AdditionalContent-body"/>

		<xsl:if test="$verbose >= 3">
			<xsl:message select="concat('Outputting: ',ancestor::File/Name, ancestor::File/Extension, ' : ', local-name(),' - ',Name,Extension)" />
		</xsl:if>

		<xsl:apply-templates mode="EachObject" select="$Object">
			<xsl:with-param name="AdditionalContent-head" select="$AdditionalContent-head"/>
			<xsl:with-param name="AdditionalContent-body" select="$AdditionalContent-body"/>
		</xsl:apply-templates>

	</xsl:template>

	<xsl:template mode="EachObject" match="*">
		<xsl:param name="AdditionalContent-head"/>
		<xsl:param name="AdditionalContent-body"/>

		<xsl:result-document href="{href}">
			<xsl:call-template name="Page">

				<xsl:with-param name="title">
					<xsl:apply-templates mode="BlockName" select="."/>
					<xsl:value-of select="concat(': ',Name)"/>
				</xsl:with-param>

				<xsl:with-param name="id-name">
					<xsl:apply-templates mode="IdName" select="."/>
				</xsl:with-param>

				<xsl:with-param name="Content-head">
					<link rel="stylesheet" type="text/css" href="{concat(Prefix,'base/Shared/EachObject/EachObject.css')}" />
					<link rel="stylesheet" type="text/css" href="{concat(Prefix,'base/Shared/Source/Source.css')}" />          
					<xsl:call-template name="Browse-head">
						<xsl:with-param name="prefix" select="Prefix"/>
					</xsl:call-template>

					<xsl:call-template name="Breadcrumbs-head"/>
					<xsl:call-template name="Menu-head"/>
					<xsl:call-template name="AddAuthorsSection-head"/>
					<xsl:copy-of select="$AdditionalContent-head"/>
				</xsl:with-param>

				<xsl:with-param name="Content-body">

					<xsl:call-template name="Browse-body">
						<xsl:with-param name="prefix" select="Prefix"/>
					</xsl:call-template>	

					<div id="Article">

						<xsl:call-template name="Breadcrumbs-body"/>

						<h2 id="Content-Title">
							<span id="Object-Type">
								<xsl:apply-templates mode="BlockName" select="."/>
							</span>	
							<span class="seperator">
								<xsl:text>: </xsl:text>
							</span>
							<span id="Object-Name">
								<xsl:apply-templates mode="Name" select="."/>
							</span>							
						</h2>
						<div id="Short-Description">
							<span>
								<xsl:copy-of select="Description/Basic/node()"/>
							</span>
						</div>

						<xsl:copy-of select="$AdditionalContent-body"/>

						<xsl:if test="local-name() = /Project/Information/ShowSource/Type">
							<xsl:apply-templates mode="AddSection" select="$source">
								<xsl:with-param name="prefix" select="Prefix" />
								<xsl:with-param name="file" select=".[local-name()='File']|ancestor::File"/>
								<xsl:with-param name="firstLine" select="Lines/First"/>
								<xsl:with-param name="lastLine" select="Lines/Last"/>
							</xsl:apply-templates>
						</xsl:if>

						<xsl:variable name="Authors">
							<Authors>
								<xsl:copy-of select="Information/Author"/>
							</Authors>
						</xsl:variable>
						<xsl:apply-templates mode="AddAuthorsSection" select="$Authors">
							<xsl:with-param name="blockName">
								<xsl:apply-templates mode="BlockName" select="."/>
							</xsl:with-param>
						</xsl:apply-templates>
					</div>
				</xsl:with-param>

			</xsl:call-template>
		</xsl:result-document>
	</xsl:template>

</xsl:stylesheet>