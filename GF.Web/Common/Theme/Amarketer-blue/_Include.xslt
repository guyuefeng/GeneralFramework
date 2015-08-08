<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns="http://www.w3.org/1999/xhtml"
	xmlns:fun="sys:fun"
	>
	<!--头部-->
	<xsl:template name="Header">
		<div class="header">
			<div class="headerTop"></div>
			<div class="headerContent">
				<div class="logo"><xsl:value-of select="setting/name"/></div>
			</div>
			<div class="headerBottom"></div>
		</div>
	</xsl:template>

	<!--菜单-->
	<xsl:template name="Menu">
		<div class="menu">
			<div class="menuTop"></div>
			<div class="menuContent">
				<ul>
					<xsl:for-each select="navigations/item">
					<li><a href="{link}"><xsl:value-of select="name"/></a></li>
					</xsl:for-each>
					<xsl:for-each select="pages/item">
					<li><a href="{link}"><xsl:value-of select="title"/></a></li>
					</xsl:for-each>
					<li><a href="{config/path}tags.aspx">标签</a></li>
					<li><a href="{config/path}fellows.aspx">链接</a></li>
					<li><a href="{config/path}Service.aspx?act=rss" class="rss" target="_blank">订阅</a></li>
					<li><a href="{config/path}AdminCP.aspx">管理</a></li>
				</ul>
			</div>
			<div class="menuBottom"></div>
		</div>
	</xsl:template>

	<!--侧边-->
	<xsl:template name="Side">
		<div class="side">
			<div class="sideTop"></div>
			<div class="sideContent">
				<!--单项-->
				<div class="box">
					<div class="boxTop">系统公告</div>
					<div class="boxContent">
						<xsl:value-of select="setting/affiche" disable-output-escaping="yes"/>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
				<!--单项-->
				<div class="box">
					<div class="boxTop">文章分类</div>
					<div class="boxContent">
						<ol>
							<xsl:for-each select="categorys/item">
							<li><a href="{link}" title="{intro}"><xsl:value-of select="name"/> (<i><xsl:value-of select="postCount"/></i>)</a></li>
							</xsl:for-each>
						</ol>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
				<!--单项-->
				<div class="box">
					<div class="boxTop">搜索</div>
					<div class="boxContent">
						<form class="search" method="get" action="{config/path}default.aspx">
							<input type="hidden" name="act" value="search"/>
							<input type="text" name="key"/><input type="submit" value="搜索(S)"/>
						</form>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
				<!--单项-->
				<div class="box">
					<div class="boxTop">随机文章</div>
					<div class="boxContent">
						<ul>
							<xsl:for-each select="randomArticles/item">
							<li><a href="{link}" title="{title}"><xsl:value-of select="fun:StrCut(title, 34)"/></a></li>
							</xsl:for-each>
						</ul>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
				<!--单项-->
				<div class="box">
					<div class="boxTop">最新评论</div>
					<div class="boxContent">
						<ul>
							<xsl:for-each select="newComments/item">
							<li><a href="{article/link}#{id}" title="{fun:StrCut(fun:ClearHtml(content), 200)}"><xsl:value-of select="fun:StrCut(author, 10)"/>: <xsl:value-of select="fun:StrCut(content, 24)"/></a></li>
							</xsl:for-each>
						</ul>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
				<!--单项-->
				<div class="box">
					<div class="boxTop"><a href="{config/path}tags.aspx" title="标签云集">标签云集</a></div>
					<div class="boxContent">
						<xsl:for-each select="tags/item">
						<a href="{link}"><xsl:value-of select="key"/></a>
						</xsl:for-each>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
				<!--单项-->
				<div class="box">
					<div class="boxTop">站点数据</div>
					<div class="boxContent">
						栏目总数: <xsl:value-of select="fun:GetTableCount('Column')"/> 个<br/>
						页面数量: <xsl:value-of select="fun:GetTableCount('Post')"/> 篇<br/>
						评论数量: <xsl:value-of select="fun:GetTableCount('Comment')"/> 条<br/>
						标签数量: <xsl:value-of select="fun:GetTableCount('Tag') + fun:GetTableCount('MyTag')"/> 个<br/>
						合作伙伴: <xsl:value-of select="fun:GetTableCount('Fellow')"/> 位<br/>
						附件数量: <xsl:value-of select="fun:GetTableCount('Attach')"/> 个<br/>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
				<!--单项-->
				<div class="box">
					<div class="boxTop"><a href="{config/path}fellows.aspx" title="友情链接">友情链接</a></div>
					<div class="boxContent">
						<ol>
							<xsl:for-each select="fellows/item">
							<li><a href="{url}" title="{explain}" target="_blank"><xsl:value-of select="name"/></a></li>
							</xsl:for-each>
						</ol>
					</div>
					<div class="boxBottom"></div>
				</div>
				<!--单项完-->
			</div>
			<div class="sideBottom"></div>
		</div>
	</xsl:template>

	<!--底部-->
	<xsl:template name="Footer">
		<div class="footer">
			<div class="footerTop"></div>
			<div class="footerContent">
				<p><xsl:value-of select="config/powered" disable-output-escaping="yes"/></p>
				<p>Copyright © 2009-2010 <a href="{setting/url}" target="_blank"><xsl:value-of select="setting/name"/></a>. All rights reserved.</p>
				<p><a href="http://www.miibeian.gov.cn/" target="_blank"><xsl:value-of select="setting/icp"/></a></p>
				<xsl:value-of select="myTag/FootInfo" disable-output-escaping="yes"/>
			</div>
			<div class="footerBottom"></div>
		</div>
		<script type="text/javascript" src="{config/path}Common/Editor/syntaxhighlighter/SyntaxHighlighter.js"></script>
		<script type="text/javascript">
		dp.SyntaxHighlighter.HighlightAll('code');
		</script>
	</xsl:template>
</xsl:stylesheet>