<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="xs xsl doctran"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:doctran="http://www.doctran.co.uk">

    <xsl:template mode="AddSection" match="Source">
        <xsl:param name="file" as="element()"/>
        <xsl:param name="firstLine" as="xs:integer">0</xsl:param>
        <xsl:param name="lastLine" as="xs:integer">0</xsl:param>

        <xsl:variable name="newline">
            <xsl:text>&#10;</xsl:text>
        </xsl:variable>

        <h2>Source</h2>
        <p>
            <xsl:text>The source code shown below is taken from '</xsl:text>
            <a href="{doctran:object-uri($file)}">
                <xsl:value-of select="doctran:object-name($file)"/>
            </a>
            <xsl:text>'.</xsl:text>
        </p>
        <div class="highlight">
            <table>
                <tbody>
                    <tr>
                        <td class="gutter">
                            <pre>
                                <xsl:for-each select="$firstLine to $lastLine">
                                    <div class="{if(. mod 2 = 0) then 'even' else 'odd'}">
                                        <xsl:value-of select="."/>
                                    </div>
                                </xsl:for-each>
                            </pre>
                        </td>
                        <td>
                            <pre>
                                <code class="language-fortran">
                                    <xsl:apply-templates mode="Source"
                                                         select="File[Identifier = $file/Identifier]/lines/line[@number &gt;= $firstLine and @number &lt;= $lastLine]"/>
                                </code>
                            </pre>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </xsl:template>

    <xsl:template mode="Source" match="line[@number mod 2 = 0]">
        <div class="even">
            <xsl:copy-of select="node()"/>
        </div>
    </xsl:template>

    <xsl:template mode="Source" match="line[@number mod 2 != 0]">
        <div class="odd">
            <xsl:copy-of select="node()"/>
        </div>
    </xsl:template>

</xsl:stylesheet>