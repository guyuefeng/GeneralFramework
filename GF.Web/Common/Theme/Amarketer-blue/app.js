function initPage()
{
	$(".displayContent img, .articleContent img").each(function()
	{
		if ($(this).width() > 550)
		{
			$(this).width(550);
		}
	});
}
function initReplyClick()
{
	$(".commentReplyClick").click(function()
	{
		$("#content").val(this.title + $("#content").val());
		self.location.href = "#commentPostFocus";
		$("#content").focus();
		return false;
	});
}
function getComments(artId, pg)
{
	$("#commentsDom").html("<div class=\"commentsLoading\">正在载入评论数据, 请稍后...</div>");
	location.href = "#commentBoxTop";
	$.ajax({
		url : $("#appPath").val() + "Service.aspx?act=comment&mode=list&artId=" + artId + "&page=" + pg,
		type : "POST",
		dataType : "xml",
		success : function(data)
		{
			$("#commentsDom").html($(data).find("html").text());
			if ($(data).find("msg").text()) { alert($(data).find("msg").text()); }
			initReplyClick();
		},
		error : function (e) { alert("出错！"); }
	});
}
$(document).ready(function()
{
	initPage();
	$("#commentPostForm").submit(function()
	{
		$("#cmtSubmit").attr("disabled", "disabled");
		var myArtId = $("#artId").val();
		var myName = $("#name").val();
		var myMail = $("#mail").val();
		var myUrl = $("#url").val();
		var myContent = $("#content").val();
		$.ajax({
			url : $("#appPath").val() + "Service.aspx?act=comment&mode=post&artId=" + myArtId,
			type : "POST",
			data : { author : myName, mail : myMail, url : myUrl, content : myContent },
			dataType : "xml",
			success : function(data)
			{
				$("#commentsDom").html($(data).find("html").text());
				if ($(data).find("msg").text() != "") { alert($(data).find("msg").text()); }
				else { location.href = "#commentBoxTop"; }
				$("#cmtSubmit").removeAttr("disabled");
				$("#content").val("");
			},
			error : function (e) { alert("出错"); }
		});
		return false;
	});
	initReplyClick();
});