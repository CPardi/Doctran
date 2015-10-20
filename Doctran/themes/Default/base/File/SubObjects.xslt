<?xml version="1.0" encoding="UTF-8" ?>
<!--
Copyright © 2015 Christopher Pardi
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
 file, You can obtain one at http://mozilla.org/MPL/2.0/.
-->

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template mode="SubObjects" match="File">

		<Group>
			<Title>Procedures</Title>
			<Containers>
				<Name>Procedures</Name>
			</Containers>
			<Objects>
				<Name>Function</Name>
				<Name>Subroutine</Name>
			</Objects>
		</Group>		
		<Group>
			<Title>Modules</Title>
			<Containers>
				<Name>Modules</Name>
			</Containers>
			<Objects>
				<Name>Module</Name>
			</Objects>
		</Group>
		<Group>
			<Title>Programs</Title>
			<Containers>
				<Name>Programs</Name>
			</Containers>
			<Objects>
				<Name>Program</Name>
			</Objects>
		</Group>			

	</xsl:template>

</xsl:stylesheet>