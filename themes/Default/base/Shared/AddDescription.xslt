<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template mode="AddDescription" match="Description">

    <!--Move into a detailed description tag-->
    <xsl:apply-templates select="Detailed"/>

  </xsl:template>

  <xsl:template match="Detailed/Text">

    <xsl:variable name="detailed">
      <Section>
        <h3>Description</h3>
        <p>
          <xsl:value-of select="."/>
        </p>
      </Section>
    </xsl:variable>

    <xsl:apply-templates mode="sectionFormatter" select="$detailed"/>

  </xsl:template>

  <xsl:template match="Detailed/Html">

    <xsl:apply-templates mode="sectionFormatter" select="."/>

  </xsl:template>

  <!--Description contains headings, write the heading and then a content div.-->
  <xsl:template mode="sectionFormatter" match="node()">

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