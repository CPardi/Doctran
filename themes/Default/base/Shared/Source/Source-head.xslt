<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Source-head">
		<xsl:param name="prefix" select="Prefix"/>
		<link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/Source/Source.css')}" />
	</xsl:template>

</xsl:stylesheet>