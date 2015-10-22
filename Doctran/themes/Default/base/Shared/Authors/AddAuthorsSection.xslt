<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template mode="AddAuthorsSection" match="Authors">
        <xsl:param name="blockName"/>

        <xsl:if test="Author">
            <h2>Authors</h2>
            <div id="Authors">
                <p>
                    <xsl:text>This </xsl:text>
                    <xsl:value-of select="lower-case($blockName)"/>
                    <xsl:text> contains contributions from</xsl:text>
                </p>

                <xsl:for-each-group select="Author" group-by="Name">
                    <xsl:sort select="concat(Rank,Name)"/>
                    <a>
                        <xsl:if test="Email">
                            <xsl:attribute name="href" select="concat('mailto:',Email)"/>
                        </xsl:if>
                        <xsl:value-of select="Name"/>
                    </a>
                    <xsl:if test="Affiliation">
                        <xsl:text> (</xsl:text>
                        <xsl:value-of select="Affiliation"/>
                        <xsl:text>)</xsl:text>
                    </xsl:if>

                    <xsl:choose>
                        <xsl:when test="position() &lt; last()-1">
                            <xsl:text>, </xsl:text>
                        </xsl:when>
                        <xsl:when test="position() = last()-1">
                            <xsl:text> and </xsl:text>
                        </xsl:when>
                    </xsl:choose>
                </xsl:for-each-group>
                <xsl:text>.</xsl:text>
            </div>
        </xsl:if>
    </xsl:template>

</xsl:stylesheet>