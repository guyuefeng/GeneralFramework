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

<xsl:variable name="appPath" select="config/path"/>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="Keywords" content="{setting/keywords}" />
<meta name="Description" content="{setting/intro}" />
<meta name="Author" content="Roc, roc@foxcup.com" />
<meta name="Copyright" content="Copyright (c) Roclog Team 2009-2010. All Rights Reserved." />
<title><xsl:value-of select="this/title"/></title>
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
				<div class="display">
					<div class="displayTop"></div>
					<div class="displayContent">

						<xsl:for-each select="this/articles/item">
						<div class="itemBox">
							<div class="itemTop"></div>
							<div class="itemContent">
								<h1><a href="{link}"><xsl:value-of select="title"/></a></h1>
								<div class="content">
									<div id="articleContentDom_{id}"><xsl:value-of select="explain" disable-output-escaping="yes"/></div>
									<div class="clear"></div>
								</div>
								<div class="info">
									<span class="date"><xsl:value-of select="publish"/>(<xsl:value-of select="author"/>)</span>
									<span class="comm"><a href="{link}#commentBoxTop">抢沙发</a>(<xsl:value-of select="postCount"/>)</span>
									<span class="cate"><a href="{category/link}"><xsl:value-of select="category/name"/></a></span><br/>
									<span class="tags"><xsl:value-of select="fun:TagsLink(tags)" disable-output-escaping="yes"/></span>
									<span class="more"><a href="{link}" title="阅读更多{title}的内容" class="moreLink">阅读全文</a></span>
								</div>
							</div>
							<div class="itemBottom"></div>
						</div>
						</xsl:for-each>

						<!--分页-->
						<div class="pages">
							<xsl:value-of select="this/articles/pages" disable-output-escaping="yes"/>
							<div class="clear"></div>
						</div>
						<!--分页完-->

					</div>
					<div class="displayBottom"></div>
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

<input type="hidden" id="appPath" value="{config/path}"/>
<script type="text/javascript" src="{config/path}Common/Script/jQuery.js"></script>
<script type="text/javascript" src="{config/path}Common/Theme/{setting/theme}/app.js"></script>

<roclog.debug/>

</body>
</html>

	</xsl:template>
</xsl:stylesheet>