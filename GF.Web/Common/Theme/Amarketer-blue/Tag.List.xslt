<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns="http://www.w3.org/1999/xhtml"
	xmlns:fun="roclog:function"
	>
	<xsl:output method="xhtml" version="1.0" omit-xml-declaration="yes" encoding="utf-8" media-type="text/html" indent="yes" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>
	<xsl:include href="_Include.xslt"/>
	<xsl:template match="ui">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="Content-Language" content="{setting/language}" />
<meta name="Keywords" content="标签云集" />
<meta name="Description" content="{setting/name}标签云集页" />
<meta name="Author" content="Roc, roc@foxcup.com" />
<meta name="Copyright" content="Copyright (c) Roclog Team 2009-2010. All Rights Reserved." />
<title><xsl:value-of select="this/title"/> - <xsl:value-of select="setting/name"/></title>
<link href="{config/path}Common/Style/System.css" rel="stylesheet" type="text/css" />
<link href="{config/path}Common/Theme/{setting/theme}/Style/Layout.css" rel="stylesheet" type="text/css" />
</head>

<body>
<div class="page">
	<div class="pageTop"></div>
	<div class="pageContent">
 		<!--头部-->
		<xsl:call-template name="Header"/>
		<!--头部完-->
		
		<!--中间部分-->
		<div class="middle">
			<div class="middleTop"></div>
			<div class="middleContent">
		
				<!--菜单-->
				<xsl:call-template name="Menu"/>
				<!--菜单完-->

				<!--主显示-->
				<div class="tagsBox">
					<div class="tagsTop"></div>
					<div class="tagsContent">

						<h1>标签云集</h1>
						<div class="content">
							<xsl:for-each select="this/tags/item">
							<span class="tagItem"><a href="{link}"><xsl:value-of select="key"/></a>(<xsl:value-of select="postCount"/>)</span>
							</xsl:for-each>
							<div class="clear"></div>
						</div>
					</div>
					<div class="tagsBottom"></div>
				</div>
				<!--主显示完-->
				
				<!--侧边-->
				<xsl:call-template name="Side"/>
				<!--侧边完-->
				
			</div>
			<div class="middleBottom"></div>
		</div>
		<!--中间部分完-->

		<!--底部-->
		<xsl:call-template name="Footer"/>
		<!--底部完-->
		
	</div>
	<div class="pageBottom"></div>
</div>

<roclog.debug/>

</body>
</html>

	</xsl:template>
</xsl:stylesheet>