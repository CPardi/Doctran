<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="doctran xsl"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:doctran="http://www.doctran.co.uk">

    <xsl:template name="Header-head">
        <xsl:param name="prefix" select="Prefix"/>

        <!-- Link to the Header's stylesheet. -->
        <link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/Header/Header.css')}"/>

    </xsl:template>

    <xsl:template name="Header-body">
        <xsl:param name="prefix" select="Prefix"/>

        <div id="Header">
            <h1>
                <a class="title" href="{concat($prefix,'index.html')}">
                    <xsl:value-of select="doctran:object-name(/Project)"/>
                </a>
                <xsl:if test="/Project/Information/Tagline">
                    <a class="tagline" href="{concat($prefix,'index.html')}">
                        <xsl:text> : </xsl:text>
                        <xsl:value-of select="/Project/Information/Tagline"/>
                    </a>
                </xsl:if>
            </h1>
        </div>

    </xsl:template>

</xsl:stylesheet>