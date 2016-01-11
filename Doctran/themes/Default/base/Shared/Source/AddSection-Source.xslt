<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="xs"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema">

    <xsl:template mode="AddSection" match="Source">
        <xsl:param name="file" as="element()"/>
        <xsl:param name="firstLine" as="xs:integer">0</xsl:param>
        <xsl:param name="lastLine" as="xs:integer">0</xsl:param>

        <h2>Source</h2>
        <p>
            <xsl:text>The source code shown below is taken from '</xsl:text>
            <a href="{$file/href}">
                <xsl:value-of select="$file/Name"/><xsl:value-of select="$file/Extension"/>
            </a>
            <xsl:text>'.</xsl:text>
        </p>
        <div class="fortran code">
            <code>
                <ul>
                    <xsl:copy-of
                            select="File[Identifier = $file/Identifier]/Lines/div/ul/li[span[@class = 'line-number-span']/text() &gt;= $firstLine][span[@class='line-number-span']/text() &lt;= $lastLine]"/>
                </ul>
            </code>
        </div>
    </xsl:template>

</xsl:stylesheet>