<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" exclude-result-prefixes="doctran xsl"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:doctran="http://www.doctran.co.uk">

    <xsl:template name="Breadcrumbs-head">
        <link rel="stylesheet" type="text/css" href="base/Shared/Breadcrumbs/Breadcrumbs.css"/>
    </xsl:template>

    <xsl:template name="Breadcrumbs-body">

        <div id="Breadcrumbs">
            <h1>Breadcrumbs</h1>
            <ul>
                <li>
                    <a href="index.html">
                        <xsl:value-of select="doctran:object-name(/Project)"/>
                    </a>
                </li>

                <xsl:apply-templates mode="Breadcrumbs" select=".">
                    <xsl:with-param name="active" select="true()"/>
                </xsl:apply-templates>
            </ul>
        </div>

    </xsl:template>

    <xsl:template mode="Breadcrumbs" match="*">
        <xsl:param name="active" select="false()"/>

        <xsl:variable name="local-name" select="local-name()"/>

        <xsl:if test="$local-name != 'Project'">
            <xsl:apply-templates mode="Breadcrumbs" select="../../."/>
            <li>
                <xsl:if test="$active">
                    <xsl:attribute name="class" select="'active'"/>
                </xsl:if>
                <xsl:if test="Access='Private'">
                    <xsl:attribute name="class" select="'noaccess'"/>
                </xsl:if>
                <span class="separator">
                    <xsl:text>»</xsl:text>
                </span>
                <span class="type">
                    <xsl:value-of select="concat('[', doctran:block-name(.),'] ')"/>
                </span>
                <xsl:choose>
                    <xsl:when test="not($active) and (Access!='Private' or not(Access))">
                        <a>
                            <xsl:attribute name="href" select="doctran:object-uri(.)"/>
                            <xsl:apply-templates mode="Name" select="."/>
                        </a>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:apply-templates mode="Name" select="."/>
                    </xsl:otherwise>
                </xsl:choose>
            </li>
        </xsl:if>

    </xsl:template>

</xsl:stylesheet>