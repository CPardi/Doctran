<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="MenuItems" match="*">
		<xsl:param name="name"/>
		<xsl:param name="prefix" />
		<xsl:param name="zeroDepth" select="false()"/>
		<xsl:param name="locationNode"/>
		<!-- Put a list heading and then add each list item. -->

		<li class="objectEntry">
			<xsl:if test="$locationNode = .">
				<xsl:attribute name="class" select="'objectEntry active'"/>
			</xsl:if>

			<xsl:variable name="href">
				<xsl:value-of select="href"/>
			</xsl:variable>

			<xsl:variable name="blockName">
				<xsl:apply-templates mode="BlockName" select="."/>
			</xsl:variable>

			<xsl:variable name="className" select="lower-case($blockName)"/>

			<xsl:choose>

				<!--When considering a link at top level and referencing the current Page: do not add a href, specify the 
				active class and recurse downwards in the menu-->
				<xsl:when test="$zeroDepth and Name=$name">
					<a class="{$className}">
						<xsl:apply-templates mode="Name" select="."/>
					</a>
					<xsl:apply-templates mode="MenuEntry" select=".">
						<xsl:with-param name="prefix" select="$prefix"/>
						<xsl:with-param name="locationNode" select="$locationNode"/>
					</xsl:apply-templates>
				</xsl:when>

				<!--When considering a link at top level but not referencing the current Page: add a href and specify the nonzeroDepth class. 
				Do not recurse downwards.-->
				<xsl:when test="$zeroDepth and Name!=$name">
					<a class="{$className}" href="{$href}">
						<xsl:apply-templates mode="Name" select="."/>
					</a>
				</xsl:when>

				<!--When considering a link not at top level add a href and recurse down.-->
				<xsl:otherwise>
					<a class="{$className}">
						<xsl:attribute name="href" select="$href"/>
						<xsl:apply-templates mode="Name" select="."/>
					</a>
					<xsl:apply-templates mode="MenuEntry" select=".">
						<xsl:with-param name="prefix" select="$prefix"/>
						<xsl:with-param name="locationNode" select="$locationNode"/>
					</xsl:apply-templates>
				</xsl:otherwise>

			</xsl:choose>

		</li>

	</xsl:template>

	<xsl:template name="RightArrow">
		<span class="rightarrow">
			<xsl:text>#9654;</xsl:text>
		</span>
	</xsl:template>

	<xsl:template name="DownArrow">
		<span class="rightarrow">
			<xsl:text>#9660;</xsl:text>
		</span>
	</xsl:template>

</xsl:stylesheet>















