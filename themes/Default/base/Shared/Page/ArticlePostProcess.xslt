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
        <xsl:param name="prefix"/>
        <xsl:param name="article"/>

        <xsl:apply-templates mode="ArticlePostProcess_Recurse" select="$article">
            <xsl:with-param name="project" select="/Project"/>
            <xsl:with-param name="prefix" select="$prefix"/>
        </xsl:apply-templates>

    </xsl:template>

    <xsl:template mode="ArticlePostProcess_Recurse" match="a | link | script | img">
        <xsl:param name="prefix"/>

        <xsl:variable name="linkName" select="doctran:linkName(local-name())"/>
        <xsl:variable name="linkAttr" select="@*[local-name()=$linkName]"/>

        <xsl:copy>
            <!-- If its an absolute URL then leave as is. If its a relative URL then add the prefix to make it relative
            to the index.html. Change any .md extensions to .html.-->
            <xsl:attribute name="{$linkName}"
                           select="replace(if (matches($linkAttr,'^(/|\w+://)')) then $linkAttr else concat($prefix, $linkAttr), '\.md', '.html')"/>
            <xsl:apply-templates mode="ArticlePostProcess_Recurse" select="@*[local-name()!=$linkName] | node()">
                <xsl:with-param name="prefix" select="$prefix"/>
            </xsl:apply-templates>
        </xsl:copy>
    </xsl:template>

    <xsl:template mode="ArticlePostProcess_Recurse" match="table[text() != '' and matches(text(), '^\s*|\s*table\s*,', 'i')]">
        <xsl:param name="project"/>
        <xsl:param name="prefix"/>

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
                        <xsl:with-param name="prefix" select="$prefix"/>
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
        <xsl:param name="prefix"/>

        <xsl:copy>
            <xsl:apply-templates mode="ArticlePostProcess_Recurse" select="@* | node()">
                <xsl:with-param name="project" select="$project"/>
                <xsl:with-param name="prefix" select="$prefix"/>
            </xsl:apply-templates>
        </xsl:copy>

    </xsl:template>

    <xsl:function name="doctran:linkName" as="xs:string">
        <xsl:param name="localName" as="xs:string"/>

        <xsl:choose>
            <xsl:when test="$localName=tokenize('a,link',',')">
                <xsl:value-of select="'href'"/>
            </xsl:when>
            <xsl:when test="$localName=tokenize('img,script',',')">
                <xsl:value-of select="'src'"/>
            </xsl:when>
        </xsl:choose>

    </xsl:function>

</xsl:stylesheet>

















