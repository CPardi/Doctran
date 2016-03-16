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
            <xsl:apply-templates mode="ObjectList" select="key($lower-local-name, '|all|')">
                <xsl:with-param name="maxDepth" select="if(option[2]='recursive')then -1 else 1"/>
            </xsl:apply-templates>
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
        <xsl:apply-templates mode="BlockName" select="$current"/>
    </xsl:template>

    <xsl:template mode="MenuRaw_recurse" match="@* | *">
        <xsl:param name="current" as="element()"/>
        <xsl:copy>
            <xsl:apply-templates mode="MenuRaw_recurse" select="@* | node()">
                <xsl:with-param name="current" select="$current"/>
            </xsl:apply-templates>
        </xsl:copy>
    </xsl:template>

    <!--<xsl:template mode="MenuRaw_recurse" match="text()">-->
    <!--<xsl:param name="current" as="element()"/>-->
    <!--&lt;!&ndash; This should contain a reference to the current a page is being generated for. &ndash;&gt;-->

    <!--<xsl:analyze-string select="normalize-space(.)" regex="(\|\s*list\s*,\s*(\w+?)\s*(,\s*(\w+?)\s*)?\|)" flags="i">-->
    <!--<xsl:matching-substring>-->
    <!--<xsl:variable name="type" select="lower-case(regex-group(2))"/>-->
    <!--<xsl:variable name="option1" select="lower-case(regex-group(4))"/>-->

    <!--<ul>-->
    <!--&lt;!&ndash; List of sub-objects of the current object. &ndash;&gt;-->
    <!--<xsl:apply-templates mode="SubObjectList" select="$current[$type='subblocks']">-->
    <!--<xsl:with-param name="maxDepth" select="if($option1='recursive')then -1 else 2"/>-->
    <!--</xsl:apply-templates>-->

    <!--&lt;!&ndash; List of current object and its sub-objects. &ndash;&gt;-->
    <!--<xsl:apply-templates mode="ObjectList" select="$current[$type='subblocksandself']">-->
    <!--<xsl:with-param name="maxDepth" select="if($option1='recursive')then -1 else 2"/>-->
    <!--</xsl:apply-templates>-->

    <!--List all objects having the same type as the current object.-->
    <!--<xsl:apply-templates mode="ObjectList"-->
    <!--select="$current[$type='sametype'][lower-case(local-name())=tokenize('project,file,program,module,derivedtype,function,subroutine,assignment,operator,overload',',')]-->
    <!--/key(lower-case(local-name()),'|all|')">-->
    <!--<xsl:with-param name="maxDepth" select="if($option1='recursive')then -1 else 1"/>-->
    <!--</xsl:apply-templates>-->
    <!--</ul>-->

    <!--&lt;!&ndash; List of object of some type. &ndash;&gt;-->
    <!--<xsl:copy-of select="$staticLists[$type!='subobjects']/Element[@type=$type][@option=$option1]/node()"/>-->

    <!--</xsl:matching-substring>-->
    <!--<xsl:non-matching-substring>-->
    <!--<span class="title">-->
    <!--<xsl:analyze-string select="." regex="(\|blockname\|)|(\|name\|)" flags="i">-->
    <!--<xsl:matching-substring>-->
    <!--<xsl:value-of select="doctran:block-name($current)[regex-group(1)!='']"/>-->
    <!--<xsl:value-of select="doctran:object-name($current)[regex-group(2)!='']"/>-->
    <!--</xsl:matching-substring>-->
    <!--<xsl:non-matching-substring>-->
    <!--<xsl:value-of select="."/>-->
    <!--</xsl:non-matching-substring>-->
    <!--</xsl:analyze-string>-->
    <!--</span>-->
    <!--</xsl:non-matching-substring>-->
    <!--</xsl:analyze-string>-->

    <!--</xsl:template>-->

</xsl:stylesheet>