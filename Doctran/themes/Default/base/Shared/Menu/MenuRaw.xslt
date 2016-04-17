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

    <!-- Return a raw menu with link given relative to index.html. -->

    <!-- If the ul just contains text, then it is a lonesome macro and a <ul> isnt needed. -->
    <xsl:template mode="MenuRaw" match="Menu">
        <xsl:param name="current" as="element()"/>

        <xsl:apply-templates mode="MenuRaw_recurse" select="node()">
            <xsl:with-param name="current" select="$current" as="element()"/>
        </xsl:apply-templates>

    </xsl:template>

    <!-- List of objects of some type. -->
    <xsl:template mode="MenuRaw_recurse" priority="0" match="macro[@name='list']">
        <xsl:variable name="option1" select="option[1]"/>
        <xsl:variable name="option2" select="option[2]"/>
        <xsl:copy-of select="$staticLists/Element[@type=$option1][@option=$option2]/node()"/>
    </xsl:template>

    <!-- List of sub-objects of the current object, can be recursive or non-recursive. -->
    <xsl:template mode="MenuRaw_recurse" match="macro[@name='list' and option[1]='subblocks']">
        <xsl:param name="current" as="element()"/>
        <xsl:apply-templates mode="SubObjectList" select="$current">
            <xsl:with-param name="maxDepth" select="if(option[2]='recursive')then -1 else 2"/>
        </xsl:apply-templates>
    </xsl:template>

    <!-- List of current object and its sub-objects, can be recursive or non-recursive. -->
    <xsl:template mode="MenuRaw_recurse" match="macro[@name='list' and option[1]='subblocksandself']">
        <xsl:param name="current" as="element()"/>
        <xsl:apply-templates mode="ObjectList" select="$current">
            <xsl:with-param name="maxDepth" select="if(option[2]='recursive')then -1 else 2"/>
        </xsl:apply-templates>
    </xsl:template>

    <!-- List all objects having the same type as the current object, can be recursive or non-recursive. -->
    <xsl:template mode="MenuRaw_recurse" match="macro[@name='list' and option[1]='sametype']">
        <xsl:param name="current" as="element()"/>
        <xsl:variable name="lower-local-name" select="lower-case(local-name($current))"/>
        <xsl:if test="$key-all-names=$lower-local-name">
            <ul>
                <xsl:apply-templates mode="ObjectList" select="key($lower-local-name, '|all|')">
                    <xsl:with-param name="maxDepth" select="if(option[2]='recursive')then -1 else 1"/>
                </xsl:apply-templates>
            </ul>
        </xsl:if>
    </xsl:template>

    <!-- Name macro -->
    <xsl:template mode="MenuRaw_recurse" match="macro[@name='name']">
        <xsl:param name="current" as="element()"/>
        <xsl:apply-templates mode="Name" select="$current"/>
    </xsl:template>

    <!-- Block name macro -->
    <xsl:template mode="MenuRaw_recurse" match="macro[@name='blockname']">
        <xsl:param name="current" as="element()"/>
        <span class="title">
            <xsl:apply-templates mode="BlockName" select="preceding-sibling::text()"/>
            <xsl:apply-templates mode="BlockName" select="$current"/>
            <xsl:apply-templates mode="BlockName" select="following-sibling::text()"/>
        </span>
    </xsl:template>

    <!-- Text nodes grouped with text-replace macros will be processed by the macro matches. -->
    <xsl:template mode="MenuRaw_recurse" match="text()[parent::li[macro[@name='blockname' or @name='name']]]">
    </xsl:template>

    <!-- Text nodes with no text-replace macros will be wrapped in a title <span> -->
    <xsl:template mode="MenuRaw_recurse" match="text()">
        <span class="title">
            <xsl:value-of select="."/>
        </span>
    </xsl:template>

    <!-- Recurse down for unmatched tags. -->
    <xsl:template mode="MenuRaw_recurse" match="@* | *">
        <xsl:param name="current" as="element()"/>
        <xsl:copy>
            <xsl:apply-templates mode="MenuRaw_recurse" select="@* | node()">
                <xsl:with-param name="current" select="$current"/>
            </xsl:apply-templates>
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>