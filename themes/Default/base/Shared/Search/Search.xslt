<?xml version="1.0" encoding="utf-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="Search-head">
        <xsl:param name="prefix" select="Prefix"/>

        <link rel="stylesheet" type="text/css"
              href="{concat($prefix,'base/Shared/Search/tipuesearch/tipuesearch.css')}"/>
        <link rel="stylesheet" type="text/css" href="{concat($prefix,'base/Shared/Search/Search.css')}"/>

        <script type="text/javascript" src="{concat($prefix,'base/Shared/Search/tipuesearch/tipuesearch_set.js')}"/>
        <script type="text/javascript" src="{concat($prefix,'base/Shared/Search/tipuesearch/tipuesearch_content.js')}"/>
        <script type="text/javascript" src="{concat($prefix,'base/Shared/Search/Search.js')}"/>
        <script type="text/javascript" src="{concat($prefix,'base/Shared/Search/tipuesearch/tipuesearch.js')}"/>

    </xsl:template>

    <xsl:template name="Search-page">

        <xsl:variable name="prefix" select="'../../'"/>

        <xsl:call-template name="TipueContent"/>

        <xsl:result-document href="html/Navigation/Search.html">
            <xsl:call-template name="Page">
                <xsl:with-param name="Content-head">

                    <xsl:call-template name="Browse-head">
                        <xsl:with-param name="prefix" select="$prefix"/>
                    </xsl:call-template>

                </xsl:with-param>

                <xsl:with-param name="Content-body">
                    <xsl:call-template name="Browse-body">
                        <xsl:with-param name="prefix" select="$prefix"/>
                    </xsl:call-template>

                    <div id="PageContent">
                        <h2>Search Results</h2>
                        <div id="tipue_search_content"></div>
                    </div>

                </xsl:with-param>

                <xsl:with-param name="title" select="'Search Results'"/>

                <xsl:with-param name="prefix" select="$prefix"/>
            </xsl:call-template>
        </xsl:result-document>

    </xsl:template>

    <xsl:template name="TipueContent">

        <xsl:result-document method="text" href="base/Shared/Search/tipuesearch/tipuesearch_content.js">
            <xsl:text>var tipuesearch={"pages": [</xsl:text>

            <xsl:apply-templates mode="TipueContentJson"
                                 select="/Project//*[local-name() = /Project/Information/Searchable/Type][not(Access) or Access!='Private']"/>

            <xsl:text>]};</xsl:text>
        </xsl:result-document>

    </xsl:template>

    <xsl:template mode="TipueContentJson" match="*">

        <xsl:variable name="quot">"</xsl:variable>

        <xsl:text>{</xsl:text>

        <xsl:text>"title":"</xsl:text>
        <xsl:apply-templates mode='Name' select='.'/>
        <xsl:text>"</xsl:text>
        <xsl:text>,</xsl:text>

        <xsl:text>"text": "</xsl:text>
        <xsl:text>Type:</xsl:text>
        <xsl:apply-templates mode='BlockName' select='.'/>
        <xsl:value-of select="'&lt;br/&gt;'"/>
        <xsl:copy-of select="Description/Basic/node()"/>
        <xsl:text>"</xsl:text>
        <xsl:text>,</xsl:text>

        <xsl:text>"tags":"</xsl:text>
        <xsl:apply-templates mode='BlockName' select='.'/>
        <xsl:text>"</xsl:text>
        <xsl:text>,</xsl:text>

        <xsl:text>"category":"</xsl:text>
        <xsl:apply-templates mode='BlockName' select='.'/>
        <xsl:text>"</xsl:text>
        <xsl:text>,</xsl:text>

        <xsl:text>"url":</xsl:text>
        <xsl:value-of select='concat("globals.prefix+", $quot, href)'/>
        <xsl:text>"</xsl:text>
        <xsl:text>,</xsl:text>

        <xsl:text>}</xsl:text>
        <xsl:if test="position() != last()">
            <xsl:text>,</xsl:text>
        </xsl:if>
    </xsl:template>

</xsl:stylesheet>