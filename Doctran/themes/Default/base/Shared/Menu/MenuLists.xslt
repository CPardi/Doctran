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

    <!--STATIC OBJECT LISTs-->

    <xsl:variable name="project" select="/Project"/>
    <xsl:variable name="staticLists" as="element()">
        <Elements>
            <xsl:for-each select="tokenize('project,file,program,module,derivedtype,function,subroutine,assignment,overload,operator,variable', ',')">
                <xsl:variable name="keyName" select="."/>
                <Element type="{$keyName}" is-recursive="0">
                    <ul>
                        <xsl:apply-templates mode="ObjectList" select="$project/key($keyName,'|all|')">
                            <xsl:with-param name="maxDepth" select="1"/>
                        </xsl:apply-templates>
                    </ul>
                </Element>
                <Element type="{$keyName}" is-recursive="1">
                    <ul>
                        <xsl:apply-templates mode="ObjectList" select="$project/key($keyName,'|all|')"/>
                    </ul>
                </Element>
            </xsl:for-each>
        </Elements>
    </xsl:variable>

    <!--DYNAMIC OBJECT LISTs-->

    <!-- Creates a list of the given nodes..-->
    <xsl:template mode="ObjectList" match="*">
        <xsl:param name="depth" select="1" as="xs:integer"/>
        <!-- The current depth of sub-objects, where
            1 - The passed node.
            2 - The passed node's child sub-objects.
            3 - and so on. -->
        <xsl:param name="maxDepth" select="-1" as="xs:integer"/>
        <!-- The maximum depth of sub-objects that should be listed. -->

        <li>
            <a data-parent="{../../doctran:object-name(.)}" data-type="{doctran:block-name(.)}" href="{href}">
                <xsl:apply-templates mode="Name" select="."/>
            </a>
            <ul>
                <xsl:apply-templates mode="SubObjectList" select=".">
                    <xsl:with-param name="depth" select="$depth"/>
                    <xsl:with-param name="maxDepth" select="$maxDepth"/>
                </xsl:apply-templates>
            </ul>
        </li>
    </xsl:template>

    <!-- Creates a list of the sub-objects, belonging to the given node. -->
    <xsl:template mode="SubObjectList" match="*">
        <xsl:param name="depth" select="1" as="xs:integer"/>
        <!-- The current depth of sub-objects, where
            1 - The passed node.
            2 - The passed node's child sub-objects.
            3 - and so on. -->
        <xsl:param name="maxDepth" select="-1" as="xs:integer"/>
        <!-- The maximum depth of sub-objects that should be listed. -->

        <xsl:variable name="groups" as="element()">
            <Groups>
                <xsl:apply-templates mode="SubObjects" select="."/>
            </Groups>
        </xsl:variable>
        <xsl:variable name="current" select="."/>

        <xsl:if test="($depth &lt; $maxDepth or $maxDepth = -1) and $groups/Group and $current/*[local-name() = $groups/Group/Containers/Name]
												/*[local-name() = $groups/Group/Objects/Name][Access!='Private' or not(Access)]">
            <xsl:for-each select="$groups/Group">
                <xsl:variable name="group" select="."/>
                <xsl:variable name="subObjects" select="$current/*[local-name() = $group/Containers/Name]
												  /*[local-name() = $group/Objects/Name][Access!='Private' or not(Access)]"/>

                <xsl:if test="$subObjects">
                    <xsl:call-template name="MenuItem_Subtitle">
                        <xsl:with-param name="value" select="Title"/>
                        <xsl:with-param name="content" as="element()">
                            <ul>
                                <xsl:apply-templates mode="ObjectList" select="$subObjects">
                                    <xsl:with-param name="depth" select="$depth+1"/>
                                    <xsl:with-param name="maxDepth" select="$maxDepth"/>
                                </xsl:apply-templates>
                            </ul>
                        </xsl:with-param>
                    </xsl:call-template>
                </xsl:if>
            </xsl:for-each>
        </xsl:if>
    </xsl:template>

    <!-- Helper templates. -->

    <xsl:template name="MenuItem_Subtitle">
        <xsl:param name="value" as="xs:string"/>
        <xsl:param name="content" as="element()"/>

        <li class="sublist">
            <span>
                <xsl:value-of select="$value"/>
            </span>
            <xsl:copy-of select="$content"/>
        </li>

    </xsl:template>

</xsl:stylesheet>