<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Header-head">
		<xsl:param name="prefix" select="Prefix"/>

		<!-- Link to the Header's stylesheet. -->
		<link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/Header/Header.css')}" />

	</xsl:template>

	<xsl:template name="Header-body">
		<xsl:param name="prefix" select="Prefix"/>

		<div id="Header">
			<h1>
				<a href="{concat($prefix,'index.html')}">
					<xsl:value-of select="/Project/Name"/>
				</a>
        <br/>
        <span>
            <xsl:value-of select="/Project/Description/Basic"/>
        </span>
			</h1>	
		</div>

	</xsl:template>

</xsl:stylesheet>