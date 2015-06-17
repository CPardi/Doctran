<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="Footer-head">
        <xsl:param name="prefix" select="Prefix"/>

        <!-- Link to the footer's stylesheet. -->
        <link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/Footer/Footer.css')}"/>

    </xsl:template>

    <xsl:template name="Footer-body">
        <div id="Bottom"/>
        <div id="Footer">
            <xsl:text>Documentation created with </xsl:text>
            <a href="http://doctran.co.uk" target="_blank">Doctran</a>
            <xsl:text> at </xsl:text>
            <xsl:value-of select="format-dateTime(/Project/DocCreated/DateTime,'[h1]:[m01] [P] on [D01]/[M01]/[Y0001]')"/>
        </div>
    </xsl:template>

</xsl:stylesheet>