<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminCP.aspx.cs" Inherits="GF.Web.AdminCP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
	<meta name="robots" content="none" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title><%#GetTitle()%></title>
	<link href="Common/Admin/Style.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="Common/Script/jQuery.js"></script>
	<script type="text/javascript" src="Common/Editor/CKEditor/ckeditor.js"></script>
</head>
<body>
	<div class="pageBox">
		<div class="header">
			<%#GetTitle()%>
		</div>
		<div class="display">
			<!--左边-->
			<div class="left">
				<asp:Label ID="menuDisplay" runat="server"></asp:Label>
			</div>
			<!--右边-->
			<div class="right">
				<div class="mainBox">
					<asp:Label ID="mainDisplay" runat="server"></asp:Label>
				</div>
			</div>
			<!--清空浮动-->
			<div class="clear"></div>
		</div>
		<!--底部-->
		<div class="foot">
			<p><asp:Label ID="powered" runat="server"></asp:Label></p>
			<p>Processed in <asp:Label ID="debug" runat="server"></asp:Label> seconds</p>
		</div>
	</div>
	<script type="text/javascript" src="Common/Script/jQuery.Cookie.js"></script>
	<script type="text/javascript" src="Common/Admin/Common.js"></script>
</body>
</html>