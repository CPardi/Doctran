<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <!--Description contains headings, write the heading and then a content div.-->
    <xsl:template mode="SectionFormatter" match="node()">

        <!-- Form groups that start from a h3 tag and end at the tag prior to the next h3.-->
        <xsl:for-each-group select="node()" group-starting-with="h3">

            <div class="section">
                <!--Copy the group heading-->
                <xsl:copy-of select="."/>
                <div class="content">

                    <!-- Copy the content with the heading removed.-->
                    <xsl:for-each select="current-group()[position() > 1]">
                        <xsl:copy-of select="."/>
                    </xsl:for-each>
                </div>
            </div>
        </xsl:for-each-group>
    </xsl:template>

</xsl:stylesheet>