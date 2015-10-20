<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <!-- The default columns to be displayed in tables. -->
    <xsl:template mode="TableColumns" match="*">
        <Columns>
            <SortBy order="ascending">Name</SortBy>
            <Column>
                <Title>Name</Title>
                <Name>Name</Name>
            </Column>
            <Column>
                <Title>Description</Title>
                <Name>Description</Name>
            </Column>
        </Columns>
    </xsl:template>

</xsl:stylesheet>