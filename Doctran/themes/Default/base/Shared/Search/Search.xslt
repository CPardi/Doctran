<?xml version="1.0" encoding="utf-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="doctran xsl"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:doctran="http://www.doctran.co.uk">

    <xsl:template name="Search-head">
        <link rel="stylesheet" type="text/css" href="base/Shared/Search/tipuesearch/tipuesearch.css"/>
        <script type="text/javascript" src="base/Shared/Search/tipuesearch/tipuesearch_set.js"></script>
        <script type="text/javascript" src="base/Shared/Search/tipuesearch/tipuesearch_content.js"></script>
        <script type="text/javascript" src="base/Shared/Search/tipuesearch/tipuesearch.min.js"></script>
        <script type="text/javascript" src="base/Shared/Search/Search.js"></script>
    </xsl:template>

    <xsl:template name="Search-page">

        <xsl:call-template name="TipueContent"/>

        <xsl:result-document href="html/Navigation/Search.html">
            <xsl:call-template name="Page">

                <xsl:with-param name="prefix" select="'../../'"/>

                <xsl:with-param name="Content-head">
                    <xsl:call-template name="Search-head"/>
                </xsl:with-param>

                <xsl:with-param name="Content-body">
                    <h1>Search Results</h1>
                    <div id="tipue_search_content"></div>
                </xsl:with-param>

                <xsl:with-param name="title" select="'Search Results'"/>

            </xsl:call-template>
        </xsl:result-document>

    </xsl:template>

    <xsl:template name="TipueContent">

        <xsl:result-document method="text" href="base/Shared/Search/tipuesearch/tipuesearch_content.js">
            <xsl:text>var tipuesearch={"pages": [</xsl:text>

            <xsl:apply-templates mode="TipueContentJson"
                                 select="/Project//*[local-name() = /Project/Information/Searchable/Type][not(Access) or Access!='Private'][href]
                                        |/Project/Information/UserPage"/>

            <xsl:text>]};</xsl:text>
        </xsl:result-document>

    </xsl:template>

    <xsl:template mode="TipueContentJson" match="*">

        <xsl:text>{</xsl:text>

        <xsl:text>"title":</xsl:text>
        <xsl:call-template name="escape-string">
            <xsl:with-param name="s">
                <xsl:apply-templates mode="TipueContent-Title" select="."/>
            </xsl:with-param>
        </xsl:call-template>
        <xsl:text>,</xsl:text>

        <xsl:text>"text":</xsl:text>
        <xsl:call-template name="escape-string">
            <xsl:with-param name="s">
                <xsl:apply-templates mode="TipueContent-Text" select="."/>
            </xsl:with-param>
        </xsl:call-template>
        <xsl:text>,</xsl:text>

        <xsl:text>"tags":</xsl:text>
        <xsl:call-template name="escape-string">
            <xsl:with-param name="s">
                <xsl:apply-templates mode="TipueContent-Tags" select="."/>
            </xsl:with-param>
        </xsl:call-template>

        <xsl:text>,</xsl:text>

        <xsl:text>"category":</xsl:text>
        <xsl:call-template name="escape-string">
            <xsl:with-param name="s">
                <xsl:apply-templates mode="TipueContent-Catagory" select="."/>
            </xsl:with-param>
        </xsl:call-template>
        <xsl:text>,</xsl:text>

        <xsl:text>"url":</xsl:text>
        <xsl:apply-templates mode="TipueContent-URL" select="."/>
        <xsl:text>}</xsl:text>
        <xsl:if test="position() != last()">
            <xsl:text>,</xsl:text>
        </xsl:if>
    </xsl:template>

    <xsl:template mode="TipueContent-Title" match="UserPage">
        <xsl:value-of select="Content/h1[1]"/>
    </xsl:template>

    <xsl:template mode="TipueContent-Text" match="UserPage">
        <xsl:value-of select="Content/p[1]"/>
    </xsl:template>

    <xsl:template mode="TipueContent-Tags" match="UserPage">
    </xsl:template>

    <xsl:template mode="TipueContent-Catagory" match="UserPage">
    </xsl:template>

    <xsl:template mode="TipueContent-Title" match="*">
        <xsl:value-of select="concat('(', doctran:block-name(.),') ', doctran:object-name(.))"/>
    </xsl:template>

    <xsl:template mode="TipueContent-Text" match="*">
        <xsl:value-of select="concat('Parent: ','(', doctran:block-name(../..),') ', doctran:object-name(../..))"/>
        <xsl:value-of select="'&lt;br/&gt;'"/>
        <xsl:value-of select="Description/Basic/node()"/>
    </xsl:template>

    <xsl:template mode="TipueContent-Tags" match="*">
        <xsl:apply-templates mode='BlockName' select='.'/>
    </xsl:template>

    <xsl:template mode="TipueContent-Catagory" match="*">
        <xsl:apply-templates mode='BlockName' select='.'/>
    </xsl:template>

    <xsl:template mode="TipueContent-URL" match="*">
        <xsl:variable name="quot">"</xsl:variable>
        <xsl:value-of select='concat("globals.prefix+", $quot, replace(href, "\\", "/"), $quot)'/>
    </xsl:template>

</xsl:stylesheet>