<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:include href="RunAll.xslt" />

	<xsl:include href="Project/include.xslt" />
	<xsl:include href="File/include.xslt" />
	<xsl:include href="UserPage/include.xslt" />

	<xsl:include href="Shared/AddInfo.xslt" />
	<xsl:include href="Shared/ModifyInfo.xslt" />
	<xsl:include href="Shared/AddDescription.xslt" />
	<xsl:include href="Shared/AddSubObjectLists.xslt" />
	<xsl:include href="Shared/For.xslt" />
	<xsl:include href="Shared/Names-Default.xslt" />
	<xsl:include href="Shared/BrowseItems.xslt" />
	<xsl:include href="Shared/xml2json.xslt" />

	<xsl:include href="Shared/Authors/AddAuthorsSection-head.xslt" />
	<xsl:include href="Shared/Authors/AddAuthorsSection.xslt" />

	<xsl:include href="Shared/Page/Page.xslt" />
	<xsl:include href="Shared/Page/ArticlePostProcess.xslt" />

	<xsl:include href="Shared/EachObject/EachObject.xslt" />

	<xsl:include href="Shared/Sections/Sections-head.xslt" />
	<xsl:include href="Shared/Sections/Section.xslt" />
	<xsl:include href="Shared/Sections/Subsection.xslt" />
	<xsl:include href="Shared/Sections/SectionFormatter.xslt" />

	<xsl:include href="Shared/Header/Header.xslt" />

	<xsl:include href="Shared/Search/Search.xslt" />
	<xsl:include href="Shared/Menu/Menu.xslt" />
	
	<xsl:include href="Shared/Breadcrumbs/Breadcrumbs.xslt" />	

	<xsl:include href="Shared/Footer/Footer.xslt"/>
	<xsl:include href="Shared/FileStructure/FileStructure.xslt" />

	<xsl:include href="Shared/Source/Source-head.xslt" />
	<xsl:include href="Shared/Source/AddSection-Source.xslt" />

    <xsl:include href="Shared/Table/Table.xslt"/>
    <xsl:include href="Shared/Table/TableCell.xslt"/>
    <xsl:include href="Shared/Table/TableColumns.xslt"/>
    <xsl:include href="Shared/Table/SmartenTable.xslt"/>

	<xsl:include href="Shared/Statistics/AddStatisticsSection.xslt" />
    <xsl:include href="Shared/Statistics/StatisticsRows.xslt" />

</xsl:stylesheet>