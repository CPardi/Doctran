<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="AddSection" match="Source">

		<xsl:call-template name="Section">
			<xsl:with-param name="name" select="'Source'"/>
			<xsl:with-param name="id" select="'Source'"/>
			<xsl:with-param name="content">
				<p>The program source shown below is taken from '<xsl:value-of select="ancestor::File/Name"/><xsl:value-of select="ancestor::File/Extension"/>'.
				</p>
				<div class="code">
					<pre class="code-content">
						<code>
							<ul>
								<xsl:apply-templates mode="WriteLine" select="."/>
							</ul>
						</code>
					</pre>
				</div>
			</xsl:with-param>
		</xsl:call-template>

	</xsl:template>

	<xsl:template mode="WriteLine" match="Line[Number mod 2 = 0]">
		<li class="even">
			<xsl:value-of select="Number"/>
			<span class="line">
				<xsl:value-of select="Text"/>
			</span>
		</li>
	</xsl:template>

	<xsl:template mode="WriteLine" match="Line[Number mod 2 != 0]">
		<li class="odd">
			<xsl:value-of select="Number"/>
			<span class="line">
				<xsl:value-of select="Text"/>
			</span>
		</li>
	</xsl:template>

</xsl:stylesheet>