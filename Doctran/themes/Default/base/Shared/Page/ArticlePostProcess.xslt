<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="doctran xs xsl"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:doctran="http://www.doctran.co.uk">

    <xsl:template name="ArticlePostProcess">
        <xsl:param name="article"/>

        <xsl:call-template name="SectionFormatter">
            <xsl:with-param name="article">
                <xsl:apply-templates mode="ArticlePostProcess_Recurse" select="$article">
                    <xsl:with-param name="project" select="/Project"/>
                </xsl:apply-templates>
            </xsl:with-param>
        </xsl:call-template>

    </xsl:template>

    <xsl:template mode="ArticlePostProcess_Recurse" match="table[text() != '' and matches(text(), '^\s*|\s*table\s*,', 'i')]">
        <xsl:param name="project"/>

        <xsl:analyze-string select="lower-case(normalize-space(text()))" regex="^\s*\|\s*table\s*,\s*(\w+?)\s*(,\s*(\w+?)\s*)?\|" flags="i">
            <xsl:matching-substring>

                <xsl:variable name="tableType" select="lower-case(regex-group(1))"/>
                <xsl:variable name="sorttable" select="lower-case(regex-group(3))='sortable'"/>

                <xsl:if test="$project/key($tableType, '|all|')">
                    <xsl:call-template name="Table">
                        <xsl:with-param name="sortBy" select="'Name'"/>
                        <xsl:with-param name="rows"
                                        select="$project/key($tableType, '|all|')[Access != 'Private' or not(Access)]"/>
                        <xsl:with-param name="columns" as="element()">
                            <xsl:apply-templates mode="TableColumns" select="$project/key($tableType, '|all|')[1]"/>
                        </xsl:with-param>
                        <xsl:with-param name="sortable" select="$sorttable"/>
                    </xsl:call-template>
                </xsl:if>
            </xsl:matching-substring>
            <xsl:non-matching-substring>
                <xsl:message select="concat('The macro ', ., ' was not recognised and has been ignored.')"/>
            </xsl:non-matching-substring>
        </xsl:analyze-string>

    </xsl:template>

    <xsl:template mode="ArticlePostProcess_Recurse" match="node() | @*">
        <xsl:param name="project"/>

        <xsl:copy>
            <xsl:apply-templates mode="ArticlePostProcess_Recurse" select="@* | node()">
                <xsl:with-param name="project" select="$project"/>
            </xsl:apply-templates>
        </xsl:copy>

    </xsl:template>

</xsl:stylesheet>

















