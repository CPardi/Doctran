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

    <xsl:template match="Detailed[Text]">
        <xsl:variable name="detailed" as="element()">
            <Section>
                <h3>Description</h3>
                <p>
                    <xsl:value-of select="Text"/>
                </p>
            </Section>
        </xsl:variable>
        <xsl:apply-templates mode="SectionFormatter" select="$detailed"/>
    </xsl:template>

    <xsl:template match="Detailed[Html]">
        <xsl:apply-templates mode="SectionFormatter" select="Html"/>
    </xsl:template>

</xsl:stylesheet>