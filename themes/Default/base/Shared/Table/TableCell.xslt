<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template mode="TableCell" match="*">
        <xsl:value-of select="node()"/>
    </xsl:template>

    <xsl:template mode="TableCell" match="Name">
        <xsl:param name="prefix"/>
            <a href="{concat($prefix, ../href)}">
                <xsl:apply-templates mode="Name" select=".."/>
            </a>
    </xsl:template>

    <xsl:template mode="TableCell" match="Description">
        <xsl:copy-of select="Basic/node()"/>
    </xsl:template>

</xsl:stylesheet>