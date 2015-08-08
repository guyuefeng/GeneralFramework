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
<meta name="Keywords" content="{this/article/tags}" />
<meta name="Description" content="{fun:StrCut(fun:ClearHtml(this/article/explain), 200)}" />
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
				<div class="articleBox">
					<div class="articleTop"></div>
					<div class="articleContent">

						<div class="navLine"><a href="{config/path}">首页</a> &gt; <a href="{this/article/category/link}"><xsl:value-of select="this/article/category/name"/></a> &gt; <xsl:value-of select="this/article/title"/></div>
						
						<h1><xsl:value-of select="this/article/title"/></h1>
						<div class="content">
							<p style="float:right; margin:0 0 10px 10px;"><xsl:value-of select="myTag/GoogleAD" disable-output-escaping="yes"/></p>
							<div id="articleContentDom_{this/article/id}"><xsl:value-of select="this/article/content" disable-output-escaping="yes"/></div>
							<div class="clear"></div>
						</div>
						<div class="tags"><xsl:value-of select="fun:TagsLink(this/article/tags)" disable-output-escaping="yes"/></div>
						<div class="other"><a href="{config/path}Service.aspx?act=trackback&amp;id={this/article/id}" target="_blank">引用通告</a>, <a href="{config/path}Service.aspx?act=rss&amp;cid={this/article/category/id}" target="_blank"><xsl:value-of select="this/article/category/name"/>RSS</a></div>

						
						<h1>相关缩略图</h1>
						<xsl:value-of select="fun:GetPostImage(this/article/id, 100, 100, 5, null)" disable-output-escaping="yes"/>
						<br/>
						<br/>
						
						<h1>相关内容</h1>
						<div class="relatedArticle">
						<ul>
						<xsl:value-of select="fun:RelatedArticle(this/article/id,8,50,this/article/tags)" disable-output-escaping="yes"/>
						</ul>
						</div>

						<!--评论列表-->
						<a name="commentBoxTop"></a>
						<div class="commentsBox">
							<div class="commentsTop"></div>
							<div class="commentsContent" id="commentsDom">
								<xsl:value-of select="this/comments/html" disable-output-escaping="yes"/>
							</div>
							<div class="commentsBottom"></div>
						</div>
						<!--评论列表完-->

						<!--评论发布-->
						<div class="postBox">
							<div class="postTop"></div>
							<div class="postContent">
								<a name="commentPostFocus"></a>
								<form method="post" id="commentPostForm">
									<dl>
										<dt>您的昵称</dt>
										<dd><input type="text" name="name" id="name" size="30" value="{fun:GetUserCookie('usrName')}" class="input"/>不管您的姓名有多么龌龊，都请写下来吧</dd>
									</dl>
									<dl>
										<dt>邮箱地址</dt>
										<dd><input type="text" name="mail" id="mail" size="60" value="{fun:GetUserCookie('usrMail')}" class="input"/>能正常联系您的电子信箱地址</dd>
									</dl>
									<dl>
										<dt>个人主页</dt>
										<dd><input type="text" name="url" id="url" size="60" value="{fun:GetUserCookie('usrSite')}" class="input"/>如果有个人主页就留下吧</dd>
									</dl>
									<dl>
										<dt>评论内容</dt>
										<dd><xsl:text disable-output-escaping="yes"><![CDATA[<textarea name="content" id="content" cols="60" rows="10"></textarea>]]></xsl:text></dd>
									</dl>
									<dl>
										<dt>&#30;</dt>
										<dd><input type="submit" id="cmtSubmit" value="发表评论(S)" class="button"/></dd>
									</dl>
									<div class="clear"></div>
								</form>
							</div>
							<div class="postBottom"></div>
						</div>
						<!--评论发布完-->

					</div>
					<div class="articleBottom"></div>
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

<input type="hidden" id="artId" value="{this/article/id}"/>
<input type="hidden" id="appPath" value="{config/path}"/>
<script type="text/javascript" src="{config/path}Common/Script/jQuery.js"></script>
<script type="text/javascript" src="{config/path}Common/Theme/{setting/theme}/app.js"></script>

<roclog.debug/>

</body>
</html>

	</xsl:template>
</xsl:stylesheet>