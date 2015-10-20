<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="Each" match="File">
    
			<xsl:call-template name="EachObject">
				<xsl:with-param name="Object" select="."/>

				<xsl:with-param name="AdditionalContent-body">

					<xsl:apply-templates mode="AddDescription" select="Description"/>

					<xsl:apply-templates mode="AddSubObjectLists" select="."/>
          
				</xsl:with-param>
        
			</xsl:call-template>
		
	</xsl:template>

</xsl:stylesheet>