<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template mode="AddSubObjectLists" match="*">
        <xsl:param name="prefix"/>
        <xsl:param name="additionalSubObjects" as="element()">
            <Groups/>
        </xsl:param>

        <xsl:variable name="groups" as="element()">
            <Groups>
                <xsl:variable name="allSubObjects" as="element()">
                    <Groups>
                        <xsl:copy-of select="$additionalSubObjects/*"/>

                        <xsl:perform-sort>
                            <xsl:sort select="position()" data-type="number" order="descending"/>
                            <xsl:apply-templates mode="SubObjects" select="."/>
                        </xsl:perform-sort>
                    </Groups>
                </xsl:variable>

                <xsl:for-each-group group-by="Title" select="$allSubObjects/Group">
                    <Group>
                        <Title>
                            <xsl:value-of select="current-grouping-key()"/>
                        </Title>
                        <Containers>
                            <xsl:copy-of select="current-group()/Containers/Name"/>
                        </Containers>
                        <Objects>
                            <xsl:copy-of select="current-group()/Objects/Name"/>
                        </Objects>
                    </Group>
                </xsl:for-each-group>
            </Groups>
        </xsl:variable>

        <xsl:variable name="current" select="."/>

        <xsl:for-each select="$groups/Group">

            <xsl:variable name="group" select="."/>
            <xsl:variable name="objects_group" select="$current/*[local-name() = $group/Containers/Name]
												  /*[local-name() = $group/Objects/Name][Access!='Private' or not(Access)]"/>
            <xsl:if test="$objects_group/*">

                <xsl:variable name="columns" as="element()">
                    <xsl:apply-templates mode="TableColumns" select="$objects_group[1]"/>
                </xsl:variable>

                <h2>
                    <xsl:value-of select="$group/Title"/>
                </h2>
                <xsl:call-template name="Table">
                    <xsl:with-param name="rows" select="$objects_group"/>
                    <xsl:with-param name="columns" select="$columns"/>
                    <xsl:with-param name="prefix" select="$prefix"/>
                    <xsl:with-param name="sortBy" select="$columns/SortBy"/>
                    <xsl:with-param name="sortable" select="true()"/>
                </xsl:call-template>
            </xsl:if>
        </xsl:for-each>

    </xsl:template>

</xsl:stylesheet>