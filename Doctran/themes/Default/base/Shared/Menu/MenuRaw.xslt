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
    <xsl:template mode="MenuRaw" match="Menu[ul/text()]">
        <xsl:param name="current" as="element()"/>

        <xsl:apply-templates mode="MenuRaw_recurse" select="ul/node()">
            <xsl:with-param name="current" select="$current" as="element()"/>
        </xsl:apply-templates>

    </xsl:template>

    <!-- If the ul contains list item, then a <ul> is needed to surround them. -->
    <xsl:template mode="MenuRaw" match="Menu[ul/not(text())]">
        <xsl:param name="current" as="element()"/>
        <!-- This should contain a reference to the current a page is being generated for. -->

        <ul>
            <xsl:apply-templates mode="MenuRaw_recurse" select="ul/node()">
                <xsl:with-param name="current" select="$current" as="element()"/>
            </xsl:apply-templates>
        </ul>

    </xsl:template>

    <xsl:template mode="MenuRaw_recurse" match="text()">
        <xsl:param name="current" as="element()"/>
        <!-- This should contain a reference to the current a page is being generated for. -->

        <xsl:analyze-string select="normalize-space(.)" regex="(\|\s*list\s*,\s*(\w+?)\s*(,\s*(\w+?)\s*)?\|)" flags="i">
            <xsl:matching-substring>
                <xsl:variable name="type" select="lower-case(regex-group(2))"/>
                <xsl:variable name="option1" select="lower-case(regex-group(4))"/>

                <ul>
                    <!-- List of sub-objects of the current object. -->
                    <xsl:apply-templates mode="SubObjectList" select="$current[$type='subblocks']">
                        <xsl:with-param name="maxDepth" select="if($option1='recursive')then -1 else 2"/>
                    </xsl:apply-templates>

                    <!-- List of current object and its sub-objects. -->
                    <xsl:apply-templates mode="ObjectList" select="$current[$type='subblocksandself']">
                        <xsl:with-param name="maxDepth" select="if($option1='recursive')then -1 else 2"/>
                    </xsl:apply-templates>

                    <!-- List all objects having the same type as the current object. -->
                    <xsl:apply-templates mode="ObjectList"
                                         select="$current[$type='sametype'][lower-case(local-name())=tokenize('project,file,module,derivedtype,function,subroutine,assignment,operator,overload',',')]
                                                                    /key(lower-case(local-name()),'|all|')">
                        <xsl:with-param name="maxDepth" select="if($option1='recursive')then -1 else 1"/>
                    </xsl:apply-templates>
                </ul>

                <!-- List of object of some type. -->
                <xsl:copy-of select="$staticLists[$type!='subobjects']/Element[@type=$type][@option=$option1]/node()"/>

            </xsl:matching-substring>
            <xsl:non-matching-substring>
                <span class="title">
                    <xsl:analyze-string select="." regex="(\|blockname\|)|(\|name\|)" flags="i">
                        <xsl:matching-substring>
                            <xsl:value-of select="doctran:block-name($current)[regex-group(1)!='']"/>
                            <xsl:value-of select="doctran:object-name($current)[regex-group(2)!='']"/>
                        </xsl:matching-substring>
                        <xsl:non-matching-substring>
                            <xsl:value-of select="."/>
                        </xsl:non-matching-substring>
                    </xsl:analyze-string>
                </span>
            </xsl:non-matching-substring>
        </xsl:analyze-string>

    </xsl:template>

    <xsl:template mode="MenuRaw_recurse" match="@* | *">
        <xsl:param name="current" as="element()"/>
        <!-- This should contain a reference to the current a page is being generated for. -->

        <xsl:copy>
            <xsl:apply-templates mode="MenuRaw_recurse" select="@* | node()">
                <xsl:with-param name="current" select="$current"/>
            </xsl:apply-templates>
        </xsl:copy>

    </xsl:template>

</xsl:stylesheet>