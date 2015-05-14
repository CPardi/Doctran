<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<!--Include the following files to get used control's head information.-->
	<xsl:include href="MenuEntry.xslt" />
	<xsl:include href="MenuItems.xslt" />

	<xsl:template name="Menu-head">

		<script src="{concat(Prefix,'base/Shared/treeview/jquery.treeview.js')}" type="text/javascript"/>	
		<!-- Link to the main menu's stylesheet. -->
		<link rel="stylesheet" type="text/css" href="{concat(Prefix,'base/Shared/Menu/Menu.css')}" />
		<script src="{concat(Prefix,'base/Shared/Menu/Menu.js')}" type="text/javascript"/>				
		<!-- Include treeview control. -->
		<!--<xsl:call-template name="treeview-head"/>-->

		<!-- Include perfect-scrollbar control. -->	
		<!--<xsl:call-template name="perfect-scrollbar-head"/>-->

	</xsl:template>

  <xsl:template name="Menu-body">
    <xsl:param name="prefix"/>
    <!-- This is the template for the contents menu that is displayed on all Pages. -->
    <div id="Menu">
      <div id="Heading">
        <h2>
          <xsl:text>Menu</xsl:text>
        </h2>
      </div>
      <div id="Holder">

        <xsl:variable name="menuBackTrack" as ="element()">
          <Items>
            <xsl:apply-templates mode="MenuBackTrack" select=".">
              <xsl:with-param name="extraClass" select="'active'"/>
            </xsl:apply-templates>
          </Items>
        </xsl:variable>
        
        <xsl:apply-templates mode="MenuPost" select="$MenuStore">
          <xsl:with-param name="prefix" select="$prefix"/>
          <xsl:with-param name="menuBackTrack" select="$menuBackTrack"/>
          <xsl:with-param name="count" select="2"/>
        </xsl:apply-templates>
      </div>
    </div>
  </xsl:template>

  <xsl:template mode="MenuBackTrack" match="*">
    <xsl:param name="extraClass"/>
    
    <xsl:if test="local-name() != 'Project'">
      <xsl:apply-templates mode="MenuBackTrack" select="../.."/>
    </xsl:if>
    <Item>
      <Name>
        <xsl:apply-templates mode="Name" select="."/>
      </Name>
      <ExtraClass>
        <xsl:value-of select="$extraClass"/>
      </ExtraClass>    
  </Item>

  </xsl:template>

  <xsl:template mode="MenuPost" match="node() | @*">
    <xsl:param name="prefix"/>
    <xsl:param name="menuBackTrack"/>
    <xsl:param name="count"/>
     
        <xsl:copy>
          <xsl:apply-templates mode="MenuPost" select="node() | @*">
            <xsl:with-param name="prefix" select="$prefix"/>
            <xsl:with-param name="menuBackTrack" select="$menuBackTrack"/>
            <xsl:with-param name="count" select="$count"/>
          </xsl:apply-templates>
        </xsl:copy>

  </xsl:template>

  <xsl:template mode="MenuPost" match="li">
    <xsl:param name="prefix"/>
    <xsl:param name="menuBackTrack"/>
    <xsl:param name="count"/>

    <xsl:choose>
      <xsl:when test="a = $menuBackTrack/Item[$count]/Name">
        <xsl:copy>
          <xsl:attribute name="class" select="concat(@class,' breadcrumbs ',$menuBackTrack/Item[$count]/ExtraClass)"/>
          <xsl:apply-templates mode="MenuPost" select="node()|@*[name()!='class']">
            <xsl:with-param name="prefix" select="$prefix"/>
            <xsl:with-param name="menuBackTrack" select="$menuBackTrack"/>
            <xsl:with-param name="count" select="$count+1"/>
          </xsl:apply-templates>
        </xsl:copy>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy>
          <xsl:apply-templates mode="MenuPost" select="node() | @*">
            <xsl:with-param name="prefix" select="$prefix"/>
            <xsl:with-param name="menuBackTrack" select="$menuBackTrack"/>
            <xsl:with-param name="count" select="$count"/>
          </xsl:apply-templates>
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
      
  </xsl:template>
  
  <xsl:template mode="MenuPost" match="*/@href">
    <xsl:param name="prefix"/>
    
    <xsl:attribute name="href">
      <xsl:value-of select="$prefix"/>
      <xsl:value-of select="."/>
    </xsl:attribute>
    
  </xsl:template>
  
</xsl:stylesheet>















