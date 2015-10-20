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

    <xsl:template name="Page">
        <xsl:param name="Content-head"/>
        <xsl:param name="Content-body"/>
        <xsl:param name="title"/>
        <xsl:param name="prefix" select="Prefix"/>

        <xsl:call-template name="PostProcessLinks">
            <xsl:with-param name="prefix" select="$prefix"/>
            <xsl:with-param name="article">
                <html>
                    <head>
                        <title>
                            <xsl:value-of select="$title"/>
                        </title>

                        <meta charset="UTF-8"/>

                        <link rel="stylesheet" type="text/css" href="base/Shared/Page/reset.css"/>
                        <link rel="stylesheet" type="text/css" href="base/Shared/Page/Page.css"/>

                        <!--Set global variable for relative prefix.-->
                        <xsl:variable name="apos">'</xsl:variable>
                        <script>
                            <xsl:value-of select="concat( 'globals = { prefix : ', $apos, $prefix, $apos, '};' )"/>
                        </script>

                        <script src="base/Shared/Page/jquery-1.11.1.min.js" type="text/javascript"></script>
                        <script src="base/Shared/Page/Page.js" type="text/javascript"></script>

                        <xsl:call-template name="Header-head"/>
                        <xsl:call-template name="Sections-head"/>
                        <xsl:call-template name="Table-head"/>
                        <xsl:call-template name="Footer-head"/>
                        <xsl:call-template name="Menu-head"/>

                        <xsl:copy-of select="$Content-head"/>
                    </head>
                    <body>
                        <div id="Page">
                            <div class="mainContainer">
                                <xsl:call-template name="Header-body"/>
                                <div id="Article">
                                    <xsl:call-template name="ArticlePostProcess">
                                        <xsl:with-param name="article">
                                            <xsl:copy-of select="$Content-body"/>
                                        </xsl:with-param>
                                    </xsl:call-template>
                                </div>
                                <xsl:call-template name="Footer-body"/>
                            </div>
                            <xsl:call-template name="Menu-body"/>
                        </div>
                        <script src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS-MML_HTMLorMML" type="text/javascript"></script>
                    </body>
                </html>
            </xsl:with-param>
        </xsl:call-template>

    </xsl:template>

</xsl:stylesheet>

















