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
        <xsl:param name="prefix"/>
        <xsl:param name="file" as="element()"/>
        <xsl:param name="firstLine" as="xs:integer">0</xsl:param>
        <xsl:param name="lastLine" as="xs:integer">0</xsl:param>

        <xsl:call-template name="Section">
            <xsl:with-param name="name" select="'Source'"/>
            <xsl:with-param name="id" select="'Source'"/>
            <xsl:with-param name="content">
                <p>The program source shown below is taken from '
                    <a href="{concat($prefix, $file/href)}">
                        <xsl:value-of select="$file/Name"/><xsl:value-of select="$file/Extension"/>
                    </a>
                    '.
                </p>
                <div class="fortran code">
                    <code>
                        <ul>
                            <xsl:copy-of
                                    select="File[Name = $file/Name]/Lines/div/ul/li[span[@class = 'line-number-span']/text() &gt;= $firstLine][span[@class='line-number-span']/text() &lt;= $lastLine]"/>
                        </ul>
                    </code>
                </div>
            </xsl:with-param>
        </xsl:call-template>
    </xsl:template>

</xsl:stylesheet>