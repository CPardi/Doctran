<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template mode="AddStatisticsSection" match="Project">

        <xsl:variable name="stats" as="element()">
            <xsl:call-template name="StatisticsRows"/>
        </xsl:variable>

        <xsl:call-template name="Section">
            <xsl:with-param name="name" select="'Statistics'"/>
            <xsl:with-param name="content">
                <p>The table below shows some basic statistics of the project.</p>
                <xsl:call-template name="Table">
                    <xsl:with-param name="rows" select="$stats/*"/>
                    <xsl:with-param name="columns" as="element()">
                        <Columns>
                            <Column>
                                <Title>Statistic</Title>
                                <Name>Name</Name>
                            </Column>
                            <Column>
                                <Title>Value</Title>
                                <Name>Value</Name>
                            </Column>
                        </Columns>
                    </xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>

    </xsl:template>

    <xsl:template mode="TableCell" match="Name[parent::Stat]">
        <xsl:value-of select="."/>
    </xsl:template>

</xsl:stylesheet>