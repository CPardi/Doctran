<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <!--Include the following files to get used control's head information.-->
    <xsl:include href="MenuRaw.xslt"/>
    <xsl:include href="MenuPostProcess.xslt"/>
    <xsl:include href="MenuLists.xslt"/>

    <xsl:template name="Menu-head">
        <!-- Link to the main menu's stylesheet. -->
        <link rel="stylesheet" type="text/css" href="base/Shared/Menu/doctranMenu/doctranmenu.css"/>
        <script type="text/javascript" src="base/Shared/Menu/doctranMenu/doctranmenu.js"></script>

        <link rel="stylesheet" type="text/css" href="base/Shared/Menu/Menu.css"/>
        <script src="base/Shared/Menu/Menu.js" type="text/javascript"></script>
    </xsl:template>

    <xsl:template name="Menu-body">
        <!-- This is the template for the contents menu that is displayed on all Pages. -->
        <div class="doctran-menu">
            <xsl:call-template name="MenuPostProcess">
                <xsl:with-param name="menu">
                    <xsl:apply-templates mode="MenuRaw" select="/Project/Information/Menu">
                        <xsl:with-param name="current" select="."/>
                    </xsl:apply-templates>
                </xsl:with-param>
            </xsl:call-template>
        </div>
    </xsl:template>

</xsl:stylesheet>















