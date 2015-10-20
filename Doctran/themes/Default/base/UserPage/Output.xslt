<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="OutputUserPages">
        <xsl:apply-templates mode="Each" select="/Project/Information/UserPage"/>
    </xsl:template>

    <xsl:template mode="Each" match="UserPage">

        <xsl:result-document href="{Path}">

            <xsl:call-template name="Page">

                <xsl:with-param name="title">
                    <xsl:value-of select="Content/(h1|h2|h3)[1]"/>
                </xsl:with-param>

                <xsl:with-param name="Content-body" as="element()">
                    <xsl:copy-of select="Content"/>
                </xsl:with-param>

            </xsl:call-template>

        </xsl:result-document>

    </xsl:template>

</xsl:stylesheet>